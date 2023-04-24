using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour, IActor
{
    [SerializeField] private PlayerMovement m_PlayerMovement;

    [Header("Enemy Detection Radius")]
    [SerializeField] private MeshRenderer m_DetectionRenderer;
    [SerializeField] private float m_EnemyDetectRadius;
    [SerializeField] private float m_VizRadiusStart;
    [SerializeField] private float m_VizRadiusEnd;

    [Header("Oxygen")]
    [SerializeField] private float m_MaxOxygen = 100.0f;
    [SerializeField] private float m_CurrOxygen;
    [SerializeField] private float m_DamagePerSecond;

    [Header("Helium")]
    [SerializeField] private Helium m_HeliumPrefab;
    [SerializeField] private int m_MaxHelium = 3;
    [SerializeField] private int m_CurrHelium;

    private Material m_mat_DetectionRadius;
    private float m_DetectPercentage;

    public float MaxOxygen => this.m_MaxOxygen;
    public float CurrOxygen => this.m_CurrOxygen;
    public int CurrHelium => this.m_CurrHelium;
    
    public void SpawnIn()
    {
        this.gameObject.SetActive(true);
        this.m_DetectPercentage = 0.0f;

        this.m_CurrOxygen = this.m_MaxOxygen;
        UIManager.Instance.OxygenUI.SetMaxOxygen(m_MaxOxygen);

        this.SetOxygen(this.m_MaxOxygen);
        this.SetHelium(0);

        GameManager.Instance.MazeGenerator.PlaceObject(this.transform, 0, 0);

        // deparent object
        Vector3 playerScale = this.transform.localScale;
        this.m_DetectionRenderer.transform.localScale = new Vector3(
            this.m_EnemyDetectRadius * 2.0f / playerScale.x,
            this.m_EnemyDetectRadius * 2.0f / playerScale.y,
            this.m_EnemyDetectRadius * 2.0f / playerScale.z
        );
        this.m_mat_DetectionRadius = this.m_DetectionRenderer.sharedMaterial;
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

    private void CheckForDestructibles()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 0.2f);
        foreach(Collider c in colliders)
        {
            if (c.CompareTag("Enemy"))
            {
                this.RemoveOxygen(this.m_DamagePerSecond * Time.deltaTime);
            }
        }
    }

    private void Update()
    {
        if (this.m_PlayerMovement.IsMoving)
        {
            // use time.deltatime to make sure damage is consistent
            this.RemoveOxygen(this.m_DamagePerSecond * Time.deltaTime);
        }

        // if walking, display detection renderer, else reduce it
        float targetDetectPercentage = this.m_PlayerMovement.IsCrouching ? 0.0f : 1.0f;
        this.m_DetectPercentage = Mathf.Lerp(this.m_DetectPercentage, targetDetectPercentage, Time.deltaTime * 2.5f);

        this.m_mat_DetectionRadius.SetFloat(
            "_Radius",
            Mathf.Lerp(this.m_VizRadiusStart, this.m_VizRadiusEnd, this.m_DetectPercentage)
        );

        if (!this.m_PlayerMovement.IsCrouching)
        {
            Collider[] colliders = Physics.OverlapSphere(
                this.transform.position,
                Mathf.Lerp(0.0f, this.m_EnemyDetectRadius, this.m_DetectPercentage)
            );
            for (int c = 0; c < colliders.Length; c++)
            {
                Collider collider = colliders[c];
                if (collider.CompareTag("Enemy"))
                {
                    Enemy enemy = collider.GetComponent<Enemy>();
                    enemy.PursuitPlayer();
                }
            }
        }

        if (this.m_PlayerMovement.IsUsingHelium && this.CurrHelium > 0)
        {
            this.RemoveHelium();
            // use helium...
            Helium helium = Object.Instantiate(this.m_HeliumPrefab, this.transform.position, Quaternion.identity);
            helium.SpawnIn();
            SceneManager.MoveGameObjectToScene(helium.gameObject, this.gameObject.scene);
        }

        if (this.m_CurrOxygen <= 0)
        {
            GameManager.Instance.Lose();
            this.m_CurrOxygen = 100.0f;
        }

        this.CheckForDestructibles();

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
