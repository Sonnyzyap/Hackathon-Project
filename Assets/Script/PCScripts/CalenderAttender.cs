using UnityEngine;
using UnityEngine.UI;
using System;

public class CalendarController : MonoBehaviour
{
    public GameObject monthButtonPrefab; // ���{�^���̃v���n�u
    public GameObject dayButtonPrefab;   // ���t�{�^���̃v���n�u
    public RectTransform calendarPanel;  // �J�����_�[��Panel

    private GridLayoutGroup gridLayoutGroup;

    void Start()
    {
        gridLayoutGroup = calendarPanel.GetComponent<GridLayoutGroup>();
        CreateMonthButtons();
    }

    void CreateMonthButtons()
    {
        // ���{�^�����쐬
        string[] months = { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };

        foreach (var month in months)
        {
            GameObject monthButton = Instantiate(monthButtonPrefab, calendarPanel);
            monthButton.GetComponentInChildren<Text>().text = month;
            int monthIndex = Array.IndexOf(months, month) + 1;
            monthButton.GetComponent<Button>().onClick.AddListener(() => OnMonthButtonClick(monthIndex));
        }
    }

    void OnMonthButtonClick(int month)
    {
        // ���݂̃J�����_�[�̓��e���N���A
        foreach (Transform child in calendarPanel)
        {
            Destroy(child.gameObject);
        }

        // �I�����ꂽ���̓��t�{�^�����쐬
        int daysInMonth = DateTime.DaysInMonth(DateTime.Now.Year, month);

        for (int day = 1; day <= daysInMonth; day++)
        {
            GameObject dayButton = Instantiate(dayButtonPrefab, calendarPanel);
            dayButton.GetComponentInChildren<Text>().text = day.ToString();
        }
    }
}
