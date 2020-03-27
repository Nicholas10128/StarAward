using System.Text;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StarUsageModifer : MonoBehaviour
{
    public StarModifier m_StarRecord;
    public Transform m_AddButton;

    private Transform m_Transform;
    private List<StarModifier> m_StarModifiers = new List<StarModifier>();
    private StringBuilder m_StringBuilder = new StringBuilder();

    void Start()
    {
        m_Transform = transform;
        m_StarModifiers.Add(m_StarRecord);
    }

    void Update()
    {
        
    }

    public void RefreshUI()
    {
        int starUsageCount = CustomStarUsage.m_Instance.m_StarUsageCount;
        int existUICount = m_StarModifiers.Count;
        if (starUsageCount != existUICount)
        {
            for (int i = existUICount; i < starUsageCount; i++)
            {
                OnAddButtonClick();
            }
        }
        for (int i = 0; i < starUsageCount; i++)
        {
            StarModifier starModifier = m_StarModifiers[i];
            starModifier.m_UsageInput.text = CustomStarUsage.m_Instance.GetUsage(i);
            starModifier.m_MaxStar.value = CustomStarUsage.m_Instance.GetStarMaxCount(i);
            starModifier.m_MaxStarText.GetComponent<SliderValue>().OnValueChanged(starModifier.m_MaxStar);
        }
    }

    public void OnAddButtonClick()
    {
        GameObject newStarRecord = Instantiate(m_StarRecord.gameObject);
        StarModifier newStarModifier = newStarRecord.GetComponent<StarModifier>();
        newStarModifier.m_UsageInput.text = string.Empty;
        newStarModifier.m_MaxStar.value = 3;
        newStarModifier.m_MaxStarText.GetComponent<SliderValue>().OnValueChanged(newStarModifier.m_MaxStar);
        m_StringBuilder.Append("Star");
        m_StringBuilder.Append(m_StarModifiers.Count);
        newStarRecord.name = m_StringBuilder.ToString();
        m_StringBuilder.Clear();
        Transform newStarRecordTransform = newStarRecord.transform;
        newStarRecordTransform.SetParent(m_Transform);
        newStarRecordTransform.SetSiblingIndex(m_StarModifiers.Count);
        newStarRecordTransform.localScale = Vector3.one;
        m_StarModifiers.Add(newStarModifier);
    }

    public void OnDeleteButtonClick(Button btn)
    {
        int iMax = m_StarModifiers.Count;
        if (iMax <= 1)
        {
            // Delete the last one is ban.
            //MessageBox
            return;
        }
        for (int i = 0; i < iMax; i++)
        {
            if (ReferenceEquals(btn, m_StarModifiers[i].m_DeleteButton))
            {
                bool templateDeleted = ReferenceEquals(m_StarRecord, m_StarModifiers[i]);
                Destroy(m_StarModifiers[i].gameObject);
                m_StarModifiers.RemoveAt(i);
                if (templateDeleted)
                {
                    m_StarRecord = m_StarModifiers[0];
                }
                break;
            }
        }
    }

    public void Save()
    {
        CustomStarUsage customStarUsage = CustomStarUsage.m_Instance;
        int iMax = customStarUsage.m_StarUsageCount = m_StarModifiers.Count;
        for (int i = 0; i < iMax; i++)
        {
            StarModifier starModifier = m_StarModifiers[i];
            customStarUsage.ResetUsage(i, starModifier.m_UsageInput.text);
            customStarUsage.ResetStarMaxCount(i, (byte)starModifier.m_MaxStar.value);
        }
        customStarUsage.Save();
    }
}
