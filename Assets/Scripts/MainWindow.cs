using System;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using GCT.UI;

public class MainWindow : MonoBehaviour
{
    private enum TabButtonUsage
    {
        TBU_Settings = 0,
        TBU_Stars,
        TBU_Spend
    }

    public Transform m_ContentNode;

    public Canvas m_MainWindow;
    public Text m_DateToday;
    public Text m_StarToday;
    public Text m_StarCount;
    //public ModifyADay m_ModifyADay;
    public StarUsageModifer m_StarUsageModifer;
    public StarUsage m_StarUsage;
    public UseStarModifer m_UseStarModifer;
    //public CustomStarUsageUI m_CustomStarUsageUI;
    //public Canvas m_CustomStarUsage;

    public RectTransform[] m_TabButtons;
    private Vector3 m_UnselectScale = new Vector3(0.7f, 0.7f, 1f);
    private Vector3 m_SelectedScale = Vector3.one;

    public GameObject[] m_TabPages;

    //public DaysSheet m_DaysSheet;

    private StringBuilder m_StringBuilder = new StringBuilder();
    private TabButtonUsage m_TabButtonUsage = TabButtonUsage.TBU_Stars;

    private MessageBox.Callback m_SettingsNotSaveConfirmCallbackDelegate;
    private MessageBox.Callback m_SpendNotSaveConfirmCallbackDelegate;

    void Start()
    {
        CustomStarUsage.Reload();
        Days.m_Instance.Reload();
        UseStarHistory.m_Instance.Reload();
        MessageBox.Initialize();

        m_SettingsNotSaveConfirmCallbackDelegate = SettingsNotSaveConfirmCallback;
        m_SpendNotSaveConfirmCallbackDelegate = SpendNotSaveConfirmCallback;

        m_DateToday.text = DateTime.Now.ToString("yyyy年M月d日");
        m_TabButtons[(int)TabButtonUsage.TBU_Stars].localScale = m_SelectedScale;
        m_TabPages[(int)TabButtonUsage.TBU_Settings].SetActive(false);
        m_TabPages[(int)TabButtonUsage.TBU_Spend].SetActive(false);
    }

    void Update()
    {
        MessageBox.Tick();
    }

    void RefreshUI()
    {
        //m_DaysSheet.RefreshUI();
        //m_ModifyADay.RefreshUI();
        m_StarUsage.RefreshUI();
    }

    //public void OnCheckInTodayButtonClick()
    //{
    //    DateTime today = DateTime.Now;
    //    int dayCount = Days.m_Instance.Count;
    //    for (int i = 0; i < dayCount; i++)
    //    {
    //        DayInfo day = Days.m_Instance.Get(i);
    //        if (day.IsDay(today))
    //        {
    //            //m_ModifyADay.OnOpen(day);
    //            m_StarUsage.RefreshDay(day);
    //            return;
    //        }
    //    }
    //    int starUsageCount = CustomStarUsage.m_Instance.m_StarUsageCount - 1;
    //    for (int i = 0; i < starUsageCount; i++)
    //    {
    //        m_StringBuilder.Append("0");
    //        m_StringBuilder.Append(".");
    //    }
    //    m_StringBuilder.Append("0");
    //    string stars = m_StringBuilder.ToString();
    //    m_StringBuilder.Clear();
    //    DayInfo newDay = new DayInfo(today.Year, today.Month, today.Day, stars);
    //    //m_ModifyADay.OnOpen(newDay);
    //    m_StarUsage.RefreshDay(newDay);
    //}

    //public void OnModifyADayButtonClick(int index)
    //{
    //    DayInfo di = Days.m_Instance.Get(index);
    //    m_ModifyADay.OnOpen(di);
    //}

    //public void CloseModifyADay(bool modified)
    //{
    //    m_ModifyADay.OnClose();
    //    if (modified)
    //    {
    //        RefreshUI();
    //    }
    //}

    public void OnTabButtonClick(int index)
    {
        if (m_TabButtonUsage == TabButtonUsage.TBU_Settings && index != (int)TabButtonUsage.TBU_Settings && m_StarUsageModifer.ArchiveIsDirty())
        {
            MessageBox.Show("提示", "是否要保存修改？", MessageBox.Type.YesOrNo, TextAnchor.MiddleCenter, m_SettingsNotSaveConfirmCallbackDelegate, index);
            return;
        }

        if (m_TabButtonUsage == TabButtonUsage.TBU_Spend && index != (int)TabButtonUsage.TBU_Spend && m_UseStarModifer.ArchiveIsDirty())
        {
            MessageBox.Show("提示", "是否要保存修改？", MessageBox.Type.YesOrNo, TextAnchor.MiddleCenter, m_SpendNotSaveConfirmCallbackDelegate, index);
            return;
        }

        _OnTabButtonClick(index);
    }

    //public void OnCustomStarUsageButtonClick()
    //{
    //    m_CustomStarUsageUI.OnOpen();
    //}

    public void CloseCustomStarUsageUI(bool modified)
    {
        //m_CustomStarUsageUI.OnClose();
        if (modified)
        {
            RefreshUI();
        }
    }

    public void OnStarCountChanged()
    {
        m_StarCount.text = m_StarUsage.totalStarCount.ToString();
    }

    void SettingsNotSaveConfirmCallback(MessageBox.ButtonID bid, object parameter)
    {
        if (bid == MessageBox.ButtonID.Confirm)
        {
            m_StarUsageModifer.Save();
            Days.m_Instance.ModifyDays();
        }

        _OnTabButtonClick((int)parameter);
    }

    void SpendNotSaveConfirmCallback(MessageBox.ButtonID bid, object parameter)
    {
        if (bid == MessageBox.ButtonID.Confirm)
        {
            m_UseStarModifer.Save();
            UseStarHistory.m_Instance.Save();
        }

        _OnTabButtonClick((int)parameter);
    }

    void _OnTabButtonClick(int index)
    {
        for (int i = 0, iMax = m_TabButtons.Length; i < iMax; i++)
        {
            m_TabButtons[i].localScale = i == index ? m_SelectedScale : m_UnselectScale;
            m_TabPages[i].SetActive(i == index);
        }

        switch (m_TabButtonUsage = (TabButtonUsage)index)
        {
            case TabButtonUsage.TBU_Settings:
                m_StarUsageModifer.RefreshUI();
                break;
            case TabButtonUsage.TBU_Stars:
                m_StarUsage.RefreshUI();
                m_StarToday.text = "今日获得星星";
                m_StarCount.text = m_StarUsage.totalStarCount.ToString();
                break;
            case TabButtonUsage.TBU_Spend:
                m_UseStarModifer.RefreshUI();
                m_StarToday.text = "你共拥有星星";
                long starCountLeft = Days.m_Instance.GetTotalCountByUsage(-1) - UseStarHistory.m_Instance.GetTotalCount();
                m_StarCount.text = starCountLeft.ToString();
                break;
        }
    }
}
