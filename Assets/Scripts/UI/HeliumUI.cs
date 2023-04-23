using UnityEngine;
using UnityEngine.UI;

public class HeliumUI : MonoBehaviour
{
    [SerializeField] private Image[] m_HeliumImages;
    [SerializeField] private Color m_BlankColor;
    [SerializeField] private Color m_OccupiedColor;

    public void SetHeliumNum(int heliumNum)
    {
        for (int h = 0; h < this.m_HeliumImages.Length; h++)
        {
            Image image = this.m_HeliumImages[h];
            image.color = this.m_BlankColor;
        }

        heliumNum = Mathf.Min(this.m_HeliumImages.Length, heliumNum);

        for (int h = 0; h < heliumNum; h++)
        {
            Image image = this.m_HeliumImages[h];
            image.color = this.m_OccupiedColor;
        }
    }

    private void Start()
    {
        UIManager.Instance.HeliumUI = this;
    }
}
