using System;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class ModifyADay : MonoBehaviour
{
    public MainWindow m_MainWindow;
    public StarUsage m_StarUsage;
    public Text m_DateText;

    private Canvas m_Canvas;
    private StringBuilder m_StringBuilder = new StringBuilder();

    private DayInfo m_TheDay;

    void Start()
    {
        m_Canvas = GetComponent<Canvas>();
    }

    void Update()
    {
        
    }

    public void OnOpen(DayInfo di)
    {
        m_TheDay = di;
        DateTime date = new DateTime(di.m_Year, di.m_Month, di.m_Day);
        m_DateText.text = date.ToShortDateString();
        m_StarUsage.RefreshDay(di);
        m_Canvas.enabled = true;
    }

    public void OnClose()
    {
        m_Canvas.enabled = false;
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
        m_StringBuilder.Clear();
        m_TheDay.m_Stars = stars;
        Days.m_Instance.ModifyADay(m_TheDay);

        m_MainWindow.CloseModifyADay(true);
    }

    public void OnCancelButtonClick()
    {
        m_MainWindow.CloseModifyADay(false);
    }
}
