using System;
using UnityEngine;

public class DayInfo
{
    public int m_Year;
    public int m_Month;
    public int m_Day;
    public string m_Stars;

    private byte[] m_StarsCount;

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
        m_StarsCount = new byte[iMax];
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
}
