using System;
using System.Text;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GCT.UI;

public class UseStarModifer : MonoBehaviour
{
    public UseStar m_StarRecord;
    public Transform m_AddButton;

    private Transform m_Transform;
    private List<UseStar> m_StarModifiers = new List<UseStar>();
    private StringBuilder m_StringBuilder = new StringBuilder();
    private List<short> m_DeletedIndexList = new List<short>();

    private MessageBox.Callback m_DeleteConfirmCallbackDelegate;

    void Awake()
    {
        m_Transform = transform;
        m_StarModifiers.Add(m_StarRecord);
        m_DeleteConfirmCallbackDelegate = DeleteConfirmCallback;
    }

    public void OnSync()
    {
        RefreshUI();
    }

    void Update()
    {
        
    }

    public void RefreshUI()
    {
        int starUsageCount = UseStarHistory.m_Instance.Count;
        int existUICount = m_StarModifiers.Count;
        if (starUsageCount != existUICount)
        {
            if (starUsageCount == 0)
            {
                UseStar starModifier = m_StarModifiers[0];
            }
            else
            {
                for (int i = existUICount; i < starUsageCount; i++)
                {
                    OnAddButtonClick();
                }
                Vector3 pos = m_Transform.localPosition;
                pos.y = starUsageCount * 200;
                m_Transform.localPosition = pos;
            }
        }
        for (int i = 0; i < starUsageCount; i++)
        {
            UseStar starModifier = m_StarModifiers[i];
            UseStarInfo useStarInfo = UseStarHistory.m_Instance.Get(i);
            starModifier.m_UsageInput.text = useStarInfo.m_Usage;
            starModifier.m_UseStarInput.text = useStarInfo.m_Count.ToString();
            if (string.IsNullOrEmpty(useStarInfo.m_Date))
            {
                useStarInfo.m_Date = DateTime.Now.ToString("yyyy年M月d日");
            }
            starModifier.m_Date.text = useStarInfo.m_Date;
        }
    }

    public void OnUsageEndInput(InputField input)
    {
        int iMax = m_StarModifiers.Count;
        for (int i = 0; i < iMax; i++)
        {
            UseStar starModifier = m_StarModifiers[i];
            if (ReferenceEquals(input, starModifier.m_UsageInput))
            {
                UseStarInfo useStarInfo = UseStarHistory.m_Instance.Get(i);
                useStarInfo.m_Usage = starModifier.m_UsageInput.text;
                break;
            }
        }
    }

    public void OnCountEndInput(InputField input)
    {
        int iMax = m_StarModifiers.Count;
        for (int i = 0; i < iMax; i++)
        {
            UseStar starModifier = m_StarModifiers[i];
            if (ReferenceEquals(input, starModifier.m_UseStarInput))
            {
                UseStarInfo useStarInfo = UseStarHistory.m_Instance.Get(i);
                int.TryParse(starModifier.m_UseStarInput.text, out useStarInfo.m_Count);
                break;
            }
        }
    }

    public void OnAddButtonClick()
    {
        GameObject newStarRecord = Instantiate(m_StarRecord.gameObject);
        UseStar newStarModifier = newStarRecord.GetComponent<UseStar>();
        newStarModifier.m_UsageInput.text = string.Empty;
        newStarModifier.m_Date.text = DateTime.Now.ToString("yyyy年M月d日");
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
            MessageBox.Show("警告", "至少得留一个吧", TextAnchor.MiddleCenter, null, null, 3);
            return;
        }
        for (int i = 0; i < iMax; i++)
        {
            if (ReferenceEquals(btn, m_StarModifiers[i].m_DeleteButton))
            {
                m_StringBuilder.Append("是否确定要删除");
                m_StringBuilder.Append(m_StarModifiers[i].m_UsageInput.text);
                m_StringBuilder.Append("花掉");
                m_StringBuilder.Append(m_StarModifiers[i].m_UseStarInput.text);
                m_StringBuilder.Append("颗星星的记录？");
                MessageBox.Show("提示", m_StringBuilder.ToString(), MessageBox.Type.YesOrNo, TextAnchor.MiddleCenter, m_DeleteConfirmCallbackDelegate, i);
                m_StringBuilder.Clear();
                break;
            }
        }
    }

    public void Save()
    {
        UseStarHistory useStarHistory = UseStarHistory.m_Instance;
        int iPrevMax = useStarHistory.Count;
        int iMax = m_StarModifiers.Count;
        for (int i = 0; i < iMax; i++)
        {
            UseStar starModifier = m_StarModifiers[i];
            UseStarInfo useStarInfo = useStarHistory.GetOrAdd(i);
            useStarInfo.m_Usage = starModifier.m_UsageInput.text;
            useStarInfo.m_Date = starModifier.m_Date.text;
            if (string.IsNullOrEmpty(useStarInfo.m_Date))
            {
                useStarInfo.m_Date = DateTime.Now.ToString("yyyy年M月d日");
            }
            int.TryParse(starModifier.m_UseStarInput.text, out useStarInfo.m_Count);
        }
        for (int i = iMax; i < iPrevMax; i++)
        {
            useStarHistory.RemoveAt(i);
        }
        useStarHistory.Save();
    }

    public bool ArchiveIsDirty()
    {
        int useStarCount = UseStarHistory.m_Instance.Count;
        int userInterfaceCount = m_StarModifiers.Count;
        if (string.IsNullOrEmpty(m_StarModifiers[userInterfaceCount - 1].m_UsageInput.text))
        {
            userInterfaceCount--;
        }
        if (useStarCount != userInterfaceCount)
        {
            return true;
        }
        for (int i = 0; i < useStarCount; i++)
        {
            UseStarInfo useStarInfo = UseStarHistory.m_Instance.Get(i);
            UseStar starModifier = m_StarModifiers[i];
            if (useStarInfo.m_Usage != starModifier.m_UsageInput.text || useStarInfo.m_Count.ToString() != starModifier.m_UseStarInput.text)
            {
                return true;
            }
        }
        return false;
    }

    void DeleteConfirmCallback(MessageBox.ButtonID bid, object parameter)
    {
        if (bid == MessageBox.ButtonID.Confirm)
        {
            int i = (int)parameter;
            bool templateDeleted = ReferenceEquals(m_StarRecord, m_StarModifiers[i]);
            Destroy(m_StarModifiers[i].gameObject);
            m_StarModifiers.RemoveAt(i);
            short realIndex = (short)i;
            foreach (short deletedIndex in m_DeletedIndexList)
            {
                if (deletedIndex < realIndex)
                {
                    realIndex++;
                }
            }
            m_DeletedIndexList.Add(realIndex);
            if (templateDeleted)
            {
                m_StarRecord = m_StarModifiers[0];
            }
        }
    }
}
