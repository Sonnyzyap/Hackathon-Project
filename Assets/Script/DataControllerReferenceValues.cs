using UnityEngine;
using UnityEngine.Networking;
using System.Collections;


[System.Serializable]
public class ParameterRange
{
    public float min;
    public float max;
}

[System.Serializable]
public class WaterQualityData
{
    public ParameterRange pH;
    public ParameterRange DO;
    public ParameterRange temperature;
    public ParameterRange salinity;
    public ParameterRange NH4;
    public ParameterRange NO2;
    public ParameterRange NO3;
    public ParameterRange Ca;
    public ParameterRange Al;
    public ParameterRange Mg;
}

public class DataControllerReferenceValues : MonoBehaviour
{
    public float updateInterval = 10000.0f; // データ更新の間隔（秒）

    void Start()
    {
        StartCoroutine(FetchDataPeriodically());
    }

    IEnumerator FetchDataPeriodically()
    {
        while (true)
        {
            yield return StartCoroutine(FetchDataFromGoogleSheets());
            yield return new WaitForSeconds(updateInterval);
        }
    }

    IEnumerator FetchDataFromGoogleSheets()
    {
        string url = "https://script.google.com/macros/s/AKfycbwZVZ0YuNiKnOCG_Yy_FSHz_e9FCKu9lxRXxgRYMejq4Mf4rRUgkQ0i3Rr4A3GOFAZD0Q/exec"; // ここにGASのURLを貼り付け
        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string jsonResult = request.downloadHandler.text;
            Debug.Log("JSON Data: " + jsonResult);

            // JSONデータをWaterQualityDataオブジェクトに変換
            WaterQualityData waterData = JsonUtility.FromJson<WaterQualityData>(jsonResult);

            // オブジェクトのデータを確認
            Debug.Log("pH min: " + waterData.pH.min + ", pH max: " + waterData.pH.max);
            // 必要に応じて他のデータも表示
        }
        else
        {
            Debug.LogError(request.error);
        }
    }
}