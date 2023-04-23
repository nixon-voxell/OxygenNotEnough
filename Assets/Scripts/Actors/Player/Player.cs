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

    public float MaxOxygen => this.m_MaxOxygen;
    public float CurrOxygen => this.m_CurrOxygen;
    public int CurrHelium => this.m_CurrHelium;

    public void SpawnIn()
    {
        this.gameObject.SetActive(true);

        this.m_CurrOxygen = this.m_MaxOxygen;
        UIManager.Instance.OxygenUI.SetMaxOxygen(m_MaxOxygen);

        this.SetOxygen(this.m_MaxOxygen);
        this.SetHelium(0);

        GameManager.Instance.MazeGenerator.PlaceObject(this.transform, 0, 0);
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

    public void AddHelium(int helium = 1)
    {
        this.SetHelium(Mathf.Min(this.m_CurrHelium + helium, this.m_MaxHelium));
    }

    public void RemoveHelium(int helium = 1)
    {
        this.SetHelium(Mathf.Max(this.m_CurrHelium - helium, 0));
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

        if (this.m_PlayerMovement.IsUsingHelium && this.CurrHelium > 0)
        {
            this.RemoveHelium();
            // use helium...
        }

        if (this.m_CurrOxygen <=0)
        {
            GameManager.Instance.Lose();
        }

        SoundEffect.Instance.Walk(this.m_PlayerMovement.IsMoving, this.m_PlayerMovement.IsCrouching);
        SoundEffect.Instance.ReleaseOxygen(this.m_PlayerMovement.IsMoving);
        GameManager.Instance.SetVignetteIntensity(1.0f - this.CurrOxygen / this.MaxOxygen);
    }

    private void OnTriggerEnter(Collider collider)
    {
        Tank tank;
        switch (collider.tag)
        {
            case "Oxygen":
                tank = collider.GetComponent<Tank>();
                tank.SwitchLocation();
                this.AddOxygen(tank.Amount);

                break;

            case "Helium":
                if (this.CurrHelium < this.m_MaxHelium)
                {
                    tank = collider.GetComponent<Tank>();
                    tank.SwitchLocation();
                    this.AddHelium(tank.Amount);
                }

                break;

            case "Exit":
                // win!
                GameManager.Instance.Win();
                break;

            default:
                break;
        }
    }
}
