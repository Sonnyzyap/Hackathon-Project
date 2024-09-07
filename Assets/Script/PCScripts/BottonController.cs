using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.Networking;

public class ToggleButtonColor : MonoBehaviour
{
    public Color normalColor = Color.white;
    public Color pressedColor = Color.gray;
    public Button resetButton;

    private Button button;
    private bool isPressed = false;
    private string buttonStateKey;
    private static List<string> allButtonKeys = new List<string>();
    private static List<string> registeredButtonKeys = new List<string>();

    private const string gasURL = "https://script.google.com/macros/s/AKfycbxCJBleS5bgYSXOE0k-VpCcosI9GMK-_yWXUT7oY1zvKg4lUtGQCJejwbTN_UWMHDkPvQ/exec"; // Google Apps Script のURLをここに入力


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
                registeredButtonKeys.Add(key.Replace("ButtonPressedState_", ""));
            }
        }

        StartCoroutine(SendKeysToGAS());

        foreach (string key in allButtonKeys)
        {
            PlayerPrefs.DeleteKey(key);
        }
        PlayerPrefs.Save();

        isPressed = false;
        UpdateButtonColor();
    }

    IEnumerator SendKeysToGAS()
    {
        // JSONデータを作成
        KeyData jsonData = new KeyData();
        jsonData.keys = registeredButtonKeys;
        string json = JsonUtility.ToJson(jsonData);

        using (UnityWebRequest request = new UnityWebRequest(gasURL, "POST"))
        {
            Debug.Log("Sending JSON: " + json); // JSON内容を確認するためのデバッグ出力
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.Log("Error sending data: " + request.error);
            }
            else
            {
                Debug.Log("Data sent successfully: " + request.downloadHandler.text);
            }
        }
    }

    [System.Serializable]
    public class KeyData
    {
        public List<string> keys; // リストとして保持するためのフィールド
    }
}
