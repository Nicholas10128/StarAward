using System.Text;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GCT.UI;

public class StarUsageModifer : MonoBehaviour
{
    public StarModifier m_StarRecord;
    public Transform m_AddButton;

    private Transform m_Transform;
    private List<StarModifier> m_StarModifiers = new List<StarModifier>();
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
        int starUsageCount = CustomStarUsage.m_Instance.m_StarUsageCount;
        int existUICount = m_StarModifiers.Count;
        if (starUsageCount != existUICount)
        {
            if (starUsageCount == 0)
            {
                StarModifier starModifier = m_StarModifiers[0];
                starModifier.m_MaxStar.value = 3;
                starModifier.m_MaxStarText.GetComponent<SliderValue>().OnValueChanged(starModifier.m_MaxStar);
            }
            else
            {
                for (int i = existUICount; i < starUsageCount; i++)
                {
                    OnAddButtonClick();
                }
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
            MessageBox.Show("警告", "至少得留一个吧", TextAnchor.MiddleCenter, null, null, 3);
            return;
        }
        for (int i = 0; i < iMax; i++)
        {
            if (ReferenceEquals(btn, m_StarModifiers[i].m_DeleteButton))
            {
                // 删除用途的时候要一并删除已经取得的星星记录。并提示会丢失多少颗星星。
                long totalCount = Days.m_Instance.GetTodayCountByUsage(i);
                if (totalCount > 0)
                {
                    m_StringBuilder.Append("删除");
                    m_StringBuilder.Append(m_StarModifiers[i].m_UsageInput.text);
                    m_StringBuilder.Append("会导致");
                    m_StringBuilder.Append(totalCount);
                    m_StringBuilder.Append("颗星星丢失，是否确定要删除？");
                    MessageBox.Show("提示", m_StringBuilder.ToString(), MessageBox.Type.YesOrNo, TextAnchor.MiddleCenter, m_DeleteConfirmCallbackDelegate, i);
                    m_StringBuilder.Clear();
                }
                else
                {
                    m_DeleteConfirmCallbackDelegate(MessageBox.ButtonID.Confirm, i);
                }
                break;
            }
        }
    }

    public void Save()
    {
        CustomStarUsage customStarUsage = CustomStarUsage.m_Instance;
        int iPrevMax = customStarUsage.m_StarUsageCount;
        int iMax = customStarUsage.m_StarUsageCount = m_StarModifiers.Count;
        for (int i = 0; i < iMax; i++)
        {
            StarModifier starModifier = m_StarModifiers[i];
            customStarUsage.ResetUsage(i, starModifier.m_UsageInput.text);
            customStarUsage.ResetStarMaxCount(i, (byte)starModifier.m_MaxStar.value);
        }
        for (int i = iMax; i < iPrevMax; i++)
        {
            customStarUsage.ResetUsage(i, " ");
            customStarUsage.ResetStarMaxCount(i, 0);
        }
        customStarUsage.Save();
    }

    public bool ArchiveIsDirty()
    {
        int starUsageCount = CustomStarUsage.m_Instance.m_StarUsageCount;
        int userInterfaceCount = m_StarModifiers.Count;
        if (string.IsNullOrEmpty(m_StarModifiers[userInterfaceCount - 1].m_UsageInput.text))
        {
            userInterfaceCount--;
        }
        if (starUsageCount != userInterfaceCount)
        {
            return true;
        }
        for (int i = 0; i < starUsageCount; i++)
        {
            string strUsage = CustomStarUsage.m_Instance.GetUsage(i);
            StarModifier starModifier = m_StarModifiers[i];
            if (strUsage != starModifier.m_UsageInput.text)
            {
                return true;
            }
            byte starMaxCount = CustomStarUsage.m_Instance.GetStarMaxCount(i);
            if (starMaxCount != starModifier.m_MaxStar.value)
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
            Days.m_Instance.RemoveTodayCount(i);
        }
    }
}
