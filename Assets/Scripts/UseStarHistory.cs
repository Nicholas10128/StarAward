using System.IO;
using System.Collections.Generic;
using UnityEngine;
using Sinbad;

public class UseStarHistory
{
    public static UseStarHistory m_Instance = new UseStarHistory();

    private string m_ArchiveFilePath = Application.persistentDataPath + "/useStarHistory.csv";
    private List<UseStarInfo> m_UseStars = new List<UseStarInfo>();

    public int Count
    {
        get
        {
            return m_UseStars.Count;
        }
    }

    public UseStarInfo Get(int index)
    {
        return m_UseStars[index];
    }

    public UseStarInfo GetOrAdd(int index)
    {
        while (m_UseStars.Count <= index)
        {
            m_UseStars.Add(new UseStarInfo());
        }
        return m_UseStars[index];
    }

    public void RemoveAt(int index)
    {
        m_UseStars.RemoveAt(index);
    }

    public void Reload()
    {
        m_UseStars.Clear();
        if (File.Exists(m_ArchiveFilePath))
        {
            m_UseStars = CsvUtil.LoadObjects<UseStarInfo>(m_ArchiveFilePath);
        }
    }

    public void Save()
    {
        if (m_UseStars.Count > 0)
        {
            CsvUtil.SaveObjects(m_UseStars, m_ArchiveFilePath);
        }
    }

    public long GetTotalCount()
    {
        long totalCount = 0;
        foreach (UseStarInfo usi in m_UseStars)
        {
            totalCount += usi.m_Count;
        }
        return totalCount;
    }
}
