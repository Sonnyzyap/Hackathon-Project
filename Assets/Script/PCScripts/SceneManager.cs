using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonSceneSwitcher : MonoBehaviour
{
    public string sceneName; // 遷移先のシーン名

    void Start()
    {
        // このスクリプトがアタッチされているボタンを取得
        Button button = GetComponent<Button>();

        //// ボタンのクリックイベントにメソッドを登録
        button.onClick.AddListener(OnButtonClick);
    }

    void OnButtonClick()
    {
        // 指定されたシーンに切り替える
        SceneManager.LoadScene(sceneName);
    }
}
