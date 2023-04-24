using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField] private Canvas[] m_Canvases;

    public OxygenUI OxygenUI;
    public HeliumUI HeliumUI;
    public GameOverMenu LoseUI;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        } else
        {
            Debug.LogError("There is probably more than one instance.", Instance);
            Object.Destroy(this);
        }

        this.OxygenUI.gameObject.SetActive(false);
        this.HeliumUI.gameObject.SetActive(false);
        this.LoseUI.gameObject.SetActive(false);
    }

    private void Start()
    {
        for (int c = 0; c < this.m_Canvases.Length; c++)
        {
            this.m_Canvases[c].worldCamera = GameManager.Instance.MainCamera;
        }
    }
}
