using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ToggleButtonColor : MonoBehaviour
{
    public Color normalColor = Color.white;
    public Color pressedColor = Color.gray;
    public Button resetButton;
    //findobjectwithtag<"resetbutton">

    private Button button;
    private bool isPressed = false;
    private string buttonStateKey;
    private static List<string> allButtonKeys = new List<string>();

    void Awake()
    {
        button = GetComponent<Button>();

        if (button == null)
        {
            Debug.LogError("ToggleButtonColorスクリプトはButtonコンポーネントと一緒に使用する必要があります。");
            return;
        }

        buttonStateKey = "ButtonPressedState_" + button.name;

        if (!allButtonKeys.Contains(buttonStateKey))
        {
            allButtonKeys.Add(buttonStateKey);
        }

        LoadButtonState();
        UpdateButtonColor();
        button.onClick.AddListener(ToggleColor);

        resetButton = GameObject.Find("Decision").GetComponent<Button>();
        if (resetButton != null)
        {
            resetButton.onClick.AddListener(ResetAllButtonStates);
        }
        else
        {
            Debug.LogError("リセットボタン 'Decision' が見つかりません。");
        }
    }

    void ToggleColor()
    {
        isPressed = !isPressed;
        UpdateButtonColor();

        PlayerPrefs.SetInt(buttonStateKey, isPressed ? 1 : 0);
        PlayerPrefs.Save();
    }

    void UpdateButtonColor()
    {
        if (isPressed)
        {
            SetButtonColor(pressedColor);
        }
        else
        {
            SetButtonColor(normalColor);
        }
    }

    void SetButtonColor(Color targetColor)
    {
        ColorBlock colorBlock = button.colors;
        colorBlock.normalColor = targetColor;
        colorBlock.highlightedColor = targetColor;
        colorBlock.pressedColor = targetColor * 0.9f;
        colorBlock.selectedColor = targetColor;
        colorBlock.disabledColor = targetColor * 0.5f;
        button.colors = colorBlock;
    }

    void LoadButtonState()
    {
        if (PlayerPrefs.HasKey(buttonStateKey))
        {
            isPressed = PlayerPrefs.GetInt(buttonStateKey) == 1;
            UpdateButtonColor();
        }
    }

    void ResetAllButtonStates()
    {
        foreach (string key in allButtonKeys)
        {
            if (PlayerPrefs.GetInt(key) == 1)
            {
                Debug.Log("Button pressed: " + key.Replace("ButtonPressedState_", ""));
            }
        }

        foreach (string key in allButtonKeys)
        {
            PlayerPrefs.DeleteKey(key);
        }
        PlayerPrefs.Save();

        isPressed = false;
        UpdateButtonColor();
    }
}
