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
    public float updateInterval = 10000.0f; // �f�[�^�X�V�̊Ԋu�i�b�j

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
        string url = "https://script.google.com/macros/s/AKfycbwZVZ0YuNiKnOCG_Yy_FSHz_e9FCKu9lxRXxgRYMejq4Mf4rRUgkQ0i3Rr4A3GOFAZD0Q/exec"; // ������GAS��URL��\��t��
        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string jsonResult = request.downloadHandler.text;
            Debug.Log("JSON Data: " + jsonResult);

            // JSON�f�[�^��WaterQualityData�I�u�W�F�N�g�ɕϊ�
            WaterQualityData waterData = JsonUtility.FromJson<WaterQualityData>(jsonResult);

            // �I�u�W�F�N�g�̃f�[�^���m�F
            Debug.Log("pH min: " + waterData.pH.min + ", pH max: " + waterData.pH.max);
            // �K�v�ɉ����đ��̃f�[�^���\��
        }
        else
        {
            Debug.LogError(request.error);
        }
    }
}