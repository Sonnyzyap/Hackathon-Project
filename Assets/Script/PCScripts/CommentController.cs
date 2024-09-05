using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections.Generic;
using System.Collections;

public class CSVDataDisplay : MonoBehaviour
{
    public Canvas canvas;  // CanvasをInspectorで設定
    public string url = "https://docs.google.com/spreadsheets/d/1xi7ljCNvzy9lssvxFl43BmZy3IQ42Vd0UMBS1UkNECs/edit?usp=sharing";  // CSVファイルのURL

    void Start()
    {
        StartCoroutine(DownloadCSV(url));
    }

    // CSVファイルをURLからダウンロードするコルーチン
    IEnumerator DownloadCSV(string url)
    {
        using (UnityWebRequest www = UnityWebRequest.Get(url))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                string csvData = www.downloadHandler.text;
                List<List<string>> data = ParseCSV(csvData);
                CreateUI(data);
            }
            else
            {
                Debug.LogError("Failed to download CSV: " + www.error);
            }
        }
    }

    // CSVデータを文字列からパースするメソッド
    List<List<string>> ParseCSV(string csvData)
    {
        List<List<string>> result = new List<List<string>>();
        string[] lines = csvData.Split('\n');

        foreach (string line in lines)
        {
            if (!string.IsNullOrWhiteSpace(line))
            {
                string[] values = line.Split(',');
                result.Add(new List<string>(values));
            }
        }

        return result;
    }

    // UIを作成するメソッド
    void CreateUI(List<List<string>> data)
    {
        float cellWidth = 200f;  // セルの幅
        float cellHeight = 50f;  // セルの高さ

        for (int row = 0; row < data.Count; row++)
        {
            for (int col = 0; col < data[row].Count; col++)
            {
                // 背景用のImageオブジェクトを作成
                GameObject background = new GameObject("CellBackground");
                background.transform.SetParent(canvas.transform);
                Image bgImage = background.AddComponent<Image>();
                bgImage.color = Color.white;  // 背景を白に設定

                // テキスト用のTextオブジェクトを作成
                GameObject textObject = new GameObject("CellText");
                textObject.transform.SetParent(background.transform);
                Text text = textObject.AddComponent<Text>();
                text.text = data[row][col];
                text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
                text.fontSize = 24;
                text.alignment = TextAnchor.MiddleCenter;
                text.color = Color.black;

                // RectTransformを設定してサイズと位置を調整
                RectTransform bgRect = background.GetComponent<RectTransform>();
                RectTransform textRect = textObject.GetComponent<RectTransform>();

                float xPos = col * (cellWidth + 10);  // セルの幅と間隔を考慮
                float yPos = -row * (cellHeight + 10);  // セルの高さと間隔を考慮

                bgRect.sizeDelta = new Vector2(cellWidth, cellHeight);
                bgRect.anchoredPosition = new Vector2(xPos, yPos);

                textRect.sizeDelta = new Vector2(cellWidth - 10, cellHeight - 10);  // 余白を考慮
                textRect.anchoredPosition = Vector2.zero;
            }
        }
    }
}
