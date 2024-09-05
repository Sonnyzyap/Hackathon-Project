using UnityEngine;
using UnityEngine.UI;

public class ToggleCanvasRenderer : MonoBehaviour
{
    public Button toggleButton;     // 表示/非表示を切り替えるボタン
    public GameObject targetUIElement; // 表示/非表示を切り替えたいUI要素

    void Start()
    {
        // ボタンが設定されているか確認し、設定されていればクリックイベントを追加
        if (toggleButton != null)
        {
            toggleButton.onClick.AddListener(ToggleUIElement);
        }

        // UI要素が設定されていれば初期状態を非表示にする
        if (targetUIElement != null)
        {
            targetUIElement.SetActive(false);
        }
    }

    void ToggleUIElement()
    {
        // UI要素の表示/非表示を切り替える
        if (targetUIElement != null)
        {
            targetUIElement.SetActive(!targetUIElement.activeSelf);
        }
    }
}
