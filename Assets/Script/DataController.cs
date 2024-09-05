// 参考サイト：https://tanisugames.com/unity-google-spreadsheet-integration/

using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;
using JetBrains.Annotations;


public class WaterQualityEveryday
{
    public string TANK; // 水槽
    public string PH, DO, TEMP, SAL, COMMENT; // 毎日測定するデータ
}

public class DataController : MonoBehaviour
{
    [SerializeField] TMPro.TMP_Text tankText;
    [SerializeField] Button dataButton;
    [SerializeField] TMP_InputField phField, doField, tempField, salField, commentField;

    // スプレッドシートの読み取りURL
    //string url_wq_everyday = "https://docs.google.com/spreadsheets/d/10nx4vfJyCYENE46bAYJQybzY0Ogyjvmi_buAM4pbHhU/gviz/tq?tqx=out:csv&sheet=water_quality_everyday";
    string url_reference_values = "https://docs.google.com/spreadsheets/d/10nx4vfJyCYENE46bAYJQybzY0Ogyjvmi_buAM4pbHhU/gviz/tq?tqx=out:csv&sheet=基準値";
    //string url_wq_others = "https://docs.google.com/spreadsheets/d/10nx4vfJyCYENE46bAYJQybzY0Ogyjvmi_buAM4pbHhU/gviz/tq?tqx=out:csv&sheet=water_quality_others";

    // Google Apps ScriptのWebアプリURL
    string gasUrl = "https://script.google.com/macros/s/AKfycbwZVZ0YuNiKnOCG_Yy_FSHz_e9FCKu9lxRXxgRYMejq4Mf4rRUgkQ0i3Rr4A3GOFAZD0Q/exec";

    List<string> datas = new List<string>();

    void Start()
    {
        StartCoroutine(GetData_reference_values());

        // ボタンクリック時の挙動
        dataButton.onClick.AddListener(() => StartCoroutine(PostData()));
    }

    // スプレッドシートからデータを取得する関数
    //IEnumerator GetData_wq_everyday()
    //{
    //    using (UnityWebRequest req = UnityWebRequest.Get(url_wq_everyday))
    //    {
    //        yield return req.SendWebRequest();

    //        if (IsWebRequestSuccessful(req))
    //        {
    //            ParseData(req.downloadHandler.text);
    //            //DisplayText();
    //        }
    //        else
    //        {
    //            Debug.Log("error");
    //        }
    //    }
    //}

    IEnumerator GetData_reference_values()
    {
        using (UnityWebRequest req = UnityWebRequest.Get(url_reference_values))
        {
            yield return req.SendWebRequest();

            if (IsWebRequestSuccessful(req))
            {
                ParseData(req.downloadHandler.text);
                //DisplayText();
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
    //void DisplayText()
    //{
    //    foreach (string data in datas)
    //    {
    //        viewText.text += data + "\n";
    //    }
    //}

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
        //string tankText = tankField.text;
        string tText = tankText.text;
        string phText = phField.text;
        string doText = doField.text;
        string tempText = tempField.text;
        string salText = salField.text;
        string comText = commentField.text;

        // 値が空の場合は処理を中断
        if (string.IsNullOrEmpty(phText) || string.IsNullOrEmpty(doText) || string.IsNullOrEmpty(tempText) || string.IsNullOrEmpty(salText))
        {
            Debug.Log("empty!");
            yield break;
        }

        var wq = new WaterQualityEveryday()
        {
            TANK = tText,
            PH = phText,
            DO = doText,
            TEMP = tempText,
            SAL = salText,
            COMMENT = comText,
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
        StartCoroutine(GetData_reference_values());
    }

    // 入力フィールドを空にする関数
    void ResetInputFields()
    {
        //tankField.text = "";
        phField.text = "";
        doField.text = "";
        tempField.text = "";
        salField.text = "";
        commentField.text = "";
    }
}