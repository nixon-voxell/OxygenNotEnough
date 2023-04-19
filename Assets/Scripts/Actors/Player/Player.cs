using UnityEngine;

public class Player : MonoBehaviour, IActor
{
    [SerializeField] private PlayerMovement m_PlayerMovement;

    [SerializeField] private float m_MaxOxygen = 100f;
    [SerializeField] private float m_CurrOxygen;
    [SerializeField] private float m_DamagePerSecond;
    public float CurrHealth => this.m_CurrOxygen;

    public GameOverMenu gameOver;

    public void SpawnIn()
    {
        this.m_CurrOxygen = this.m_MaxOxygen;
        UIManager.Instance.OxygenUI.SetMaxO2(m_MaxOxygen);
        this.gameObject.SetActive(true);
    }

    public void SpawnOut()
    {
        this.gameObject.SetActive(false);
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
        this.SetOxygen(Mathf.Max(this.m_CurrOxygen - oxygen, 0.0f));
    }

    public void AddOxygen(float oxygen)
    {
        this.SetOxygen(Mathf.Min(this.m_CurrOxygen + oxygen, 100.0f));
    }

    public void SetOxygen(float oxygen)
    {
        this.m_CurrOxygen = oxygen;
        UIManager.Instance.OxygenUI.SetOxygen(oxygen);
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Oxygen"))
        {
            Destroy(collider.gameObject);
            // TODO: add base on variable
            this.AddOxygen(40.0f);
            // TODO: animate out & animate in in another place
            // this.StartCoroutine(GameManager.Instance.OxygenSpawner.SpawnOxygenTanks(1));
        } else if (collider.gameObject.CompareTag("Exit"))
        {
            // GameManager.Instance.GameState = GameState.Win;
        }
    }
}
