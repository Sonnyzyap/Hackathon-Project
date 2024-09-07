// 参考サイト：https://tanisugames.com/unity-google-spreadsheet-integration/

using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;
using JetBrains.Annotations;


// 硝化槽の水質データを記録
public class WaterQualityOthers
{
    public string type; // jsonの種類
    public string date; // 日付
    public string TANK; // 硝化槽
    public string PH, DO, TEMP, SAL, NH4, NO2, NO3, COMMENT; // 1週間に1回ほど測定するデータ
}

public class DataControllerOthers : MonoBehaviour
{
    [SerializeField] TMP_Text tankText;
    [SerializeField] Button dataButton;
    [SerializeField] TMP_InputField phField, doField, tempField, salField, nh4Field, no2Field, no3Field, commentField;
    public GameObject successTextPrefab;
    public Transform successParent;

    // スプレッドシートの読み取りURL

    // Google Apps ScriptのWebアプリURL
    private string gasUrl = "https://script.google.com/macros/s/AKfycbxCJBleS5bgYSXOE0k-VpCcosI9GMK-_yWXUT7oY1zvKg4lUtGQCJejwbTN_UWMHDkPvQ/exec";

    List<string> datas = new List<string>();

    void Start()
    {
        // ボタンクリック時の挙動
        dataButton.onClick.AddListener(() => StartCoroutine(PostData()));
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
        string tText = tankText.text;
        string phText = phField.text;
        string doText = doField.text;
        string tempText = tempField.text;
        string salText = salField.text;
        string nh4Text = nh4Field.text;
        string no2Text = no2Field.text;
        string no3Text = no3Field.text;
        string comText = commentField.text;

        // 値が空の場合は処理を中断
        if (string.IsNullOrEmpty(phText) || string.IsNullOrEmpty(doText) || string.IsNullOrEmpty(tempText) || string.IsNullOrEmpty(salText))
        {
            Debug.Log("empty!");
            yield break;
        }

        var wq = new WaterQualityOthers()
        {
            type = "type2",
            date = "2024-08-10",
            TANK = tText,
            PH = phText,
            DO = doText,
            TEMP = tempText,
            SAL = salText,
            NH4 = nh4Text,
            NO2 = no2Text,
            NO3 = no3Text,
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
            GameObject successInstance = Instantiate(successTextPrefab, successParent);
        }
        else
        {
            Debug.Log("error");
        }

        // 入力フィールドのリセット
        ResetInputFields();
    }

    // 入力フィールドを空にする関数
    void ResetInputFields()
    {
        phField.text = "";
        doField.text = "";
        tempField.text = "";
        salField.text = "";
        nh4Field.text = "";
        no2Field.text = "";
        no3Field.text = "";
        commentField.text = "";
    }
}