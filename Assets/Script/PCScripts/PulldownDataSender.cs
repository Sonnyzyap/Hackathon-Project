using UnityEngine;
using TMPro; // TextMeshProUGUI を使うための名前空間
using UnityEngine.UI;

public class PulldownDataSender : MonoBehaviour
{
    // Dropdownコンポーネントをインスペクタから割り当てる
    public TMP_Dropdown dropdown;

    void Start()
    {
        // Dropdownコンポーネントが設定されているか確認
        if (dropdown != null)
        {
            // Dropdownの選択変更イベントにメソッドを登録
            dropdown.onValueChanged.AddListener(OnDropdownValueChanged);
        }
        else
        {
            Debug.LogError("Dropdown コンポーネントがインスペクタから設定されていません。");
        }
    }

    void OnDropdownValueChanged(int index)
    {
        // 選択された項目のテキストを取得
        if (dropdown != null && dropdown.options.Count > index)
        {
            string selectedText = dropdown.options[index].text;
            if (selectedText == "週")
            {
                selectedText = "week";
            }
            else if (selectedText == "月")
            {
                selectedText = "month";
            }
            else if (selectedText == "年")
            {
                selectedText = "year";
            }
            else if (selectedText == "年ごと")
            {
                selectedText = "byYear";
            }
            else if (selectedText == "1系")
            {
                selectedText = "1series";
            }
            else if (selectedText == "2系")
            {
                selectedText = "2series";
            }
            else if (selectedText == "3系-1")
            {
                selectedText = "3series1";
            }
            else if (selectedText == "3系-2")
            {
                selectedText = "3series2";
            }
            else if (selectedText == "全て")
            {
                selectedText = "allSeries";
            }
            Debug.Log(selectedText);
        }
        else
        {
            Debug.LogError("選択された項目が見つかりませんでした。");
        }
    }
}
