using System;
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

    public void Reload()
    {
        m_Days.Clear();
        m_Days = CsvUtil.LoadObjects<DayInfo>(m_ArchiveFilePath);
        foreach (DayInfo day in m_Days)
        {
            day.Init();
        }
    }

    public void Save()
    {
        List<DayInfo> days = new List<DayInfo>();
        days.Add(new DayInfo(2020, 3, 23, "1.2.3.4"));
        CsvUtil.SaveObjects(days, m_ArchiveFilePath);
    }
}
