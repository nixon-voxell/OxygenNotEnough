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
    }

    private void Update()
    {
        if (this.m_PlayerMovement.IsMoving)
        {
            // use time.deltatime to make sure damage is consistent
            this.RemoveOxygen(this.m_DamagePerSecond * Time.deltaTime);
        }
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

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Oxygen"))
        {
            Destroy(collider.gameObject);
            // TODO: add base on variable
            this.AddOxygen(40.0f);
            this.StartCoroutine(GameManager.Instance.OxygenSpawner.SpawnOxygenTank(1));
        } else if (collider.gameObject.CompareTag("Exit"))
        {
            Debug.Log("Win");
            SoundEffect.Instance.Win();
            // GameManager.Instance.GameState = GameState.Win;
        }
    }
}
