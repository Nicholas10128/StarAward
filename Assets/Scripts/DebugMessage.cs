using UnityEngine;
using UnityEngine.UI;
using WebGLResolution;

public class DebugMessage : MonoBehaviour
{
    public string m_DebugMessage;

    private Text m_Text;

    void Start()
    {
        m_Text = GetComponent<Text>();
    }

    void Update()
    {
        //m_Text.text = ScreenWindow.ms_DebugMessage;
    }
}
