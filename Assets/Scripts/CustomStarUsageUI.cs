using UnityEngine;

public class CustomStarUsageUI : MonoBehaviour
{
    public MainWindow m_MainWindow;
    public StarUsageModifer m_StarUsageModifer;

    private Canvas m_Canvas;

    void Start()
    {
        m_Canvas = GetComponent<Canvas>();
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
        m_MainWindow.CloseCustomStarUsageUI(true);
    }

    public void OnCancelButtonClick()
    {
        //MessageBox
        m_MainWindow.CloseCustomStarUsageUI(false);
    }
}
