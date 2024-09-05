using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;   //DateTime

using n_Calendar;

public class SampleCalendar : MonoBehaviour
{
    [SerializeField]
    private Text pv_TextObj;

    private const string pv_FORMAT = "yyyy年MM月dd日";

    //クリック
    public void Click()
    {
        DateTime t_Date = new DateTime();
        try
        {
            t_Date = DateTime.Parse(pv_TextObj.text);
        }
        catch
        {
            t_Date = DateTime.Now;
        }

        makeObjCalendar(pv_TextObj, t_Date, pv_FORMAT);
    }

    //カレンダー表示
    private void makeObjCalendar(Text a_Text, DateTime a_Date, string a_FormatDate)
    {
        //プレハブ読み込み
        GameObject t_Prefab = (GameObject)Resources.Load("Prefabs/Canvas_Calendar");

        //プレハブからインスタンスを生成
        GameObject t_InputObj = Instantiate(t_Prefab, this.transform.position, Quaternion.identity, this.transform);
        t_InputObj.transform.localScale = new Vector3(1f, 1f, 1f);

        CalendarScript t_Calendar = t_InputObj.GetComponent<CalendarScript>();
        t_Calendar.SetInit(a_Text, a_Date, a_FormatDate);

    }

}
