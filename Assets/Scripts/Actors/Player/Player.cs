using UnityEngine;

public class Player : MonoBehaviour, IActor
{
    [SerializeField] private PlayerMovement m_PlayerMovement;

    [Header("Oxygen")]
    [SerializeField] private float m_MaxOxygen = 100.0f;
    [SerializeField] private float m_CurrOxygen;
    [SerializeField] private float m_DamagePerSecond;

    [Header("Helium")]
    [SerializeField] private int m_MaxHelium = 3;
    [SerializeField] private int m_CurrHelium;

    public float CurrHealth => this.m_CurrOxygen;
    public int CurrHelium => this.m_CurrHelium;
    public GameOverMenu gameOver;

    public void SpawnIn()
    {
        this.m_CurrOxygen = this.m_MaxOxygen;
        m_CurrHelium = 0;
        m_CurrOxygen = m_MaxOxygen;
        UIManager.Instance.OxygenUI.SetMaxO2(m_MaxOxygen);
        this.gameObject.SetActive(true);
    }

    public void SpawnOut()
    {
        this.gameObject.SetActive(false);
    }

    public void RemoveOxygen(float oxygen)
    {
        this.SetOxygen(Mathf.Max(this.m_CurrOxygen - oxygen, 0.0f));
    }

    public void AddOxygen(float oxygen)
    {
        SoundEffect.Instance.GetOxygenTank();
        this.SetOxygen(Mathf.Min(this.m_CurrOxygen + oxygen, 100.0f));
    }

    public void SetOxygen(float oxygen)
    {
        this.m_CurrOxygen = oxygen;
        UIManager.Instance.OxygenUI.SetOxygen(oxygen);
    }

    public void AddHelium()
    {
        this.SetHelium(Mathf.Min(this.m_CurrHelium + 1, this.m_MaxHelium));
    }

    public void RemoveHelium()
    {
        this.SetHelium(Mathf.Max(this.m_CurrHelium - 1, 0));
    }

    public void SetHelium(int helium)
    {
        this.m_CurrHelium = helium;
        UIManager.Instance.HeliumUI.SetHeliumNum(helium);
    }

    private void Update()
    {
        if (this.m_PlayerMovement.IsMoving)
        {
            // use time.deltatime to make sure damage is consistent
            this.RemoveOxygen(this.m_DamagePerSecond * Time.deltaTime);
        }
        if(this.m_PlayerMovement.IsUsingHelium)
        {
            this.m_CurrHelium ++;
            this.RemoveHelium();
        }
        else         
            this.m_CurrHelium = 0;

        SoundEffect.Instance.Walk(this.m_PlayerMovement.IsMoving,this.m_PlayerMovement.IsCrouching);
        SoundEffect.Instance.ReleaseOxygen(this.m_CurrOxygen,this.m_PlayerMovement.IsMoving);
        GameManager.Instance.SetVignetteIntensity(1.0f - this.CurrHealth / 100.0f);
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Oxygen"))
        {
            Destroy(collider.gameObject);
            // TODO: add base on variable
            this.AddOxygen(40.0f);
            // TODO: animate out & animate in in another place
        } else if (collider.gameObject.CompareTag("Exit"))
        {
           // TODO: win game
        } else if (collider.gameObject.CompareTag("Helium"))
        {
            Destroy(collider.gameObject);
            // TODO: add base on variable
            AddHelium();
            this.StartCoroutine(GameManager.Instance.HeliumSpawner.SpawnHeliumTank(1));
        }
        
        else if (collider.gameObject.CompareTag("Exit"))
        {
            Debug.Log("Win");
            SoundEffect.Instance.Win();
            // GameManager.Instance.GameState = GameState.Win;
        }
    }
}
