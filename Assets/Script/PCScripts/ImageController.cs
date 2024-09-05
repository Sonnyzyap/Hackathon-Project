using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections;

public class ImageDownloader : MonoBehaviour
{
    public string imageUrl = "YOUR_IMAGE_URL";  // Google�h���C�u�̉摜URL
    public RawImage rawImage;  // �_�E�����[�h�����摜��\������UI

    void Start()
    {
        StartCoroutine(DownloadImage());
    }

    IEnumerator DownloadImage()
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(imageUrl);
        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError(request.error);
        }
        else
        {
            Texture2D texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
            rawImage.texture = texture;  // �摜��UI�ɕ\��
        }
    }
}
