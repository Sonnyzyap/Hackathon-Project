using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;

public class BottonController1 : MonoBehaviour
{
    public Color normalColor = Color.white;
    public Color pressedColor = Color.gray;
    public Button resetButton;
    public string imageUrl;  // �ǉ�: �摜��URL���i�[����ϐ�

    private Button button;
    private bool isPressed = false;
    private string buttonStateKey;
    private static List<string> allButtonKeys = new List<string>();

    void Awake()
    {
        button = GetComponent<Button>();
        imageURLobj url = new imageURLobj();
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

        // �ǉ�: �摜��URL���烍�[�h���ă{�^���ɐݒ�
        if (!string.IsNullOrEmpty(url.url1PH))
        {
            StartCoroutine(LoadImageFromUrl(url.url1PH));
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
                Debug.Log(key.Replace("ButtonPressedState_", ""));
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

    // �ǉ�: URL����摜�����[�h���ă{�^���ɐݒ�
    private IEnumerator LoadImageFromUrl(string url)
    {
        using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(url))
        {
            yield return uwr.SendWebRequest();

            if (uwr.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Failed to load image from URL: " + url);
            }
            else
            {
                Texture2D texture = DownloadHandlerTexture.GetContent(uwr);
                Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                button.image.sprite = sprite;  // �{�^���̔w�i�摜�Ƃ��Đݒ�
            }
        }
    }
}
