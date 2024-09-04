using UnityEngine;
using UnityEngine.UI;
using System;

public class CalendarController : MonoBehaviour
{
    public GameObject monthButtonPrefab; // 月ボタンのプレハブ
    public GameObject dayButtonPrefab;   // 日付ボタンのプレハブ
    public RectTransform calendarPanel;  // カレンダーのPanel

    private GridLayoutGroup gridLayoutGroup;

    void Start()
    {
        gridLayoutGroup = calendarPanel.GetComponent<GridLayoutGroup>();
        CreateMonthButtons();
    }

    void CreateMonthButtons()
    {
        // 月ボタンを作成
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
        // 現在のカレンダーの内容をクリア
        foreach (Transform child in calendarPanel)
        {
            Destroy(child.gameObject);
        }

        // 選択された月の日付ボタンを作成
        int daysInMonth = DateTime.DaysInMonth(DateTime.Now.Year, month);

        for (int day = 1; day <= daysInMonth; day++)
        {
            GameObject dayButton = Instantiate(dayButtonPrefab, calendarPanel);
            dayButton.GetComponentInChildren<Text>().text = day.ToString();
        }
    }
}
