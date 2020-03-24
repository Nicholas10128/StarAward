using UnityEngine;

public class MainWindow : MonoBehaviour
{
    public Transform m_ContentNode;

    public Canvas m_MainWindow;
    public Canvas m_ModifyADay;
    public Canvas m_CustomStarUsage;

    public DaysSheet m_DaysSheet;

    void Start()
    {
        CustomStarUsage.Reload();
        Days.m_Instance.Reload();
    }

    void Update()
    {
        
    }

    void RefreshUI()
    {
        m_DaysSheet.RefreshUI();
    }

    public void CloseModifyADay(bool modified)
    {
        m_ModifyADay.enabled = false;
        if (modified)
        {
            RefreshUI();
            m_MainWindow.enabled = true;
        }
    }
}
