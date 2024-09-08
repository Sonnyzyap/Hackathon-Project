using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using Newtonsoft.Json;
public class GoogleDriveImageFetcher2 : MonoBehaviour
{
    [System.Serializable]
    public class ImageUrlsResponse
    {
        public List<string> fileUrls; // To hold the array of URLs
    }
    private string googleScriptUrl = "https://script.google.com/macros/s/AKfycbwTv_tjVnXZBKUiy7F-wqKvmvwSEiDiX_u9AjxaLp7KqyKQR8kqGxPJ2Ul4FaLw2vM/exec";
    public GameObject graphImagePrefab;
    public Transform gridParent;

    // Start fetching the images
    IEnumerator Start()
    {
        yield return GetImageUrlsAndDownload();
    }
    // Function to fetch URLs and download the images
    private IEnumerator GetImageUrlsAndDownload()
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(googleScriptUrl))
        {
            // Send request and wait for the response
            yield return webRequest.SendWebRequest();
            if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error: " + webRequest.error);
            }
            else
            {
                // Parse the JSON response
                string jsonResponse = webRequest.downloadHandler.text;
                Debug.Log("Raw JSON Response: " + jsonResponse);
                // Deserialize the JSON response to get the list of URLs
                ImageUrlsResponse response = JsonConvert.DeserializeObject<ImageUrlsResponse>(jsonResponse);
                // Iterate over each URL and download the image
                foreach (string url in response.fileUrls)
                {
                    Debug.Log("Downloading image from: " + url);
                    yield return DownloadImage(url);
                }
            }
        }
    }
    // Function to download an image from the provided URL
    private IEnumerator DownloadImage(string url)
    {
        using (UnityWebRequest request = UnityWebRequestTexture.GetTexture(url))
        {
            yield return request.SendWebRequest();
            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Image download failed: " + request.error);
            }
            else
            {
                // Success: Apply the downloaded image texture to a material or object
                Texture2D texture = DownloadHandlerTexture.GetContent(request);
                // Instantiate the prefab
                GameObject graphImageInstance = Instantiate(graphImagePrefab, gridParent);
                RawImage rawImage = graphImageInstance.GetComponentInChildren<RawImage>();
                if (rawImage != null)
                {
                    // Set the texture to the RawImage component
                    rawImage.texture = texture;
                    //rawImage.SetNativeSize(); // Optional: Adjust size to fit the image
                    Debug.Log("Image downloaded and applied successfully.");
                }
                else
                {
                    Debug.LogError("No RawImage component found in the prefab.");
                }
                Debug.Log("Image downloaded successfully.");
                // Example: Apply texture to a GameObject (e.g., a plane or cube)
                // GetComponent<Renderer>().material.mainTexture = texture;
            }
        }
    }
}