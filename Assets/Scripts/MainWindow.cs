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
    public Image m_StarImage;
    public StarUsageModifer m_StarUsageModifer;
    public StarUsage m_StarUsage;
    public UseStarModifer m_UseStarModifer;

    public RectTransform[] m_TabButtons;
    private Vector3 m_UnselectScale = new Vector3(0.7f, 0.7f, 1f);
    private Vector3 m_SelectedScale = Vector3.one;

    public GameObject[] m_TabPages;

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

    public void OnStarCountChanged()
    {
        m_StarCount.text = m_StarUsage.totalStarCount.ToString();
    }

    void SettingsNotSaveConfirmCallback(MessageBox.ButtonID bid, object parameter)
    {
        if (bid == MessageBox.ButtonID.Confirm)
        {
            m_StarUsageModifer.Save();
            m_StarUsage.RefreshUI();
            m_StarUsage.RefreshToday();
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
                m_StarToday.text = string.Empty;
                m_StarCount.text = string.Empty;
                m_StarImage.enabled = false;
                break;
            case TabButtonUsage.TBU_Stars:
                m_StarUsage.RefreshUI();
                m_StarToday.text = "今日获得星星";
                m_StarCount.text = m_StarUsage.totalStarCount.ToString();
                m_StarImage.enabled = true;
                break;
            case TabButtonUsage.TBU_Spend:
                m_UseStarModifer.RefreshUI();
                m_StarToday.text = "你共拥有星星";
                long starCountLeft = Days.m_Instance.GetTotalCountByUsage(-1) - UseStarHistory.m_Instance.GetTotalCount();
                m_StarCount.text = starCountLeft.ToString();
                m_StarImage.enabled = true;
                break;
        }
    }
}
