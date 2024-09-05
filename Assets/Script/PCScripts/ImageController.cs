using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections;

public class ImageDownloader : MonoBehaviour
{
    public string imageUrl = "https://docs.google.com/document/d/1U8C6cm-nSAxLSRyO2Y17r7HYU5_ptAtu/edit";  // Googleドライブの画像URL
    public RawImage rawImage;  // ダウンロードした画像を表示するUI

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
            rawImage.texture = texture;  // 画像をUIに表示
        }
    }
}
