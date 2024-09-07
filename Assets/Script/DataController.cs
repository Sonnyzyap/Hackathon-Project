// 参考サイト：https://tanisugames.com/unity-google-spreadsheet-integration/

using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;
using JetBrains.Annotations;
using System;


// 水槽の水質データを記録
public class WaterQualityEveryday
{
    public string type; // Jsonの種類
    public string date; // 日付
    public string TANK; // 水槽
    public string PH, DO, TEMP, SAL, COMMENT; // 毎日測定するデータ
}

public class DataController : MonoBehaviour
{
    [SerializeField] TMP_Text tankText;
    [SerializeField] Button dataButton;
    [SerializeField] TMP_InputField phField, doField, tempField, salField, commentField;
    public GameObject successTextPrefab;
    public Transform successParent;

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
            type = "type1",
            date = DateTime.Now.ToString("yyyy-MM-dd"),
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
        //tankField.text = "";
        phField.text = "";
        doField.text = "";
        tempField.text = "";
        salField.text = "";
        commentField.text = "";
    }
}