using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private Rigidbody m_rb;
    [SerializeField] private float m_speed = 5f;
    private float crouchSpeed = 2f;
    [SerializeField] private float m_PercentageDamage = 0.05f;
    [Header("Status")]
    private bool isCrouch=false;
    [Header("Health")]
    // oxygen part
    public float maxHealth = 100f;
	public float currentHealth;
	public Oxygen HealthBar;
    void Start()
    {
        m_rb = GetComponent<Rigidbody>();
        
        currentHealth = maxHealth;
        HealthBar.SetMaxO2(maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        bool isMoving = Mathf.Abs(horizontalInput) > 0.0f || Mathf.Abs(verticalInput) > 0.0f;

        // if(Input.GetButton("Jump"))
        // {
        //     m_rb.velocity = new Vector3(m_rb.velocity.x, 5f, m_rb.velocity.z);
        // }
        
        if(Input.GetButton("Crouch"))
        {
            Crouch();

        } else if(!Input.GetButton("Crouch") && isCrouch)
        {
            StandUp();
        }

        m_rb.velocity = new Vector3(horizontalInput*m_speed, m_rb.velocity.y, verticalInput*m_speed);
        if (isMoving)
        {
            TakeDamage(m_PercentageDamage * Time.deltaTime);
        }

        GameManager.Instance.SetVignetteIntensity(1.0f - currentHealth / 100.0f);
    }

    public void TakeDamage(float damage)
	{
		currentHealth -= damage;
		HealthBar.SetHealth(currentHealth);
	}
    public void AddHealth(float health)
	{
		currentHealth += health;
		HealthBar.SetHealth(currentHealth);
	}

    // heath
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Oxygen"))
        {
            // Debug.Log("Oxygen get");
            Destroy(collision.gameObject);
            AddHealth(50f);
        }
    }
    void Crouch()
    {
        isCrouch = true;
        // percentage_Damage = 0.01f;
        m_speed = crouchSpeed;

    }
    void StandUp()
    {
        isCrouch = false;
        // percentage_Damage = 0.05f;
        m_speed = 5f;
    }
    
}
