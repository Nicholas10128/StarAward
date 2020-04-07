using System;
using System.Text;
using UnityEngine;

public class DayInfo
{
    public int m_Year;
    public int m_Month;
    public int m_Day;
    public string m_Stars;

    private byte[] m_StarsCount;

    private StringBuilder m_StringBuilder = new StringBuilder();

    public int starsCount
    {
        get
        {
            return m_StarsCount.Length;
        }
    }

    public byte GetStarCount(int index)
    {
        return m_StarsCount[index];
    }

    public DayInfo()
    {
        DateTime now = DateTime.Now;
        m_Year = now.Year;
        m_Month = now.Month;
        m_Day = now.Day;
        m_Stars = string.Empty;
    }

    public DayInfo(int year, int month, int day, string stars)
    {
        m_Year = year;
        m_Month = month;
        m_Day = day;
        m_Stars = stars;

        Init();
    }

    public void Init()
    {
        string[] strs = m_Stars.Split('.');
        int iMax = strs.Length;
        m_StarsCount = new byte[Mathf.Max(CustomStarUsage.m_Instance.m_StarUsageCount, iMax)];
        for (int i = 0; i < iMax; i++)
        {
            if (!byte.TryParse(strs[i], out m_StarsCount[i]))
            {
                Debug.LogError("It's invalid star count: " + strs[i]);
                break;
            }
        }
    }

    public void Modify(DayInfo day)
    {
        m_Stars = day.m_Stars;
        Init();
    }

    public void Modify()
    {
        byte[] originStarCount = m_StarsCount;
        int iMax = CustomStarUsage.m_Instance.m_StarUsageCount;
        m_StarsCount = new byte[iMax];
        iMax = Mathf.Min(iMax, originStarCount.Length) - 1;
        for (int i = 0; i < iMax; i++)
        {
            m_StarsCount[i] = originStarCount[i];
            m_StringBuilder.Append(m_StarsCount[i]);
            m_StringBuilder.Append('.');
        }
        m_StarsCount[iMax] = originStarCount[iMax];
        m_StringBuilder.Append(m_StarsCount[iMax]);
        m_Stars = m_StringBuilder.ToString();
        m_StringBuilder.Clear();
    }

    public bool IsToday()
    {
        return IsDay(DateTime.Now);
    }

    public bool IsDay(DateTime day)
    {
        return m_Year == day.Year && m_Month == day.Month && m_Day == day.Day;
    }

    public bool IsDay(DayInfo day)
    {
        return m_Year == day.m_Year && m_Month == day.m_Month && m_Day == day.m_Day;
    }
}
