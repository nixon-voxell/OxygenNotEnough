using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private PlayerMovement m_PlayerMovement;

    [SerializeField] private float m_MaxOxygen = 100f;
    [SerializeField] private float m_CurrOxygen;
    [SerializeField] private OxygenUI m_OxygenUI;
    [SerializeField] private float m_DamagePerSecond;

    public float CurrHealth => this.m_CurrOxygen;

    private void Start()
    {
        m_CurrOxygen = m_MaxOxygen;
        m_OxygenUI.SetMaxO2(m_MaxOxygen);
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
        this.SetOxygen(this.m_CurrOxygen - oxygen);
    }

    public void AddOxygen(float oxygen)
    {
        this.SetOxygen(this.m_CurrOxygen + oxygen);
    }

    public void SetOxygen(float oxygen)
    {
        this.m_CurrOxygen = oxygen;
        this.m_OxygenUI.SetOxygen(oxygen);
    }

    // heath
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Oxygen"))
        {
            Destroy(collision.gameObject);
            // TODO: add base on variable
            this.AddOxygen(20.0f);
        }
    }
}
