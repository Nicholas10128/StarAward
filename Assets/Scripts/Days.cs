using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Sinbad;

public class Days
{
    public static Days m_Instance = new Days();

    private string m_ArchiveFilePath = Application.persistentDataPath + "/archive.csv";
    private List<DayInfo> m_Days = new List<DayInfo>();

    private bool m_CountRemoved = false;

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
            if (di.IsDay(day))
            {
                di.Modify(day);
                Save();
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

    public void Desearialize(NetworkReader reader)
    {
        int count = reader.ReadInt32();
        m_Days.Clear();
        for (int i = 0; i < count; i++)
        {
            DayInfo di = new DayInfo();
            di.m_Year = reader.ReadInt16();
            di.m_Month = reader.ReadByte();
            di.m_Day = reader.ReadByte();
            int starsCount = reader.ReadByte();
            di.RebuildStarName(starsCount);
            di.RebuildStarCount(starsCount);
            for (int j = 0; j < starsCount; j++)
            {
                di.SetStarName(j, reader.ReadString());
                di.SetStarCount(j, reader.ReadByte());
            }
            di.OnRebuild();
            m_Days.Add(di);
        }
    }

    public void Searialize(NetworkWriter writer)
    {
        int count = m_Days.Count;
        writer.Write(count);
        for (int i = 0; i < count; i++)
        {
            DayInfo di = m_Days[i];
            writer.Write((short)di.m_Year);
            writer.Write((byte)di.m_Month);
            writer.Write((byte)di.m_Day);
            int starsCount = di.starsCount;
            writer.Write((byte)di.starsCount);
            for (int j = 0; j < starsCount; j++)
            {
                writer.Write(di.GetStarName(j));
                writer.Write(di.GetStarCount(j));
            }
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

    public long GetTodayCountByUsage(int usage)
    {
        long totalCount = 0;
        if (usage >= 0)
        {
            foreach (DayInfo di in m_Days)
            {
                if (di.IsToday())
                {
                    totalCount += di.GetStarCount(usage);
                    break;
                }
            }
        }
        else
        {
            for (int i = 0, iMax = CustomStarUsage.m_Instance.m_StarUsageCount; i < iMax; i++)
            {
                foreach (DayInfo di in m_Days)
                {
                    if (di.IsToday())
                    {
                        totalCount += di.GetStarCount(i);
                        break;
                    }
                }
            }
        }
        return totalCount;
    }

    public void RemoveTodayCount(int index)
    {
        foreach (DayInfo di in m_Days)
        {
            if (di.IsToday())
            {
                di.RemoveCount(index);
                break;
            }
        }
        m_CountRemoved = true;
    }

    public bool ClearCountRemoved()
    {
        bool countRemoved = m_CountRemoved;
        m_CountRemoved = false;
        return countRemoved;
    }
}
