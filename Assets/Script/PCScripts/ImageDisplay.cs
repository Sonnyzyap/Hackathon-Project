using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

public class ImageDisplay : MonoBehaviour
{
    public string[] imageUrls;  // �摜��URL���i�[����z��
    public Image imagePrefabInstance; // UI��Image�I�u�W�F�N�g�̃v���n�u
    public Canvas uiCanvas;     // �L�����o�X�I�u�W�F�N�g

    void Start()
    {
        StartCoroutine(LoadAndDisplayImages());
    }

    private IEnumerator LoadAndDisplayImages()
    {
        if (imageUrls.Length == 1)
        {
            // �摜��������̏ꍇ�A�L�����o�X�̒����ɔz�u
            yield return StartCoroutine(LoadImageFromUrl(imageUrls[0], imagePrefabInstance, Vector2.zero));
        }
        else if (imageUrls.Length >= 2)
        {
            // �摜����ȏ�̏ꍇ�A�ŏ��̓�����ɕ��ׂĒ����ɔz�u
            for (int i = 0; i < 2; i++)
            {
                float xOffset = (i == 0) ? -imagePrefabInstance.rectTransform.sizeDelta.x / 2 : imagePrefabInstance.rectTransform.sizeDelta.x / 2;
                yield return StartCoroutine(LoadImageFromUrl(imageUrls[i], imagePrefabInstance, new Vector2(xOffset, 0)));
            }
        }
    }

    private IEnumerator LoadImageFromUrl(string url, Image imagePrefab, Vector2 position)
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

                Image imageInstance = Instantiate(imagePrefab, uiCanvas.transform);
                imageInstance.sprite = sprite;
                imageInstance.rectTransform.anchoredPosition = position;
            }
        }
    }
}
