using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;


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
    public GameObject errorMessagePrefab;
    public Transform errorParent;
    public WaterQualityData waterQualityRanges;
    public float updateInterval = 10.0f; // �f�[�^�X�V�̊Ԋu�i�b�j

    private Dictionary<(ParameterRange, string), List<TMP_InputField>> parameterInputFieldMap;
    public List<TMP_InputField> phInputFields;
    public List<TMP_InputField> DOInputFields;
    public List<TMP_InputField> temperatureInputFields;
    public List<TMP_InputField> salinityInputFields;
    public List<TMP_InputField> nH4InputFields;
    public List<TMP_InputField> NO2InputFields;
    public List<TMP_InputField> NO3InputFields;
    public List<TMP_InputField> CaInputFields;
    public List<TMP_InputField> AlInputFields;
    public List<TMP_InputField> MgInputFields;

    void Start()
    {
        parameterInputFieldMap = new Dictionary<(ParameterRange, string), List<TMP_InputField>>
        {
            { (waterQualityRanges.pH, "pH"), phInputFields },
            { (waterQualityRanges.DO, "DO"), DOInputFields },
            { (waterQualityRanges.temperature, "temperature"), temperatureInputFields },
            { (waterQualityRanges.salinity, "salinity"), salinityInputFields },
            { (waterQualityRanges.NH4, "NH4"), nH4InputFields },
            { (waterQualityRanges.NO2, "NO2"), NO2InputFields },
            { (waterQualityRanges.NO3, "NO3"), NO3InputFields },
            { (waterQualityRanges.Ca, "Ca"), CaInputFields },
            { (waterQualityRanges.Al, "Al"), AlInputFields },
            { (waterQualityRanges.Mg, "Mg"), MgInputFields }
        };

        foreach (var paramPair in parameterInputFieldMap)
        {
            foreach (TMP_InputField inputField in paramPair.Value)
            {
                inputField.onEndEdit.AddListener(delegate { ValidateInput(inputField, paramPair.Key.Item1, paramPair.Key.Item2); });
            }
        }
        StartCoroutine(FetchDataPeriodically());
    }

    void ValidateInput(TMP_InputField inputField, ParameterRange range, string parameterName)
    {
        float value;
        if (float.TryParse(inputField.text, out value))
        {
            if (value < range.min || value > range.max)
            {
                string errorMessage = parameterName + " is out of range!";
                ShowErrorMessage(inputField, errorMessage);
                Debug.LogWarning(parameterName + " out of range: " + value);
            }
            else
            {
                //errorMessage.text = ""; // Clear error message if valid
                Debug.Log(parameterName + " is within range.");
            }
        }
        else
        {
            //errorMessage.text = "Invalid input for " + data.parameterName;
        }
    }

    void ShowErrorMessage(TMP_InputField inputField, string message)
    {
        // Instantiate the error prefab and place it under the errorParent
        GameObject errorInstance = Instantiate(errorMessagePrefab, errorParent);
        TMP_Text errorText = errorInstance.GetComponentInChildren<TMP_Text>();

        // Set the error message text
        errorText.text = message;
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
        string url = "https://script.google.com/macros/s/AKfycbx75yHbTZFnE2CfOOA_TXqd_HYeiwTXu9dekhHoW8LoPUpbRzZM4noZGDxzZCksPzH2uQ/exec"; // ������GAS��URL��\��t��
        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string jsonResult = request.downloadHandler.text;
            Debug.Log("JSON Data: " + jsonResult);

            // JSON�f�[�^��WaterQualityData�I�u�W�F�N�g�ɕϊ�
            WaterQualityData waterData = JsonUtility.FromJson<WaterQualityData>(jsonResult);

            waterQualityRanges.pH.min = waterData.pH.min;
            waterQualityRanges.pH.max = waterData.pH.max;
            waterQualityRanges.DO.min = waterData.DO.min;
            waterQualityRanges.DO.max = waterData.DO.max;
            waterQualityRanges.temperature.min = waterData.temperature.min;
            waterQualityRanges.temperature.max = waterData.temperature.max;
            waterQualityRanges.salinity.min = waterData.salinity.min;
            waterQualityRanges.salinity.max = waterData.salinity.max;
            waterQualityRanges.NH4.min = waterData.NH4.min;
            waterQualityRanges.NH4.max = waterData.NH4.max;
            waterQualityRanges.NO2.min = waterData.NO2.min;
            waterQualityRanges.NO2.max = waterData.NO2.max;
            waterQualityRanges.NO3.min = waterData.NO3.min;
            waterQualityRanges.NO3.max = waterData.NO3.max;
            waterQualityRanges.Ca.min = waterData.Ca.min;
            waterQualityRanges.Ca.max = waterData.Ca.max;
            waterQualityRanges.Al.min = waterData.Al.min;
            waterQualityRanges.Al.max = waterData.Al.max;
            waterQualityRanges.Mg.min = waterData.Mg.min;
            waterQualityRanges.Mg.max = waterData.Mg.max;
        }
        else
        {
            Debug.LogError(request.error);
        }
    }
}