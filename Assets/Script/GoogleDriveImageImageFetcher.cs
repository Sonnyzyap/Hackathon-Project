using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;  // Import Newtonsoft.Json namespace
using System.Collections;
using UnityEngine.UI;
using Unity.VisualScripting;
//public class imageURLobj
//{
//    public string url1PH, url2PH, url31PH, url32PH, url1DO, url2DO, url31DO, url32DO, url1TEMP, url2TEMP, url31TEMP, url32TEMP, url1SAL, url2SAL, url31SAL, url32SAL;  
//}
public class GoogleDriveImageFetcher : MonoBehaviour 

{
    // GAS��URL�̖����Ɂu?format=typeB�v��t���Ă�������
    private string googleAppScriptUrl = "https://script.google.com/macros/s/AKfycbxnvPbVtKAkQL3u1xVZYmbmvAZpktXY83npDNFKhluc7tJ9w-Kd_1EA3Qk0_QdcFaxF/exec?format=typeB";  // Replace with your deployed Google Apps Script URL
    public RawImage rawImage;

    void Start()
    {
        StartCoroutine(GetImageUrlAndDownload());
    }

    IEnumerator GetImageUrlAndDownload()
    {
        UnityWebRequest request = UnityWebRequest.Get(googleAppScriptUrl);
        yield return request.SendWebRequest();
        Debug.Log(request.result);
        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Error fetching image URL: " + request.error);
        }
        else
        {
            // Log the raw JSON response
            string jsonResponse = request.downloadHandler.text;
            Debug.Log("Raw JSON Response: " + jsonResponse);

            try
            {
                // Parse the JSON response using Newtonsoft.Json
                ////var response = JsonConvert.DeserializeObject<Response>(jsonResponse);
                ////imageURLobj image = JsonUtility.FromJson<imageURLobj>(jsonResponse);
                //string testimageUrl = image.url1PH;
                //Debug.Log("image url: " + testimageUrl);
                //// Debug log the deserialized response to verify the imageUrl
                //if (image != null && !string.IsNullOrEmpty(image.url1PH))
                //{
                //    Debug.Log("Deserialized Image URL: " + image.url1PH);
                //}
                //else
                //{
                //    Debug.LogWarning("Image URL is null or empty in the response.");
                //}

                ////string imageUrl = response.fileUrl;

                ////// Now use the imageUrl to download and display the image
                //StartCoroutine(DownloadImage(image.url1PH));
            }
            catch (System.Exception ex)
            {
                Debug.LogError("Error parsing JSON with Newtonsoft.Json: " + ex.Message);
            }
        }
    }

    IEnumerator DownloadImage(string imageUrl)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(imageUrl);
        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Error downloading image: " + request.error);
        }
        else
        {
            Texture2D texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
            rawImage.texture = texture;  // Set the downloaded texture to the RawImage UI
        }
    }
}
