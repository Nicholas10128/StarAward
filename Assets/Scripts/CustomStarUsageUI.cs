using UnityEngine;
using GCT.UI;

public class CustomStarUsageUI : MonoBehaviour
{
    public MainWindow m_MainWindow;
    public StarUsageModifer m_StarUsageModifer;

    private Canvas m_Canvas;
    private MessageBox.Callback m_CancelConfirmCallbackDelegate;

    void Start()
    {
        m_Canvas = GetComponent<Canvas>();
        m_CancelConfirmCallbackDelegate = CancelConfirmCallback;
    }

    void Update()
    {
        
    }

    public void OnOpen()
    {
        RefreshUI();
        m_Canvas.enabled = true;
    }

    public void OnClose()
    {
        m_Canvas.enabled = false;
    }

    void RefreshUI()
    {
        m_StarUsageModifer.RefreshUI();
    }

    public void OnConfirmButtonClick()
    {
        m_StarUsageModifer.Save();
        Days.m_Instance.ModifyDays();
        m_MainWindow.CloseCustomStarUsageUI(true);
    }

    public void OnCancelButtonClick()
    {
        if (m_StarUsageModifer.ArchiveIsDirty())
        {
            MessageBox.Show("提示", "确定不保存就关闭吗？", MessageBox.Type.YesOrNo, TextAnchor.MiddleCenter, m_CancelConfirmCallbackDelegate, null);
        }
        else
        {
            CancelConfirmCallback(MessageBox.ButtonID.Confirm, null);
        }
    }

    void CancelConfirmCallback(MessageBox.ButtonID bid, object parameter)
    {
        if (bid == MessageBox.ButtonID.Confirm)
        {
            m_MainWindow.CloseCustomStarUsageUI(false);
        }
    }
}
