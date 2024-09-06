// 参考サイト：https://tanisugames.com/unity-google-spreadsheet-integration/

using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;
using JetBrains.Annotations;


public class graphType
{
    public string type, yearClass, monthClass, dayClass, timeLineClass, gTypeClass; // 毎日測定するデータ
}

public class SendGraphData : MonoBehaviour
{
    [SerializeField] TMPro.TMP_Text year, month, day, timeLine, graphType;
    [SerializeField] Button rightButton;

    // スプレッドシートの読み取りURL

    // Google Apps ScriptのWebアプリURL
    string gasUrl = "https://script.google.com/macros/s/AKfycbwZVZ0YuNiKnOCG_Yy_FSHz_e9FCKu9lxRXxgRYMejq4Mf4rRUgkQ0i3Rr4A3GOFAZD0Q/exec";

    List<string> datas = new List<string>();

    void Start()
    {
        StartCoroutine(PostGraphData());
        year.text = System.DateTime.Now.Year.ToString();
        month.text = System.DateTime.Now.Month.ToString();
        day.text = System.DateTime.Now.Day.ToString();

        // ボタンクリック時の挙動
        rightButton.onClick.AddListener(() => StartCoroutine(PostGraphData()));
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

    //IEnumerator GetData_reference_values()
    //{
    //    using (UnityWebRequest req = UnityWebRequest.Get(url_reference_values))
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

    // データを整形する関数
    //void ParseData(string csvData)
    //{
    //    string[] rows = csvData.Split(new[] { "\n" }, System.StringSplitOptions.RemoveEmptyEntries);
    //    foreach (string row in rows)
    //    {
    //        string[] cells = row.Split(new[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries);
    //        foreach (string cell in cells)
    //        {
    //            string trimCell = cell.Trim('"');
    //            if (!string.IsNullOrEmpty(trimCell))
    //            {
    //                datas.Add(trimCell);
    //            }
    //        }
    //    }
    //}

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
    IEnumerator PostGraphData()
    {
        // 入力フィールドから情報を取得
        //string tankText = tankField.text;
        string yText = year.text;
        string mText = month.text;
        string dText = day.text;
        string tlText = timeLine.text;
        string gtText = graphType.text;

        // 値が空の場合は処理を中断
        if (string.IsNullOrEmpty(dText) || string.IsNullOrEmpty(tlText) || string.IsNullOrEmpty(gtText))
        {
            Debug.Log("empty!");
            yield break;
        }

        var wq = new graphType()
        {
            type = "type3",
            yearClass = yText,
            monthClass = mText,
            dayClass = dText,
            timeLineClass = tlText,
            gTypeClass = gtText,
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
    }

    // 入力フィールドを空にする関数
    void ResetInputFields()
    {
        //tankField.text = "";
        year.text = "";
        month.text = "";
        day.text = "";
        timeLine.text = "";
        graphType.text = "";
    }
}