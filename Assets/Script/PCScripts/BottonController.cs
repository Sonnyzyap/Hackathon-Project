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
    //findobjectwithtag<"resetbutton">

    private Button button;
    private bool isPressed = false;
    private string buttonStateKey;
    private static List<string> allButtonKeys = new List<string>();
    private static List<string> registeredButtonKeys = new List<string>();

    const string gasURL = "https://script.google.com/macros/s/AKfycbwargcpSEC1nmWMrHohHbz7xcYKOGKnBag2bLPCiE1wgynPfrJaHHx0uZDuSeoExWQmQA/exec"; // Google Apps Script のURLをここに入力

    private static int pressedButtonCount = 0; // 選択されているボタンの数をカウントする
    private static bool isSending = false; // データ送信中かどうかを示すフラグ

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
        if (!isPressed && pressedButtonCount >= 2)
        {
            Debug.Log("すでに2つのボタンが選択されています。");
            return; // 2個以上は選択できないようにする
        }

        isPressed = !isPressed;

        if (isPressed)
        {
            pressedButtonCount++;
        }
        else
        {
            pressedButtonCount--;
        }

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
            if (isPressed)
            {
                pressedButtonCount++;
            }
            UpdateButtonColor();
        }
    }

    void ResetAllButtonStates()
    {
        if (isSending) return; // すでに送信中の場合は処理を行わない

        registeredButtonKeys.Clear(); // 前回のデータをクリア

        foreach (string key in allButtonKeys)
        {
            if (PlayerPrefs.GetInt(key) == 1)
            {
                Debug.Log("Button pressed: " + key.Replace("ButtonPressedState_", ""));
                registeredButtonKeys.Add(key.Replace("ButtonPressedState_", ""));
            }
        }

        StartCoroutine(SendKeysToGAS());

        // PlayerPrefsからすべてのキーを削除
        foreach (string key in allButtonKeys)
        {
            PlayerPrefs.DeleteKey(key);
        }
        PlayerPrefs.Save();

        // 各ボタンの状態をリセット
        ResetAllButtons();

        pressedButtonCount = 0; // リセット時にカウントをリセット
        Debug.Log(pressedButtonCount);
    }

    void ResetAllButtons()
    {
        // 全てのボタンの状態をリセット
        ToggleButtonColor[] allButtons = FindObjectsOfType<ToggleButtonColor>();
        foreach (ToggleButtonColor buttonScript in allButtons)
        {
            buttonScript.ResetButtonState();
        }
    }

    public void ResetButtonState()
    {
        if (isPressed) // ボタンが押されている場合のみカウントを減らす
        {
            pressedButtonCount--;
        }
        isPressed = false;
        UpdateButtonColor();
    }
    IEnumerator SendKeysToGAS()
    {
        isSending = true; // 送信中フラグをセット

        // JSONデータを作成
        KeyData jsonData = new KeyData();
        jsonData.keys = registeredButtonKeys;
        string json = JsonUtility.ToJson(jsonData);

        Debug.Log("Sending JSON: " + json); // JSON内容を確認するためのデバッグ出力

        using (UnityWebRequest request = new UnityWebRequest(gasURL, "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error sending data: " + request.error);
            }
            else if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Data sent successfully: " + request.downloadHandler.text);
            }
            else
            {
                Debug.LogError("Unknown error occurred. Response Code: " + request.responseCode);
            }
        }

        isSending = false; // 送信完了後にフラグをリセット
    }


    [System.Serializable]
    public class KeyData
    {
        public List<string> keys; // リストとして保持するためのフィールド
    }

    bool IsWebRequestSuccessful(UnityWebRequest req)
    {
        return req.result != UnityWebRequest.Result.ProtocolError &&
               req.result != UnityWebRequest.Result.ConnectionError;
    }
}