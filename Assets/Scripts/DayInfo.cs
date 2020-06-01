using System;
using System.Text;
using UnityEngine;

public class DayInfo
{
    public int m_Year;
    public int m_Month;
    public int m_Day;
    public string m_Stars;

    private string[] m_StarsName;
    private byte[] m_StarsCount;

    private StringBuilder m_StringBuilder = new StringBuilder();

    public int starsCount
    {
        get
        {
            return m_StarsCount.Length;
        }
    }

    public string GetStarName(int index)
    {
        return m_StarsName[index];
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
        m_StarsName = new string[Mathf.Max(CustomStarUsage.m_Instance.m_StarUsageCount, iMax)];
        m_StarsCount = new byte[m_StarsName.Length];
        for (int i = 0; i < iMax; i++)
        {
            string[] nameAndCount = strs[i].Split('|');
            string strCount = null;
            if (nameAndCount.Length == 1) // Old version archive.
            {
                strCount = nameAndCount[0];
            }
            else
            {
                m_StarsName[i] = nameAndCount[0];
                strCount = nameAndCount[1];
            }
            if (!byte.TryParse(strCount, out m_StarsCount[i]))
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

    public void RemoveCount(int index)
    {
        int count = starsCount - 1;
        string[] newNames = new string[count];
        byte[] newCounts = new byte[count];
        for (int i = 0, j = 0; i < count; i++, j++)
        {
            if (i == index)
            {
                j++;
            }
            newNames[i] = m_StarsName[j];
            newCounts[i] = m_StarsCount[j];
        }
        m_StarsName = newNames;
        m_StarsCount = newCounts;
    }
}
