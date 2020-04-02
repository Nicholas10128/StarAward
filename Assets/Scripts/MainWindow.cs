using System;
using System.Text;
using UnityEngine;
using GCT.UI;

public class MainWindow : MonoBehaviour
{
    public Transform m_ContentNode;

    public Canvas m_MainWindow;
    public ModifyADay m_ModifyADay;
    public CustomStarUsageUI m_CustomStarUsageUI;
    public Canvas m_CustomStarUsage;

    public DaysSheet m_DaysSheet;

    private StringBuilder m_StringBuilder = new StringBuilder();

    void Start()
    {
        CustomStarUsage.Reload();
        Days.m_Instance.Reload();
        MessageBox.Initialize();
    }

    void Update()
    {
        MessageBox.Tick();
    }

    void RefreshUI()
    {
        m_DaysSheet.RefreshUI();
    }

    public void OnCheckInTodayButtonClick()
    {
        DateTime today = DateTime.Now;
        int dayCount = Days.m_Instance.Count;
        for (int i = 0; i < dayCount; i++)
        {
            DayInfo day = Days.m_Instance.Get(i);
            if (day.m_Year == today.Year && day.m_Month == today.Month && day.m_Day == today.Day)
            {
                m_ModifyADay.OnOpen(day);
                return;
            }
        }
        int starUsageCount = CustomStarUsage.m_Instance.m_StarUsageCount - 1;
        for (int i = 0; i < starUsageCount; i++)
        {
            m_StringBuilder.Append("0");
            m_StringBuilder.Append(".");
        }
        m_StringBuilder.Append("0");
        string stars = m_StringBuilder.ToString();
        m_StringBuilder.Clear();
        DayInfo newDay = new DayInfo(today.Year, today.Month, today.Day, stars);
        m_ModifyADay.OnOpen(newDay);
    }

    public void OnModifyADayButtonClick(int index)
    {
        DayInfo di = Days.m_Instance.Get(index);
        m_ModifyADay.OnOpen(di);
    }

    public void CloseModifyADay(bool modified)
    {
        m_ModifyADay.OnClose();
        if (modified)
        {
            RefreshUI();
        }
    }

    public void OnCustomStarUsageButtonClick()
    {
        m_CustomStarUsageUI.OnOpen();
    }

    public void CloseCustomStarUsageUI(bool modified)
    {
        m_CustomStarUsageUI.OnClose();
        if (modified)
        {
            RefreshUI();
        }
    }
}
