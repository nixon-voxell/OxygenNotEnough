using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private PlayerMovement m_PlayerMovement;

    [SerializeField] private float m_MaxOxygen = 100f;
    [SerializeField] private float m_CurrOxygen;
    [SerializeField] private float m_DamagePerSecond;
    public float CurrHealth => this.m_CurrOxygen;

    public GameOverMenu gameOver;

    private void Start()
    {
        m_CurrOxygen = m_MaxOxygen;
        UIManager.Instance.OxygenUI.SetMaxO2(m_MaxOxygen);
        GameManager.Instance.SpawnOxygen.SpawnCube(1);
    }

    private void Update()
    {
        if (this.m_PlayerMovement.IsMoving)
        {
            // use time.deltatime to make sure damage is consistent
            this.RemoveOxygen(this.m_DamagePerSecond * Time.deltaTime);
        }

        GameManager.Instance.SetVignetteIntensity(1.0f - this.CurrHealth / 100.0f);
    }

    public void RemoveOxygen(float oxygen)
    {
        if (this.m_CurrOxygen > 0.0f)
            this.SetOxygen(this.m_CurrOxygen - oxygen);
        else
            gameOver.Gameover(); 

    }

    public void AddOxygen(float oxygen)
    {
        if(this.m_CurrOxygen>100.0f)
        {
            this.m_CurrOxygen=100.0f;
            this.SetOxygen(100.0f);
        }

        else
            this.SetOxygen(this.m_CurrOxygen + oxygen);
    }

    public void SetOxygen(float oxygen)
    {
        this.m_CurrOxygen = oxygen;
        UIManager.Instance.OxygenUI.SetOxygen(oxygen);
    }

    // heath
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("Oxygen"))
        {
            // Destroy(hit.gameObject);
            // TODO: add base on variable
            this.AddOxygen(20.0f);
            GameManager.Instance.SpawnOxygen.SpawnCube(1);
        }
    }
}
