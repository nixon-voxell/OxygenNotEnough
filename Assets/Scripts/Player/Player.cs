using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private PlayerMovement m_PlayerMovement;

    [SerializeField] private float m_MaxOxygen = 100f;
    [SerializeField] private float m_CurrOxygen;
    [SerializeField] private float m_DamagePerSecond;
    [SerializeField] private int m_CurrHelium;

    public float CurrHealth => this.m_CurrOxygen;
    public int currHelium_num;
    public GameOverMenu gameOver;
    private void Start()
    {
        m_CurrHelium = 0;
        currHelium_num = 0;
        m_CurrOxygen = m_MaxOxygen;
        UIManager.Instance.OxygenUI.SetMaxO2(m_MaxOxygen);
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
            this.currHelium_num ++;
            this.RemoveHelium();
        }
        else         
            this.currHelium_num = 0;

        SoundEffect.Instance.Walk(this.m_PlayerMovement.IsMoving,this.m_PlayerMovement.IsCrouching);
        SoundEffect.Instance.ReleaseOxygen(this.m_CurrOxygen,this.m_PlayerMovement.IsMoving);
        GameManager.Instance.SetVignetteIntensity(1.0f - this.CurrHealth / 100.0f);
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
        this.SetHelium(Mathf.Min(this.m_CurrHelium+1, 3));
    }  
    public void RemoveHelium()
    {
        // Debug.Log("Removing");
        if(this.currHelium_num == 1)
            this.SetHelium(Mathf.Max(this.m_CurrHelium-1, 0));
    }   
    public void SetHelium(int helium)
    {
        this.m_CurrHelium = helium;
        UIManager.Instance.HeliumUI.GetHelium(helium);

    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Oxygen"))
        {
            Destroy(collider.gameObject);
            // TODO: add base on variable
            this.AddOxygen(40.0f);
            this.StartCoroutine(GameManager.Instance.OxygenSpawner.SpawnOxygenTank(1));
        } 
        if (collider.gameObject.CompareTag("Helium"))
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
