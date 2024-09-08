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

    const string gasURL = "https://script.google.com/macros/s/AKfycbwargcpSEC1nmWMrHohHbz7xcYKOGKnBag2bLPCiE1wgynPfrJaHHx0uZDuSeoExWQmQA/exec"; // Google Apps Script ��URL�������ɓ���

    private static int pressedButtonCount = 0; // �I������Ă���{�^���̐����J�E���g����
    private static bool isSending = false; // �f�[�^���M�����ǂ����������t���O

    void Awake()
    {
        button = GetComponent<Button>();

        if (button == null)
        {
            Debug.LogError("ToggleButtonColor�X�N���v�g��Button�R���|�[�l���g�ƈꏏ�Ɏg�p����K�v������܂��B");
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
            Debug.LogError("���Z�b�g�{�^�� 'Decision' ��������܂���B");
        }
    }

    void ToggleColor()
    {
        if (!isPressed && pressedButtonCount >= 2)
        {
            Debug.Log("���ł�2�̃{�^�����I������Ă��܂��B");
            return; // 2�ȏ�͑I���ł��Ȃ��悤�ɂ���
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
        if (isSending) return; // ���łɑ��M���̏ꍇ�͏������s��Ȃ�

        registeredButtonKeys.Clear(); // �O��̃f�[�^���N���A

        foreach (string key in allButtonKeys)
        {
            if (PlayerPrefs.GetInt(key) == 1)
            {
                Debug.Log("Button pressed: " + key.Replace("ButtonPressedState_", ""));
                registeredButtonKeys.Add(key.Replace("ButtonPressedState_", ""));
            }
        }

        StartCoroutine(SendKeysToGAS());

        // PlayerPrefs���炷�ׂẴL�[���폜
        foreach (string key in allButtonKeys)
        {
            PlayerPrefs.DeleteKey(key);
        }
        PlayerPrefs.Save();

        // �e�{�^���̏�Ԃ����Z�b�g
        ResetAllButtons();

        pressedButtonCount = 0; // ���Z�b�g���ɃJ�E���g�����Z�b�g
        Debug.Log(pressedButtonCount);
    }

    void ResetAllButtons()
    {
        // �S�Ẵ{�^���̏�Ԃ����Z�b�g
        ToggleButtonColor[] allButtons = FindObjectsOfType<ToggleButtonColor>();
        foreach (ToggleButtonColor buttonScript in allButtons)
        {
            buttonScript.ResetButtonState();
        }
    }

    public void ResetButtonState()
    {
        if (isPressed) // �{�^����������Ă���ꍇ�̂݃J�E���g�����炷
        {
            pressedButtonCount--;
        }
        isPressed = false;
        UpdateButtonColor();
    }
    IEnumerator SendKeysToGAS()
    {
        isSending = true; // ���M���t���O���Z�b�g

        // JSON�f�[�^���쐬
        KeyData jsonData = new KeyData();
        jsonData.keys = registeredButtonKeys;
        string json = JsonUtility.ToJson(jsonData);

        Debug.Log("Sending JSON: " + json); // JSON���e���m�F���邽�߂̃f�o�b�O�o��

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

        isSending = false; // ���M������Ƀt���O�����Z�b�g
    }


    [System.Serializable]
    public class KeyData
    {
        public List<string> keys; // ���X�g�Ƃ��ĕێ����邽�߂̃t�B�[���h
    }

    bool IsWebRequestSuccessful(UnityWebRequest req)
    {
        return req.result != UnityWebRequest.Result.ProtocolError &&
               req.result != UnityWebRequest.Result.ConnectionError;
    }
}