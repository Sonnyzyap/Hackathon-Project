using System.Text;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows;
using System.Linq;

public class CalenderButtonController : MonoBehaviour
{
    private Button buttonToday; // 「Button_Today」ボタンを格納する変数

    void Start()
    {
        // このスクリプトがアタッチされているボタンを取得
        Button button = GetComponent<Button>();

        // シーン内から「Button_Today」オブジェクトを取得し、Buttonコンポーネントを取得
        GameObject buttonObject = GameObject.Find("Button_Today");
        if (buttonObject != null)
        {
            buttonToday = buttonObject.GetComponent<Button>();
        }
        else
        {
            Debug.LogError("Button_Today がシーン内に見つかりませんでした。");
        }

        // ボタンのクリックイベントにメソッドを登録
        button.onClick.AddListener(OnButtonClick);
    }

    void OnButtonClick()
    {
        // このスクリプトがアタッチされているボタンのTextコンポーネントを取得
        Text buttonText = GetComponentInChildren<Text>();

        // 「Button_Today」ボタンのTextコンポーネントを取得
        Text anotherButtonText = buttonToday?.GetComponentInChildren<Text>();

        // テキストの内容をつなげてコンソールに出力
        if (buttonText != null && anotherButtonText != null)
        {
            string combinedText = anotherButtonText.text + buttonText.text;
            Debug.Log(TraverseString(combinedText));
        }
        else
        {
            Debug.Log("テキストコンポーネントが見つかりませんでした。");
        }
    }

    static string TraverseString(string input)
    {
        StringBuilder sb = new StringBuilder(input);

        // forループで文字列を走査
        for (int i = 0; i < input.Length; i++)
        {
            char c = input[i];
            if (c == '月')
            {
                sb[i] = '-';  // StringBuilderで文字を置換
            }
            else if (c == '年')
            {
                sb[i] = '-';  // StringBuilderで文字を置換
            }
        }

        return sb.ToString();  // 置換後の文字列を返す
    }

}


