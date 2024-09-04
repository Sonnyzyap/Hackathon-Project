// 参考サイト：https://tanisugames.com/unity-google-spreadsheet-integration/

using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;
using JetBrains.Annotations;


public class WaterQualityOthers
{
    public string SERIES; // 系
    public string NH4, NO2, NO3; // 1週間に1回ほど測定するデータ
}

public class DataControllerOthers : MonoBehaviour
{
    [SerializeField] TMPro.TMP_Text viewText;
    [SerializeField] Button dataButton;
    [SerializeField] TMP_InputField seriesField, nh4Field, no2Field, no3Field;

    // スプレッドシートの読み取りURL
    // url_wq_everyday = "https://docs.google.com/spreadsheets/d/10nx4vfJyCYENE46bAYJQybzY0Ogyjvmi_buAM4pbHhU/gviz/tq?tqx=out:csv&sheet=water_quality_everyday";
    string url_wq_others = "https://docs.google.com/spreadsheets/d/10nx4vfJyCYENE46bAYJQybzY0Ogyjvmi_buAM4pbHhU/gviz/tq?tqx=out:csv&sheet=water_quality_others";

    // Google Apps ScriptのWebアプリURL
    string gasUrl = "https://script.google.com/macros/s/AKfycbwekuIf9FR2Phpw7bsBQUiVW3VwiPP-7AnLJEOFQJy6UUDxXrPkCa_XJ_4n5eeycrJnLg/exec";

    List<string> datas = new List<string>();

    void Start()
    {
        StartCoroutine(GetData_wq_everyday());

        // ボタンクリック時の挙動
        dataButton.onClick.AddListener(() => StartCoroutine(PostData()));
    }

    // スプレッドシートからデータを取得する関数
    IEnumerator GetData_wq_everyday()
    {
        using (UnityWebRequest req = UnityWebRequest.Get(url_wq_others))
        {
            yield return req.SendWebRequest();

            if (IsWebRequestSuccessful(req))
            {
                ParseData(req.downloadHandler.text);
                DisplayText();
            }
            else
            {
                Debug.Log("error");
            }
        }
    }

    // データを整形する関数
    void ParseData(string csvData)
    {
        string[] rows = csvData.Split(new[] { "\n" }, System.StringSplitOptions.RemoveEmptyEntries);
        foreach (string row in rows)
        {
            string[] cells = row.Split(new[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries);
            foreach (string cell in cells)
            {
                string trimCell = cell.Trim('"');
                if (!string.IsNullOrEmpty(trimCell))
                {
                    datas.Add(trimCell);
                }
            }
        }
    }

    // データを表示する関数
    void DisplayText()
    {
        foreach (string data in datas)
        {
            viewText.text += data + "\n";
        }
    }

    // リクエストが成功したかどうかを判定する関数
    bool IsWebRequestSuccessful(UnityWebRequest req)
    {
        return req.result != UnityWebRequest.Result.ProtocolError &&
               req.result != UnityWebRequest.Result.ConnectionError;
    }

    // JSON形式でデータを送信する関数
    IEnumerator PostData()
    {
        // 入力フィールドから情報を取得
        string seriesText = seriesField.text;
        string nh4Text = nh4Field.text;
        string no2Text = no2Field.text;
        string no3Text = no3Field.text;

        // 値が空の場合は処理を中断
        if (string.IsNullOrEmpty(seriesText) || string.IsNullOrEmpty(nh4Text) || string.IsNullOrEmpty(no2Text) || string.IsNullOrEmpty(no3Text))
        {
            Debug.Log("empty!");
            yield break;
        }

        var wq = new WaterQualityOthers()
        {
            SERIES = seriesText,
            NH4 = nh4Text,
            NO2 = no2Text,
            NO3 = no3Text,
        };

        // クラスをJSON形式に変換
        string jsonData = JsonUtility.ToJson(wq);
        Debug.Log(jsonData);

        // UnityWebRequestを使ってGoogle Apps ScriptにJSONデータをPOST
        UnityWebRequest req = new UnityWebRequest(gasUrl, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
        req.uploadHandler = new UploadHandlerRaw(bodyRaw);
        req.downloadHandler = new DownloadHandlerBuffer();
        req.SetRequestHeader("Content-Type", "application/json");

        // 情報を送信
        yield return req.SendWebRequest();

        // リクエストが成功したかどうかの判定
        if (IsWebRequestSuccessful(req))
        {
            Debug.Log("success");
        }
        else
        {
            Debug.Log("error");
        }

        // 入力フィールドのリセット
        ResetInputFields();

        // スプレッドシートから情報を再取得
        StartCoroutine(GetData_wq_everyday());
    }

    // 入力フィールドを空にする関数
    void ResetInputFields()
    {
        seriesField.text = "";
        nh4Field.text = "";
        no2Field.text = "";
        no3Field.text = "";
    }
}