using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField] private Canvas[] m_Canvases;

    public OxygenUI OxygenUI;
    public HeliumUI HeliumUI;
    public GameOverMenu LoseUI;
    public GameWinMenu WinUI;

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
        this.WinUI.gameObject.SetActive(false);

    }

    private void Update()
    {
        GameManager gameManager = GameManager.Instance;
        for (int c = 0; c < this.m_Canvases.Length; c++)
        {
            if (gameManager.GameState == GameState.InProgress)
            {
                this.m_Canvases[c].worldCamera = gameManager.MainCamera;
            } else
            {
                this.m_Canvases[c].worldCamera = gameManager.MainMenuCamera;
            }
        }
    }
}
