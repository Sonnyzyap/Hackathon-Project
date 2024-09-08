using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections.Generic;
using System.Collections;

public class CommentController : MonoBehaviour
{

    public Canvas canvas;  // CanvasはInspectorで設定
    private string url = "https://docs.google.com/spreadsheets/d/e/2PACX-1vQ3hwpVfYo2aEJqYwtrEWdx62-9EOfD9WN2cetG8vYq7AyHcgW1Y91ldORvxBZ7KehbwLbEkou4zlju/pub?gid=0&single=true&output=csv";  // CSVファイルのURL

    public float cellWidth = 200f;  // セルの幅（Inspectorから設定可能）
    public float cellHeight = 50f;  // セルの高さ（Inspectorから設定可能）
    public float horizontalSpacing = 10f;  // 横のセル間隔（Inspectorから設定可能）
    public float verticalSpacing = 20f;  // 縦のセル間隔（Inspectorから設定可能）
    public float rightCellWidth = 1000f;  // 右端のセルの幅（5倍に拡大）

    public float startX = 0f;  // セルのX座標（Inspectorから設定可能）
    public float startY = 0f;  // セルのY座標（Inspectorから設定可能）

    void Start()
    {
        StartCoroutine(DownloadCSV(url));
    }

    IEnumerator DownloadCSV(string url)
    {
        using (UnityWebRequest www = UnityWebRequest.Get(url))
        {
            www.redirectLimit = 10;
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                string csvData = www.downloadHandler.text;
                Debug.Log("CSV Data: " + csvData); // Log the data to verify
                if (csvData.StartsWith("<!DOCTYPE html>")) // Check for HTML content
                {
                    Debug.LogError("Received HTML instead of CSV. Please check the URL.");
                }
                else
                {
                    List<List<string>> data = ParseCSV(csvData);
                    CreateUI(data);
                }
            }
            else
            {
                Debug.LogError("Failed to download CSV: " + www.error);
            }
        }
    }

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

    void CreateUI(List<List<string>> data)
    {
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

                // 一番右のセルのテキストの配置を設定
                if (col == data[row].Count - 1)
                {
                    text.alignment = TextAnchor.MiddleLeft;  // テキストを左揃え
                }
                else
                {
                    text.alignment = TextAnchor.MiddleCenter;  // 中央揃え（デフォルト）
                }
                text.color = Color.black;

                // RectTransformを設定してサイズと位置を調整
                RectTransform bgRect = background.GetComponent<RectTransform>();
                RectTransform textRect = textObject.GetComponent<RectTransform>();

                float xPos = startX + col * (cellWidth + horizontalSpacing);  // 横のセルの幅と間隔の計算
                float yPos = startY - row * (cellHeight + verticalSpacing);  // 縦のセルの高さと間隔の計算

                // 一番右のセルの横幅を変更
                if (col == data[row].Count - 1)
                {
                    bgRect.sizeDelta = new Vector2(rightCellWidth, cellHeight);
                    textRect.sizeDelta = new Vector2(rightCellWidth - 10, cellHeight - 10);  // テキストの余白を設定
                }
                else
                {
                    bgRect.sizeDelta = new Vector2(cellWidth, cellHeight);
                    textRect.sizeDelta = new Vector2(cellWidth - 10, cellHeight - 10);  // テキストの余白を設定
                }

                bgRect.anchoredPosition = new Vector2(xPos, yPos);
                textRect.anchoredPosition = Vector2.zero;
            }
        }
    }
}
