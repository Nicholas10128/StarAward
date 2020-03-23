using System;
using UnityEngine;
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

    private string m_ArchiveFilePath = Application.persistentDataPath + "/starUsage.csv";

    public static void Reload()
    {
        m_Instance = new CustomStarUsage();
        CsvUtil.LoadObject(m_Instance.m_ArchiveFilePath, ref m_Instance);
    }

    public void Save()
    {
        m_StarUsageCount = 3;
        m_StarUsage1 = "做作业";
        m_StarUsage2 = "弹钢琴";
        m_StarUsage3 = "吃饭";
        CsvUtil.SaveObject(this, m_ArchiveFilePath);
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
}
