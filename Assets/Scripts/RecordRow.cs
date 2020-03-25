using UnityEngine;
using UnityEngine.UI;

public class RecordRow : MonoBehaviour
{
    public GridLayoutGroup m_ParentGridLayoutGroup;
    public GridLayoutGroup m_GridLayoutGroup;

    public GameObject m_ModifyButton;
    public Text m_DateTime;
    public Text[] m_Stars;

    private Transform m_Transform;

    void Start()
    {
        m_Transform = transform;
        RefreshColumns();
    }

    public void RefreshColumns()
    {
        Vector2 cellSize = m_ParentGridLayoutGroup.cellSize;
        cellSize.x = Mathf.FloorToInt(m_GridLayoutGroup.cellSize.x * m_Transform.childCount);
        m_ParentGridLayoutGroup.cellSize = cellSize;
    }

    public void RefreshUI()
    {

    }
}
