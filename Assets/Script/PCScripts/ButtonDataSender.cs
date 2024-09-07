using UnityEngine;
using TMPro; // TextMeshProUGUI を使うための名前空間
using UnityEngine.UI;

public class ButtonDataSender : MonoBehaviour
{
    void Start()
    {
        // このスクリプトがアタッチされているボタンを取得
        Button button = GetComponent<Button>();

        // ボタンのクリックイベントにメソッドを登録
        button.onClick.AddListener(OnButtonClick);
    }

    void OnButtonClick()
    {
        // このスクリプトがアタッチされているボタンのTextMeshProUGUIコンポーネントを取得
        TextMeshProUGUI buttonText = GetComponentInChildren<TextMeshProUGUI>();

        // TextMeshProUGUIコンポーネントが存在するか確認
        if (buttonText != null)
        {
            string subButtonText = buttonText.text;
            if (subButtonText == "ph等")
            {
                subButtonText = "phEtc";
            }
            else if (subButtonText == "NH4等")
            {
                buttonText.text = "NH4Etc";
            }
            else if (subButtonText == "成長")
            {
                subButtonText = "growth";
            }
            else if (subButtonText == "コメント")
            {
                subButtonText = "comments";
            }
            else if (subButtonText == "OK")
            {
                subButtonText = "phEtc";
            }
            else if (subButtonText == "戻る")
            {
                subButtonText = "phEtc";
            }
            Debug.Log(subButtonText);
        }
        else
        {
            Debug.LogError("TextMeshProUGUIコンポーネントが見つかりませんでした。ボタンにTextMeshProUGUIコンポーネントが正しくアタッチされているか確認してください。");
        }
    }
}
