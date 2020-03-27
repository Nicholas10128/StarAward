using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class SliderValue : MonoBehaviour
{
    private static StringBuilder m_StringBuilder = new StringBuilder();
    private Text m_Text;

    private float m_CachedValue = 0; 

    void Start()
    {
        m_Text = GetComponent<Text>();
        m_StringBuilder.Append(m_CachedValue);
        m_Text.text = m_StringBuilder.ToString();
        m_StringBuilder.Clear();
    }

    public void OnValueChanged(Slider slider)
    {
        m_CachedValue = slider.value;
        if (ReferenceEquals(m_Text, null))
        {
            return;
        }
        m_StringBuilder.Append(slider.value);
        m_Text.text = m_StringBuilder.ToString();
        m_StringBuilder.Clear();
    }
}
