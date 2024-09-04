// 参考サイト：https://tanisugames.com/unity-google-spreadsheet-integration/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI; //追加
using TMPro;

public class DataController : MonoBehaviour
{
    [SerializeField] TMPro.TMP_Text viewText;
    [SerializeField] Button dataButton;　//追加
    [SerializeField] TMP_InputField phField, doField, tempField, salField; //追加
    string url_wq_everyday = "https://docs.google.com/spreadsheets/d/10nx4vfJyCYENE46bAYJQybzY0Ogyjvmi_buAM4pbHhU/gviz/tq?tqx=out:csv&sheet=water_quality_everyday";
    string url_wq_others = "https://docs.google.com/spreadsheets/d/10nx4vfJyCYENE46bAYJQybzY0Ogyjvmi_buAM4pbHhU/gviz/tq?tqx=out:csv&sheet=water_quality_others";

    //追加　後ほど使う書き込み用URLの変数
    string gasUrl = "https://script.google.com/macros/s/AKfycbzLk3AYzc4vWe6w-nsEgkCLmdmUHSm0TzhPKlNhQ-jgbjrDqMMje58Fto0hWaePL0pyBw/exec";

    List<string> datas = new List<string>();
    void Start()
    {
        StartCoroutine(GetData_wq_everyday());

        //追加　ボタンクリック時の挙動
        dataButton.onClick.AddListener(() => StartCoroutine(PostData()));
    }


    //Spread Sheet to App (毎日測定しなければいけない水質データ, ph, do, temp, salinity)
    IEnumerator GetData_wq_everyday()
    {
        using (UnityWebRequest req = UnityWebRequest.Get(url_wq_everyday)) //UnityWebRequest型オブジェクト
        {
            yield return req.SendWebRequest(); //URLにリクエストを送る

            if (IsWebRequestSuccessful(req)) //成功した場合
            {
                ParseData(req.downloadHandler.text); //受け取ったデータを整形する関数に情報を渡す
                DisplayText(); //データを表示する
            }
            else                            //失敗した場合
            {
                Debug.Log("error");
            }
        }
    }

    //Spread Sheet to App (1週間に1回程測定している水質データ，NH4, NO2, NO3)
    IEnumerator GetData_wq_others()
    {
        using (UnityWebRequest req = UnityWebRequest.Get(url_wq_others)) //UnityWebRequest型オブジェクト
        {
            yield return req.SendWebRequest(); //URLにリクエストを送る

            if (IsWebRequestSuccessful(req)) //成功した場合
            {
                ParseData(req.downloadHandler.text); //受け取ったデータを整形する関数に情報を渡す
                DisplayText(); //データを表示する
            }
            else                            //失敗した場合
            {
                Debug.Log("error");
            }
        }
    }


    //データを整形する関数
    void ParseData(string csvData)
    {
        string[] rows = csvData.Split(new[] { "\n" }, System.StringSplitOptions.RemoveEmptyEntries); //スプレッドシートを1行ずつ配列に格納
        foreach (string row in rows)
        {
            string[] cells = row.Split(new[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries);//一行ずつの情報を1セルずつ配列に格納
            foreach (string cell in cells)
            {
                string trimCell = cell.Trim('"'); //セルの文字列からダブルクォーテーションを除去
                if (!string.IsNullOrEmpty(trimCell)) //除去した文字列が空白でなければdatasに追加していく
                {
                    datas.Add(trimCell);
                }
            }
        }
    }


    //文字を表示させる関数
    void DisplayText()
    {
        foreach (string data in datas)
        {
            viewText.text += data + "\n";
        }
    }


    //リクエストが成功したかどうか判定する関数
    bool IsWebRequestSuccessful(UnityWebRequest req)
    {
        /*プロトコルエラーとコネクトエラーではない場合はtrueを返す*/
        return req.result != UnityWebRequest.Result.ProtocolError &&
               req.result != UnityWebRequest.Result.ConnectionError;
    }


    //App to Spred Sheet
    //追加　情報を送信する関数　今はまだ空っぽ
    // 辞書型データを送信する関数
    IEnumerator PostData()
    {
        // 辞書型データの作成
        Dictionary<string, string> dataDict = new Dictionary<string, string>();
        dataDict.Add("ph", phField.text);
        dataDict.Add("do", doField.text);
        dataDict.Add("temp", tempField.text);
        dataDict.Add("salinity", salField.text);

        // 値が空の場合は処理を中断
        if (string.IsNullOrEmpty(dataDict["ph"]) || string.IsNullOrEmpty(dataDict["do"]) || string.IsNullOrEmpty(dataDict["temp"]) || string.IsNullOrEmpty(dataDict["salinity"]))
        {
            Debug.Log("empty!");
            yield break;
        }

        // 辞書型データをJSON形式に変換
        string jsonData = JsonUtility.ToJson(new Wrapper(dataDict));

        // UnityWebRequestを使ってGoogle Apps ScriptにJSONデータをPOST
        using (UnityWebRequest req = new UnityWebRequest(gasUrl, "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
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
        }

        // 入力フィールドのリセット
        ResetInputFields();

        // 追加　スプレッドシートから情報を取得する
        StartCoroutine(GetData_wq_everyday());
    }

    // 辞書型データをJSONに変換するためのラッパークラス
    [System.Serializable]
    private class Wrapper
    {
        public Dictionary<string, string> dict;

        public Wrapper(Dictionary<string, string> dictionary)
        {
            dict = dictionary;
        }
    }

    // InputFieldを空にする関数
    void ResetInputFields()
    {
        phField.text = "";
        doField.text = "";
        tempField.text = "";
        salField.text = "";
    }
}
