using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;  // Import Newtonsoft.Json namespace
using System.Collections;
using UnityEngine.UI;
public class GoogleDriveImageFetcher : MonoBehaviour
{
    private string googleAppScriptUrl = "https://script.google.com/macros/s/AKfycbxqTK0r2481dAvTC5i9QoZzF3smjo5KO9vPamL0RStC4PNU6M_iv5jeEVoViRhZIy0C/exec";  // Replace with your deployed Google Apps Script URL
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
                var response = JsonConvert.DeserializeObject<Response>(jsonResponse);
                string testimageUrl = response.fileUrl;
                Debug.Log("image url: " + testimageUrl);
                // Debug log the deserialized response to verify the imageUrl
                if (response != null && !string.IsNullOrEmpty(response.fileUrl))
                {
                    Debug.Log("Deserialized Image URL: " + response.fileUrl);
                }
                else
                {
                    Debug.LogWarning("Image URL is null or empty in the response.");
                }

                string imageUrl = response.fileUrl;

                // Now use the imageUrl to download and display the image
                StartCoroutine(DownloadImage(imageUrl));
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

    // Class for deserializing the JSON response
    private class Response
    {
        public string fileUrl { get; set; }
    }
}
