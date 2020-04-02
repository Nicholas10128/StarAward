using System.IO;
using System.Collections.Generic;
using UnityEngine;
using Sinbad;

public class Days
{
    public static Days m_Instance = new Days();

    private string m_ArchiveFilePath = Application.persistentDataPath + "/archive.csv";
    private List<DayInfo> m_Days = new List<DayInfo>();

    public int Count
    {
        get
        {
            return m_Days.Count;
        }
    }

    public DayInfo Get(int index)
    {
        return m_Days[index];
    }

    public void ModifyADay(DayInfo day)
    {
        foreach (DayInfo di in m_Days)
        {
            if (di.m_Year == day.m_Year && di.m_Month == day.m_Month && di.m_Day == day.m_Day)
            {
                di.Modify(day);
                return;
            }
        }
        m_Days.Add(day);
        Save();
    }

    public void Reload()
    {
        m_Days.Clear();
        if (File.Exists(m_ArchiveFilePath))
        {
            m_Days = CsvUtil.LoadObjects<DayInfo>(m_ArchiveFilePath);
        }
        foreach (DayInfo day in m_Days)
        {
            day.Init();
        }
    }

    public void Save()
    {
        if (m_Days.Count > 0)
        {
            CsvUtil.SaveObjects(m_Days, m_ArchiveFilePath);
        }
    }

    public long GetTotalCountByUsage(int usage)
    {
        long totalCount = 0;
        if (usage >= 0)
        {
            foreach (DayInfo di in m_Days)
            {
                totalCount += di.GetStarCount(usage);
            }
        }
        else
        {
            for (int i = 0, iMax = CustomStarUsage.m_Instance.m_StarUsageCount; i < iMax; i++)
            {
                foreach (DayInfo di in m_Days)
                {
                    totalCount += di.GetStarCount(i);
                }
            }
        }
        return totalCount;
    }
}
