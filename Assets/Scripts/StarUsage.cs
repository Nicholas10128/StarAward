using System;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class StarUsage : MonoBehaviour
{
    public MainWindow m_MainWindow;
    public StarRow m_StarRow;

    public int totalStarCount { get; private set; }

    private Transform m_Transform;
    private GameObject m_StarRowTemplate;

    private StringBuilder m_StringBuilder = new StringBuilder();

    private StarRow[] m_StarRows;

    private DayInfo m_Today;

    void Start()
    {
        m_StarRow.TemplateInit();
        m_Transform = transform;
        m_StarRowTemplate = m_StarRow.gameObject;

        RefreshUI();

        OnOpen();
    }

    public void RefreshUI()
    {
        int starUsageCount = CustomStarUsage.m_Instance.m_StarUsageCount;
        bool isDirty = true;
        if (m_StarRows != null)
        {
            isDirty = starUsageCount != m_StarRows.Length;
        }
        if (!isDirty)
        {
            for (int i = 0; i < starUsageCount; i++)
            {
                isDirty = m_StarRows[i].m_Usage.text != CustomStarUsage.m_Instance.GetUsage(i);
                if (isDirty)
                {
                    break;
                }
                else
                {
                    isDirty = m_StarRows[i].m_Stars.Length != CustomStarUsage.m_Instance.GetStarMaxCount(i);
                    if (isDirty)
                    {
                        break;
                    }
                }
            }
        }
        if (isDirty)
        {
            AdjustStarRows(starUsageCount);
            AdjustStars(starUsageCount);
        }
        for (int i = 0; i < starUsageCount; i++)
        {
            StarRow starRow = m_StarRows[i];
            starRow.m_Usage.text = CustomStarUsage.m_Instance.GetUsage(i);
            starRow.RefreshColumns();
        }
    }

    public void RefreshToday()
    {
        RefreshDay(m_Today);
    }

    public void OnStarChanged()
    {
        totalStarCount = 0;
        byte starCount;
        int iMax = starRowCount - 1;
        for (int i = 0; i < iMax; i++)
        {
            m_StringBuilder.Append(CustomStarUsage.m_Instance.GetUsage(i));
            m_StringBuilder.Append("|");
            starCount = GetStarRow(i).selectedStars;
            m_StringBuilder.Append(starCount);
            totalStarCount += starCount;
            m_StringBuilder.Append(".");
        }
        m_StringBuilder.Append(CustomStarUsage.m_Instance.GetUsage(iMax));
        m_StringBuilder.Append("|");
        starCount = GetStarRow(iMax).selectedStars;
        m_StringBuilder.Append(starCount);
        totalStarCount += starCount;
        string stars = m_StringBuilder.ToString();
        m_StringBuilder.Clear();
        m_Today.m_Stars = stars;
        m_Today.Init();
        Days.m_Instance.ModifyADay(m_Today);
        m_MainWindow.OnStarCountChanged();
    }

    private void OnOpen()
    {
        DateTime today = DateTime.Now;
        int dayCount = Days.m_Instance.Count;
        for (int i = 0; i < dayCount; i++)
        {
            DayInfo day = Days.m_Instance.Get(i);
            if (day.IsDay(today))
            {
                m_Today = day;
                RefreshDay(day);
                return;
            }
        }
        int starUsageCount = CustomStarUsage.m_Instance.m_StarUsageCount - 1;
        if (starUsageCount < 0)
        {
            m_Today = new DayInfo();
            return;
        }
        for (int i = 0; i < starUsageCount; i++)
        {
            m_StringBuilder.Append("|0");
            m_StringBuilder.Append(".");
        }
        m_StringBuilder.Append("|0");
        string stars = m_StringBuilder.ToString();
        m_StringBuilder.Clear();
        m_Today = new DayInfo(today.Year, today.Month, today.Day, stars);
        RefreshDay(m_Today);
    }

    private void RefreshDay(DayInfo di)
    {
        totalStarCount = 0;
        int iMax = Mathf.Min(di.starsCount, m_StarRows.Length);
        for (int i = 0; i < iMax; i++)
        {
            byte starCount = di.GetStarCount(i);
            if (starCount == 0)
            {
                m_StarRows[i].ClickStar(null, true);
            }
            else
            {
                m_StarRows[i].ClickStar(m_StarRows[i].m_Stars[starCount - 1], true);
                totalStarCount += starCount;
            }
        }
        m_MainWindow.OnStarCountChanged();
    }

    public int starRowCount
    {
        get
        {
            return m_StarRows.Length;
        }
    }

    public StarRow GetStarRow(int index)
    {
        return m_StarRows[index];
    }

    public void OnScroll()
    {
        // This should be UGUI's bug.
        foreach (StarRow starRow in m_StarRows)
        {
            starRow.OnScroll();
        }
    }

    private void AdjustStarRows(int starUsageCount)
    {
        if (starUsageCount <= 0)
        {
            return;
        }
        StarRow[] originalStarRows = m_StarRows;
        m_StarRows = new StarRow[starUsageCount];
        int originalStarRowsLength = 1;
        if (originalStarRows != null)
        {
            int iMax = Mathf.Min(starUsageCount, originalStarRows.Length);
            for (int i = 0; i < iMax; i++)
            {
                m_StarRows[i] = originalStarRows[i];
            }
            originalStarRowsLength = originalStarRows.Length;
        }
        else if (starUsageCount > 0)
        {
            m_StarRows[0] = m_StarRow;
        }
        for (int i = originalStarRowsLength; i < starUsageCount; i++)
        {
            GameObject starRowGameObject = Instantiate(m_StarRowTemplate);
            m_StringBuilder.Append("Star");
            m_StringBuilder.Append(i);
            starRowGameObject.name = m_StringBuilder.ToString();
            m_StringBuilder.Clear();
            Transform starRowGameObjectTr = starRowGameObject.transform;
            starRowGameObjectTr.SetParent(m_Transform);
            starRowGameObjectTr.localScale = Vector3.one;
            m_StarRows[i] = starRowGameObject.GetComponent<StarRow>();
        }
        if (originalStarRowsLength > 1)
        {
            for (int i = starUsageCount; i < originalStarRowsLength; i++)
            {
                Destroy(originalStarRows[i].gameObject);
            }
        }
    }

    private void AdjustStars(int starUsageCount)
    {
        if (starUsageCount <= 0)
        {
            return;
        }
        for (int i = 0; i < starUsageCount; i++)
        {
            StarRow starRow = m_StarRows[i];
            int starMaxCount = CustomStarUsage.m_Instance.GetStarMaxCount(i);
            Button[] originalStars = starRow.m_Stars;
            if (starMaxCount == originalStars.Length)
            {
                continue;
            }
            GameObject templateStarButton = originalStars[0].gameObject;
            starRow.m_Stars = new Button[starMaxCount];
            int jMax = Mathf.Min(starMaxCount, originalStars.Length);
            for (int j = 0; j < jMax; j++)
            {
                starRow.m_Stars[j] = originalStars[j];
            }
            for (int j = originalStars.Length; j < starMaxCount; j++)
            {
                GameObject starText = Instantiate(templateStarButton);
                m_StringBuilder.Append("Button");
                m_StringBuilder.Append(j);
                starText.name = m_StringBuilder.ToString();
                m_StringBuilder.Clear();
                starText.transform.SetParent(starRow.m_Stars[0].transform.parent);
                starText.transform.localScale = Vector3.one;
                starRow.m_Stars[j] = starText.GetComponent<Button>();
            }
            jMax = originalStars.Length;
            for (int j = starMaxCount; j < jMax; j++)
            {
                Destroy(originalStars[j].gameObject);
            }
        }
    }
}
