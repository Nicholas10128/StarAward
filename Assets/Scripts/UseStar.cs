using UnityEngine;
using UnityEngine.UI;
using GCT.UI;

public class UseStar : MonoBehaviour
{
    public Button m_DeleteButton;
    public InputField m_UsageInput;
    public InputField m_UseStarInput;
    public Text m_Date;

    private void Start()
    {
        m_UseStarInput.onValueChanged.AddListener(OnIntegerValueChange);
    }

    void OnIntegerValueChange(string inputContent)
    {
        if (inputContent.IndexOf('.') >= 0)
        {
            m_UseStarInput.text = inputContent.Replace(".", "");
        }
        if (inputContent.IndexOf(',') >= 0)
        {
            m_UseStarInput.text = inputContent.Replace(",", "");
        }
    }
}
