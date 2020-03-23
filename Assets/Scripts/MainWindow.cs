using UnityEngine;

public class MainWindow : MonoBehaviour
{
    public Transform m_ContentNode;

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

    }
}
