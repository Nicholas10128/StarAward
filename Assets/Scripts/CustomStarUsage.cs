using System.IO;
using System.Text;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Sinbad;

public class CustomStarUsage
{
    public const int MAX_COUNT = 50;

    public static CustomStarUsage m_Instance;

    public int m_StarUsageCount = 0;

    public string m_StarUsage1 = " ";
    public string m_StarUsage2 = " ";
    public string m_StarUsage3 = " ";
    public string m_StarUsage4 = " ";
    public string m_StarUsage5 = " ";
    public string m_StarUsage6 = " ";
    public string m_StarUsage7 = " ";
    public string m_StarUsage8 = " ";
    public string m_StarUsage9 = " ";
    public string m_StarUsage10 = " ";

    public string m_StarUsage11 = " ";
    public string m_StarUsage12 = " ";
    public string m_StarUsage13 = " ";
    public string m_StarUsage14 = " ";
    public string m_StarUsage15 = " ";
    public string m_StarUsage16 = " ";
    public string m_StarUsage17 = " ";
    public string m_StarUsage18 = " ";
    public string m_StarUsage19 = " ";
    public string m_StarUsage20 = " ";

    public string m_StarUsage21 = " ";
    public string m_StarUsage22 = " ";
    public string m_StarUsage23 = " ";
    public string m_StarUsage24 = " ";
    public string m_StarUsage25 = " ";
    public string m_StarUsage26 = " ";
    public string m_StarUsage27 = " ";
    public string m_StarUsage28 = " ";
    public string m_StarUsage29 = " ";
    public string m_StarUsage30 = " ";

    public string m_StarUsage31 = " ";
    public string m_StarUsage32 = " ";
    public string m_StarUsage33 = " ";
    public string m_StarUsage34 = " ";
    public string m_StarUsage35 = " ";
    public string m_StarUsage36 = " ";
    public string m_StarUsage37 = " ";
    public string m_StarUsage38 = " ";
    public string m_StarUsage39 = " ";
    public string m_StarUsage40 = " ";

    public string m_StarUsage41 = " ";
    public string m_StarUsage42 = " ";
    public string m_StarUsage43 = " ";
    public string m_StarUsage44 = " ";
    public string m_StarUsage45 = " ";
    public string m_StarUsage46 = " ";
    public string m_StarUsage47 = " ";
    public string m_StarUsage48 = " ";
    public string m_StarUsage49 = " ";
    public string m_StarUsage50 = " ";

    private byte[] m_StarsMaxCount = new byte[MAX_COUNT];

    private string m_ArchiveFilePath = Application.persistentDataPath + "/starUsage.csv";
    private StringBuilder m_StringBuilder = new StringBuilder();
    private List<string> m_StringListBuffer = new List<string>(16);

    public static void Reload()
    {
        m_Instance = new CustomStarUsage();
        if (File.Exists(m_Instance.m_ArchiveFilePath))
        {
            CsvUtil.LoadObject(m_Instance.m_ArchiveFilePath, ref m_Instance);
        }
        for (int i = 0; i < m_Instance.m_StarUsageCount; i++)
        {
            string strUsage = m_Instance.GetUsage(i);
            string[] strs = strUsage.Split('.');
            if (strs.Length != 2)
            {
                Debug.LogError("Invalid star usage format: " + strUsage);
                break;
            }
            m_Instance.ResetUsage(i, strs[0]);
            if (!byte.TryParse(strs[1], out m_Instance.m_StarsMaxCount[i]))
            {
                Debug.LogError("Invalid star usage format: " + strUsage);
                break;
            }
        }
    }

    public void Save()
    {
        int starUsageCount = m_Instance.m_StarUsageCount;
        for (int i = 0; i < starUsageCount; i++)
        {
            string strUsage = m_Instance.GetUsage(i);
            m_StringListBuffer.Add(strUsage);
            m_StringBuilder.Append(strUsage);
            m_StringBuilder.Append(".");
            m_StringBuilder.Append(m_Instance.m_StarsMaxCount[i]);
            m_Instance.ResetUsage(i, m_StringBuilder.ToString());
            m_StringBuilder.Clear();
        }
        CsvUtil.SaveObject(this, m_ArchiveFilePath);
        // 上面Save的时候加入了"."，需要还原显示。
        for (int i = 0; i < starUsageCount; i++)
        {
            m_Instance.ResetUsage(i, m_StringListBuffer[i]);
        }
        m_StringListBuffer.Clear();
    }

    public void Desearialize(NetworkReader reader)
    {
        m_StarUsageCount = reader.ReadInt32();
        for (int i = 0; i < MAX_COUNT; i++)
        {
            ResetUsage(i, reader.ReadString());
            ResetStarMaxCount(i, reader.ReadByte());
        }
    }

    public void Searialize(NetworkWriter writer)
    {
        writer.Write(m_StarUsageCount);
        for (int i = 0; i < MAX_COUNT; i++)
        {
            writer.Write(GetUsage(i));
            writer.Write(GetStarMaxCount(i));
        }
    }

    public string GetUsage(int index)
    {
        // Such ugly codes are due to UnityCsvUtil's reflection using.
        switch(index)
        {
            case 0:
                return m_StarUsage1;
            case 1:
                return m_StarUsage2;
            case 2:
                return m_StarUsage3;
            case 3:
                return m_StarUsage4;
            case 4:
                return m_StarUsage5;
            case 5:
                return m_StarUsage6;
            case 6:
                return m_StarUsage7;
            case 7:
                return m_StarUsage8;
            case 8:
                return m_StarUsage9;
            case 9:
                return m_StarUsage10;
            case 10:
                return m_StarUsage11;
            case 11:
                return m_StarUsage12;
            case 12:
                return m_StarUsage13;
            case 13:
                return m_StarUsage14;
            case 14:
                return m_StarUsage15;
            case 15:
                return m_StarUsage16;
            case 16:
                return m_StarUsage17;
            case 17:
                return m_StarUsage18;
            case 18:
                return m_StarUsage19;
            case 19:
                return m_StarUsage20;
            case 20:
                return m_StarUsage21;
            case 21:
                return m_StarUsage22;
            case 22:
                return m_StarUsage23;
            case 23:
                return m_StarUsage24;
            case 24:
                return m_StarUsage25;
            case 25:
                return m_StarUsage26;
            case 26:
                return m_StarUsage27;
            case 27:
                return m_StarUsage28;
            case 28:
                return m_StarUsage29;
            case 29:
                return m_StarUsage20;
            case 30:
                return m_StarUsage31;
            case 31:
                return m_StarUsage32;
            case 32:
                return m_StarUsage33;
            case 33:
                return m_StarUsage34;
            case 34:
                return m_StarUsage35;
            case 35:
                return m_StarUsage36;
            case 36:
                return m_StarUsage37;
            case 37:
                return m_StarUsage38;
            case 38:
                return m_StarUsage39;
            case 39:
                return m_StarUsage40;
            case 40:
                return m_StarUsage41;
            case 41:
                return m_StarUsage42;
            case 42:
                return m_StarUsage43;
            case 43:
                return m_StarUsage44;
            case 44:
                return m_StarUsage45;
            case 45:
                return m_StarUsage46;
            case 46:
                return m_StarUsage47;
            case 47:
                return m_StarUsage48;
            case 48:
                return m_StarUsage49;
            case 49:
                return m_StarUsage50;
        }

        Debug.LogError("Invalid index of custom star usage: " + index);
        return string.Empty;
    }

    public void ResetUsage(int index, string strUsage)
    {
        // Such ugly codes are due to UnityCsvUtil's reflection using.
        switch (index)
        {
            case 0:
                m_StarUsage1 = strUsage;
                return;
            case 1:
                m_StarUsage2 = strUsage;
                return;
            case 2:
                m_StarUsage3 = strUsage;
                return;
            case 3:
                m_StarUsage4 = strUsage;
                return;
            case 4:
                m_StarUsage5 = strUsage;
                return;
            case 5:
                m_StarUsage6 = strUsage;
                return;
            case 6:
                m_StarUsage7 = strUsage;
                return;
            case 7:
                m_StarUsage8 = strUsage;
                return;
            case 8:
                m_StarUsage9 = strUsage;
                return;
            case 9:
                m_StarUsage10 = strUsage;
                return;
            case 10:
                m_StarUsage11 = strUsage;
                return;
            case 11:
                m_StarUsage12 = strUsage;
                return;
            case 12:
                m_StarUsage13 = strUsage;
                return;
            case 13:
                m_StarUsage14 = strUsage;
                return;
            case 14:
                m_StarUsage15 = strUsage;
                return;
            case 15:
                m_StarUsage16 = strUsage;
                return;
            case 16:
                m_StarUsage17 = strUsage;
                return;
            case 17:
                m_StarUsage18 = strUsage;
                return;
            case 18:
                m_StarUsage19 = strUsage;
                return;
            case 19:
                m_StarUsage20 = strUsage;
                return;
            case 20:
                m_StarUsage21 = strUsage;
                return;
            case 21:
                m_StarUsage22 = strUsage;
                return;
            case 22:
                m_StarUsage23 = strUsage;
                return;
            case 23:
                m_StarUsage24 = strUsage;
                return;
            case 24:
                m_StarUsage25 = strUsage;
                return;
            case 25:
                m_StarUsage26 = strUsage;
                return;
            case 26:
                m_StarUsage27 = strUsage;
                return;
            case 27:
                m_StarUsage28 = strUsage;
                return;
            case 28:
                m_StarUsage29 = strUsage;
                return;
            case 29:
                m_StarUsage30 = strUsage;
                return;
            case 30:
                m_StarUsage31 = strUsage;
                return;
            case 31:
                m_StarUsage32 = strUsage;
                return;
            case 32:
                m_StarUsage33 = strUsage;
                return;
            case 33:
                m_StarUsage34 = strUsage;
                return;
            case 34:
                m_StarUsage35 = strUsage;
                return;
            case 35:
                m_StarUsage36 = strUsage;
                return;
            case 36:
                m_StarUsage37 = strUsage;
                return;
            case 37:
                m_StarUsage38 = strUsage;
                return;
            case 38:
                m_StarUsage39 = strUsage;
                return;
            case 39:
                m_StarUsage40 = strUsage;
                return;
            case 40:
                m_StarUsage41 = strUsage;
                return;
            case 41:
                m_StarUsage42 = strUsage;
                return;
            case 42:
                m_StarUsage43 = strUsage;
                return;
            case 43:
                m_StarUsage44 = strUsage;
                return;
            case 44:
                m_StarUsage45 = strUsage;
                return;
            case 45:
                m_StarUsage46 = strUsage;
                return;
            case 46:
                m_StarUsage47 = strUsage;
                return;
            case 47:
                m_StarUsage48 = strUsage;
                return;
            case 48:
                m_StarUsage49 = strUsage;
                return;
            case 49:
                m_StarUsage50 = strUsage;
                return;
        }

        Debug.LogError("Invalid index of custom star usage: " + index);
    }

    public byte GetStarMaxCount(int index)
    {
        return m_StarsMaxCount[index];
    }

    public void ResetStarMaxCount(int index, byte count)
    {
        m_StarsMaxCount[index] = count;
    }
}
