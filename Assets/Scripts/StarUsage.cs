using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class StarUsage : MonoBehaviour
{
    public StarRow m_StarRow;

    private Transform m_Transform;
    private GameObject m_StarRowTemplate;

    private StringBuilder m_StringBuilder = new StringBuilder();

    private StarRow[] m_StarRows;

    void Start()
    {
        m_StarRow.TemplateInit();
        m_Transform = transform;
        m_StarRowTemplate = m_StarRow.gameObject;

        RefreshUI();
    }

    void RefreshUI()
    {
        int starUsageCount = CustomStarUsage.m_Instance.m_StarUsageCount;
        m_StarRows = new StarRow[starUsageCount];
        GameObject[] starRowGameObjects = new GameObject[starUsageCount];
        starRowGameObjects[0] = m_StarRowTemplate;
        for (int i = 1; i < starUsageCount; i++)
        {
            starRowGameObjects[i] = Instantiate(starRowGameObjects[0]);
            m_StringBuilder.Append("Star");
            m_StringBuilder.Append(i);
            starRowGameObjects[i].name = m_StringBuilder.ToString();
            m_StringBuilder.Clear();
            starRowGameObjects[i].transform.SetParent(m_Transform);
            starRowGameObjects[i].transform.localScale = Vector3.one;
        }
        for (int i = 0; i < starUsageCount; i++)
        {
            GameObject starRowGameObject = starRowGameObjects[i];
            StarRow starRow = starRowGameObject.GetComponent<StarRow>();
            starRow.m_Usage.text = CustomStarUsage.m_Instance.GetUsage(i);
            int starMaxCount = CustomStarUsage.m_Instance.GetStarMaxCount(i);
            Button templateStarButton = starRow.m_Stars[0];
            starRow.m_Stars = new Button[starMaxCount];
            starRow.m_Stars[0] = templateStarButton;
            for (int j = 1; j < starMaxCount; j++)
            {
                GameObject starText = Instantiate(starRow.m_Stars[0].gameObject);
                m_StringBuilder.Append("Button");
                m_StringBuilder.Append(j);
                starText.name = m_StringBuilder.ToString();
                m_StringBuilder.Clear();
                starText.transform.SetParent(starRow.m_Stars[0].transform.parent);
                starText.transform.localScale = Vector3.one;
                starRow.m_Stars[j] = starText.GetComponent<Button>();
            }
            m_StarRows[i] = starRow;
        }
    }

    public void RefreshDay(DayInfo di)
    {
        int iMax = di.starsCount;
        for (int i = 0; i < iMax; i++)
        {
            byte starCount = di.GetStarCount(i);
            if (starCount == 0)
            {
                m_StarRows[i].OnStarClick(null);
            }
            else
            {
                m_StarRows[i].OnStarClick(m_StarRows[i].m_Stars[starCount - 1]);
            }
        }
    }

    public int starRowCount
    {
        get
        {
            return m_StarRows.Length;
        }
    }

    public StarRow GetStarRow(int index)
    {
        return m_StarRows[index];
    }
}
