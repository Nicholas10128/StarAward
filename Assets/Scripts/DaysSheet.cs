using System;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class DaysSheet : MonoBehaviour
{
    public RecordRow m_CaptionRow;

    private Transform m_Transform;
    private GameObject m_RecordTemplate;
    private RecordRow[] m_RecordRows = new RecordRow[0];

    private StringBuilder m_StringBuilder = new StringBuilder();

    void Start()
    {
        m_Transform = transform;
        m_RecordTemplate = m_CaptionRow.gameObject;

        RefreshCaption();
        RefreshUI();
    }

    public void RefreshCaption()
    {
        int starUsageCount = CustomStarUsage.m_Instance.m_StarUsageCount;
        m_CaptionRow.m_DateTime.text = "日期";
        Text templateStarText = m_CaptionRow.m_Stars[0];
        m_CaptionRow.m_Stars = new Text[starUsageCount];
        m_CaptionRow.m_Stars[0] = templateStarText;
        for (int i = 1; i < starUsageCount; i++)
        {
            GameObject starText = Instantiate(m_CaptionRow.m_Stars[0].gameObject);
            m_StringBuilder.Append("Star");
            m_StringBuilder.Append(i);
            starText.name = m_StringBuilder.ToString();
            m_StringBuilder.Clear();
            starText.transform.SetParent(m_CaptionRow.transform);
            m_CaptionRow.m_Stars[i] = starText.GetComponent<Text>();
            m_CaptionRow.m_Stars[i].text = CustomStarUsage.m_Instance.GetUsage(i);
        }
        m_CaptionRow.m_Stars[0].text = CustomStarUsage.m_Instance.m_StarUsage1;
    }

    public void RefreshUI()
    {
        int starUsageCount = CustomStarUsage.m_Instance.m_StarUsageCount;
        int dayCount = Days.m_Instance.Count;
        if (m_RecordRows.Length != dayCount)
        {
            RecordRow[] newRecordRows = new RecordRow[dayCount];
            int i = 0;
            for (int iMax = Mathf.Min(m_RecordRows.Length, dayCount); i < iMax; i++)
            {
                newRecordRows[i] = m_RecordRows[i];
            }
            i = m_RecordRows.Length;
            m_RecordRows = newRecordRows;
            for (; i < dayCount; i++)
            {
                DayInfo day = Days.m_Instance.Get(i);
                GameObject record = Instantiate(m_RecordTemplate);
                m_StringBuilder.Append("Record");
                m_StringBuilder.Append(i);
                record.name = m_StringBuilder.ToString();
                m_StringBuilder.Clear();
                record.transform.SetParent(m_Transform);
                m_RecordRows[i] = record.GetComponent<RecordRow>();
            }
        }
        for (int i = 0; i < dayCount; i++)
        {
            DayInfo day = Days.m_Instance.Get(i);
            m_RecordRows[i].m_DateTime.text = new DateTime(day.m_Year, day.m_Month, day.m_Day).ToShortDateString();
            int starsCount = Mathf.Min(day.starsCount, starUsageCount);
            for (int j = 0; j < starsCount; j++)
            {
                m_StringBuilder.Append(day.GetStarCount(j));
                m_RecordRows[i].m_Stars[j].text = m_StringBuilder.ToString();
                m_StringBuilder.Clear();
            }
        }
    }
}
