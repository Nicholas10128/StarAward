using System;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class DaysSheet : MonoBehaviour
{
    public MainWindow m_MainWindow;
    public RecordRow m_CaptionRow;
    public RectTransform m_AdditionalRowRectTr;

    private Transform m_Transform;
    private GameObject m_RecordTemplate;
    private RecordRow[] m_RecordRows = new RecordRow[0];

    private StringBuilder m_StringBuilder = new StringBuilder();
    private bool m_TodayExist = false;

    void Start()
    {
        m_Transform = transform;
        m_RecordTemplate = m_CaptionRow.gameObject;

        RefreshUI();
    }

    public void RefreshUI()
    {
        int starUsageCount = CustomStarUsage.m_Instance.m_StarUsageCount;
        m_CaptionRow.m_DateTime.text = "日期";
        int existStarsCount = m_CaptionRow.m_Stars.Length;
        if (existStarsCount != starUsageCount)
        {
            if (starUsageCount > 0)// At least one star in the caption row as a template.
            {
                AdjustStars(m_CaptionRow, existStarsCount, starUsageCount);
            }
            for (int i = 0; i < starUsageCount; i++)
            {
                Text starText = m_CaptionRow.m_Stars[i];
                m_StringBuilder.Append("Star");
                m_StringBuilder.Append(i);
                starText.name = m_StringBuilder.ToString();
                m_StringBuilder.Clear();
                Transform starTextTr = starText.transform;
                starTextTr.SetParent(m_CaptionRow.transform);
                starTextTr.localScale = Vector3.one;
                starText.text = CustomStarUsage.m_Instance.GetUsage(i);
            }
        }
        int dayCount = Days.m_Instance.Count;
        if (m_RecordRows.Length != dayCount)
        {
            AdjustRecordRows(dayCount);
        }
        if (m_RecordRows.Length > 0)
        {
            existStarsCount = m_RecordRows[0].m_Stars.Length;
        }
        if (existStarsCount != starUsageCount)
        {
            for (int i = 0; i < dayCount; i++)
            {
                AdjustStars(m_RecordRows[i], existStarsCount, starUsageCount);
            }
        }
        for (int i = 0; i < dayCount; i++)
        {
            DayInfo day = Days.m_Instance.Get(i);
            RecordRow recordRow = m_RecordRows[i];
            recordRow.m_DateTime.text = new DateTime(day.m_Year, day.m_Month, day.m_Day).ToShortDateString();
            int starsCount = Mathf.Min(day.starsCount, starUsageCount);
            for (int j = 0; j < starsCount; j++)
            {
                m_StringBuilder.Append(day.GetStarCount(j));
                recordRow.m_Stars[j].text = m_StringBuilder.ToString();
                m_StringBuilder.Clear();
            }
            recordRow.RefreshColumns();
        }
    }

    public void OnModifyButton(GameObject go)
    {
        if (ReferenceEquals(m_CaptionRow.m_ModifyButton, go))
        {
            m_MainWindow.OnCustomStarUsageButtonClick();
            return;
        }
        for (int i = 0, iMax = m_RecordRows.Length; i < iMax; i++)
        {
            if (ReferenceEquals(m_RecordRows[i].m_ModifyButton, go))
            {
                m_MainWindow.OnModifyADayButtonClick(i);
                break;
            }
        }
    }

    void AdjustRecordRows(int dayCount)
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
            Transform recordTr = record.transform;
            recordTr.SetParent(m_Transform);
            recordTr.localScale = Vector3.one;
            m_RecordRows[i] = record.GetComponent<RecordRow>();
            m_RecordRows[i].m_ModifyButtonText.text = "修改";
            if (!m_TodayExist)
            {
                if (m_TodayExist = day.IsToday())
                {
                    m_AdditionalRowRectTr.gameObject.SetActive(false);
                }
            }
        }
        if (!m_TodayExist)
        {
            m_AdditionalRowRectTr.SetAsLastSibling();
        }
    }

    void AdjustStars(RecordRow recordRow, int existStarsCount, int starUsageCount)
    {
        if (existStarsCount > starUsageCount)
        {
            for (int j = starUsageCount, jMax = existStarsCount; j < jMax; j++)
            {
                Destroy(recordRow.m_Stars[j].gameObject);
            }
            Text[] newStars = new Text[starUsageCount];
            for (int j = 0, jMax = starUsageCount; j < jMax; j++)
            {
                newStars[j] = recordRow.m_Stars[j];
            }
            recordRow.m_Stars = newStars;
        }
        else
        {
            Text[] newStars = new Text[starUsageCount];
            for (int j = 0, jMax = existStarsCount; j < jMax; j++)
            {
                newStars[j] = recordRow.m_Stars[j];
            }
            for (int j = existStarsCount, jMax = starUsageCount; j < jMax; j++)
            {
                GameObject newStar = Instantiate(recordRow.m_Stars[0].gameObject);
                Transform newStarTr = newStar.transform;
                newStarTr.SetParent(recordRow.m_Stars[0].transform.parent);
                newStarTr.localScale = Vector3.one;
                newStars[j] = newStar.GetComponent<Text>();
            }
            recordRow.m_Stars = newStars;
        }
    }
}
