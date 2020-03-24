using System;
using System.Text;
using UnityEngine;

public class ModifyADay : MonoBehaviour
{
    public MainWindow m_MainWindow;
    public StarUsage m_StarUsage;

    private StringBuilder m_StringBuilder = new StringBuilder();

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void OnConfirmButtonClick()
    {
        int iMax = m_StarUsage.starRowCount - 1;
        for (int i = 0; i < iMax; i++)
        {
            m_StringBuilder.Append(m_StarUsage.GetStarRow(i).selectedStars);
            m_StringBuilder.Append(".");
        }
        m_StringBuilder.Append(m_StarUsage.GetStarRow(iMax).selectedStars);
        string stars = m_StringBuilder.ToString();
        DateTime today = DateTime.Now;
        DayInfo day = new DayInfo(today.Year, today.Month, today.Day, stars);
        Days.m_Instance.ModifyADay(day);

        m_MainWindow.CloseModifyADay(true);
    }

    public void OnCancelButtonClick()
    {
        m_MainWindow.CloseModifyADay(false);
    }
}
