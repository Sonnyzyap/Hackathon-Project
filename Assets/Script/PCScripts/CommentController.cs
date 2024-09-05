using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections.Generic;
using System.Collections;

public class CSVDataDisplay : MonoBehaviour
{
    public Canvas canvas;  // Canvas��Inspector�Őݒ�
    public string url = "https://docs.google.com/spreadsheets/d/1xi7ljCNvzy9lssvxFl43BmZy3IQ42Vd0UMBS1UkNECs/edit?usp=sharing";  // CSV�t�@�C����URL

    void Start()
    {
        StartCoroutine(DownloadCSV(url));
    }

    // CSV�t�@�C����URL����_�E�����[�h����R���[�`��
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

    // CSV�f�[�^�𕶎��񂩂�p�[�X���郁�\�b�h
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

    // UI���쐬���郁�\�b�h
    void CreateUI(List<List<string>> data)
    {
        float cellWidth = 200f;  // �Z���̕�
        float cellHeight = 50f;  // �Z���̍���

        for (int row = 0; row < data.Count; row++)
        {
            for (int col = 0; col < data[row].Count; col++)
            {
                // �w�i�p��Image�I�u�W�F�N�g���쐬
                GameObject background = new GameObject("CellBackground");
                background.transform.SetParent(canvas.transform);
                Image bgImage = background.AddComponent<Image>();
                bgImage.color = Color.white;  // �w�i�𔒂ɐݒ�

                // �e�L�X�g�p��Text�I�u�W�F�N�g���쐬
                GameObject textObject = new GameObject("CellText");
                textObject.transform.SetParent(background.transform);
                Text text = textObject.AddComponent<Text>();
                text.text = data[row][col];
                text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
                text.fontSize = 24;
                text.alignment = TextAnchor.MiddleCenter;
                text.color = Color.black;

                // RectTransform��ݒ肵�ăT�C�Y�ƈʒu�𒲐�
                RectTransform bgRect = background.GetComponent<RectTransform>();
                RectTransform textRect = textObject.GetComponent<RectTransform>();

                float xPos = col * (cellWidth + 10);  // �Z���̕��ƊԊu���l��
                float yPos = -row * (cellHeight + 10);  // �Z���̍����ƊԊu���l��

                bgRect.sizeDelta = new Vector2(cellWidth, cellHeight);
                bgRect.anchoredPosition = new Vector2(xPos, yPos);

                textRect.sizeDelta = new Vector2(cellWidth - 10, cellHeight - 10);  // �]�����l��
                textRect.anchoredPosition = Vector2.zero;
            }
        }
    }
}
