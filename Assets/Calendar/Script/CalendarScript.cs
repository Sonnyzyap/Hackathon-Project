using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;        //DirectoryInfo
using UnityEngine.UI;
using System;

namespace n_Calendar
{
    public class CalendarScript : MonoBehaviour
    {
        [SerializeField]
        private GameObject pv_DispDayCalendar;

        [SerializeField]
        private Text pv_TextDate;

        //カレンダーで表示する日付
        private DateTime pv_DispDate;

        //カレンダーで表示する最初の日付
        private DateTime pv_DispFirstDay;

        //選択した日付を書き込むテキストオブジェ
        private Text pv_WriteText;

        //日付フォーマット
        private string pv_FormatDate;

        private bool pv_ReqDisp;

        //========================== システム用関数 =============================
        // Use this for initialization
        void Awake()
        {
            pv_DispDate = DateTime.Now;
            pv_ReqDisp = false;
            pv_FormatDate = "";
            creatCalendar();
        }

        // Start is called before the first frame update
        void Start()
        {
            ChangeToday();           //ゲーム開始時、カレンダー内で今日を表示する
        }

        // Update is called once per frame
        void Update()
        {
            if (pv_ReqDisp == true)
            {
                setCalendar();
                pv_ReqDisp = false;
            }
        }

        //========================== イベント関数 =============================
        //日付選択
        private void OnClickButton(string a_Num)
        {
            DateTime t_SelectDate = pv_DispFirstDay.AddDays(int.Parse(a_Num));  //選択した日付
            pv_WriteText.text = t_SelectDate.ToString(pv_FormatDate);

            // コンソールに選択された日付を表示
            Debug.Log("選択した日付: " + t_SelectDate.ToString(pv_FormatDate));

            DestroyImmediate(this.gameObject, true);
        }

        //表示年変更
        public void ChangeYear(int a_Year)
        {
            pv_DispDate = pv_DispDate.AddYears(a_Year);
            pv_ReqDisp = true;
        }

        //表示年変更
        public void ChangeMonth(int a_Month)
        {
            pv_DispDate = pv_DispDate.AddMonths(a_Month);
            pv_ReqDisp = true;
        }

        //今日に戻る
        public void ChangeToday()
        {
            pv_DispDate = DateTime.Now;
            pv_ReqDisp = true;
        }

        //カレンダーを閉じる
        public void ReqClose()
        {
            this.gameObject.SetActive(false);
        }

        //========================== 外部公開関数 =============================
        public void SetInit(Text a_Text, DateTime a_Date, string a_FormatDate)
        {
            //書き込み先登録
            pv_WriteText = a_Text;

            pv_FormatDate = a_FormatDate;

            pv_DispDate = a_Date;
            pv_ReqDisp = true;
        }

        //========================== 内部関数 =============================
        //カレンダー表示用のボタンのみ作成
        private void creatCalendar()
        {
            GameObject t_Prefab = (GameObject)Resources.Load("Prefabs/Button_DayCalendar");

            for (int i = 0; i < 42; i++)
            {
                GameObject t_GameObj = Instantiate(t_Prefab, pv_DispDayCalendar.transform);

                //clone化したオブジェクトに名前をつける
                t_GameObj.name = i.ToString();

                //ボタンクリック時のcallする関数を登録する
                //Button t_Button = t_GameObj.GetComponent<Button>();
                //t_Button.onClick.AddListener(() => OnClickButton(t_GameObj.name));
            }
        }

        //カレンダー設定
        // a_Date:表示する月
        private void setCalendar()
        {
            pv_DispDate = chkDateRange(pv_DispDate);
            DateTime t_DispDate = pv_DispDate;

            pv_TextDate.text = t_DispDate.Year.ToString() + "年" + t_DispDate.Month.ToString() + "月";

            //表示月最初の日付
            DateTime t_FirstDate = new DateTime(t_DispDate.Year, t_DispDate.Month, 1);

            //表示月が何日まであるか
            int t_DaysInThisMonth = DateTime.DaysInMonth(t_DispDate.Year, t_DispDate.Month);

            //表示月最後の日付
            DateTime t_LastDate = new DateTime(t_DispDate.Year, t_DispDate.Month, t_DaysInThisMonth);

            //本日判定用の日付
            DateTime t_Now = DateTime.Now;
            DateTime t_Today = new DateTime(t_Now.Year, t_Now.Month, t_Now.Day);

            //表示月1日の曜日から前月表示日数を求める
            int t_PaddingDays;
            switch (t_FirstDate.DayOfWeek)
            {
                case DayOfWeek.Sunday:
                    t_PaddingDays = 0;
                    break;
                case DayOfWeek.Monday:
                    t_PaddingDays = 1;
                    break;
                case DayOfWeek.Tuesday:
                    t_PaddingDays = 2;
                    break;
                case DayOfWeek.Wednesday:
                    t_PaddingDays = 3;
                    break;
                case DayOfWeek.Thursday:
                    t_PaddingDays = 4;
                    break;
                case DayOfWeek.Friday:
                    t_PaddingDays = 5;
                    break;
                case DayOfWeek.Saturday:
                    t_PaddingDays = 6;
                    break;
                default:
                    t_PaddingDays = 0;
                    break;
            }

            //カレンダー表示最初の日付
            DateTime t_Date = t_FirstDate.AddDays(-t_PaddingDays);
            pv_DispFirstDay = t_Date;

            for (int i = 0; i < 42; i++)
            {
                Color t_LetterColor = Color.black;
                Color t_BackgronndColor = Color.yellow;

                if ((t_Date >= t_FirstDate)
                 && (t_Date <= t_LastDate))
                {
                    //土曜日青・日曜日赤
                    switch (t_Date.DayOfWeek)
                    {
                        case DayOfWeek.Sunday:
                            t_LetterColor = Color.red;
                            break;

                        case DayOfWeek.Saturday:
                            t_LetterColor = Color.blue;
                            break;

                        default:
                            t_LetterColor = Color.black;
                            break;
                    }

                    //当日背景色
                    if (t_Today == t_Date)
                    {
                        t_BackgronndColor = Color.cyan;
                    }
                }
                else
                {
                    t_BackgronndColor = Color.grey;
                }

                Transform DAY = pv_DispDayCalendar.transform.GetChild(i);

                //日にち書き込み
                DAY.GetChild(0).GetComponent<Text>().text = t_Date.Day.ToString();

                //文字色設定
                DAY.GetChild(0).GetComponent<Text>().color = t_LetterColor;

                //背景色設定
                DAY.GetComponent<Image>().color = t_BackgronndColor;

                //翌日
                t_Date = t_Date.AddDays(1);
            }
        }

        private DateTime chkDateRange(DateTime a_Date)
        {
            DateTime t_Ret = new DateTime();
            DateTime t_Start = DateTime.Parse("1950年1月1日");
            DateTime t_End = DateTime.Parse("2200年12月31日");

            if (a_Date < t_Start)
            {
                t_Ret = t_End;
            }
            else if (a_Date > t_End)
            {
                t_Ret = t_Start;
            }
            else
            {
                t_Ret = a_Date;
            }

            return t_Ret;
        }
    }
}
