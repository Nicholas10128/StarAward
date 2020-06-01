using UnityEngine;
using UnityEngine.UI;

public class StarRow : MonoBehaviour
{
    public StarUsage m_StarUsage;
    public GridLayoutGroup m_ParentGridLayoutGroup;
    public GridLayoutGroup m_GridLayoutGroup;

    public Text m_Usage;
    public Button[] m_Stars;

    public byte selectedStars { get; private set; }

    private Transform m_Transform;
    private RectTransform m_UsageTransform;

    private static bool m_TemplateInited = false;
    private static ColorBlock m_ActiveColor;
    private static ColorBlock m_DeactiveColor;

    void Start()
    {
        m_Transform = m_GridLayoutGroup.transform;
        m_UsageTransform = m_Usage.GetComponent<RectTransform>();
        RefreshColumns();
        if (m_TemplateInited)
        {
            RefreshUI();
        }
    }

    public void TemplateInit()
    {
        m_ActiveColor = m_Stars[0].colors;
        m_DeactiveColor = m_Stars[0].colors;
        m_DeactiveColor.normalColor = Color.grey;
        RefreshUI();
        m_TemplateInited = true;
    }

    public void RefreshColumns()
    {
        if (ReferenceEquals(m_Transform, null))
        {
            return;
        }
        Vector2 cellSize = m_ParentGridLayoutGroup.cellSize;
        float width = m_UsageTransform.offsetMax.x - m_UsageTransform.offsetMin.x;
        cellSize.x = Mathf.Max(cellSize.x, Mathf.FloorToInt((m_GridLayoutGroup.cellSize.x + m_GridLayoutGroup.spacing.x) * m_Transform.childCount + width));
        m_ParentGridLayoutGroup.cellSize = cellSize;
    }

    public void RefreshUI()
    {
        int iMax = m_Stars.Length;
        for (int j = 0; j < selectedStars; j++)
        {
            m_Stars[j].colors = m_ActiveColor;
        }
        for (int j = selectedStars; j < iMax; j++)
        {
            m_Stars[j].colors = m_DeactiveColor;
        }
    }

    public void OnStarClick(Button btn)
    {
        ClickStar(btn, false);
    }

    public void ClickStar(Button btn, bool byScript)
    {
        int iMax = m_Stars.Length;
        for (int i = 0; i < iMax; i++)
        {
            if (ReferenceEquals(m_Stars[i], btn))
            {
                selectedStars = (byte)(i + 1);
                RefreshUI();
                btn.OnDeselect(null);
                if (!byScript)
                {
                    m_StarUsage.OnStarChanged();
                }
                return;
            }
        }
        selectedStars = 0;
        RefreshUI();
        if (!byScript)
        {
            m_StarUsage.OnStarChanged();
        }
    }

    public void OnScroll()
    {
        foreach (Button star in m_Stars)
        {
            star.OnDeselect(null);
        }
    }
}
