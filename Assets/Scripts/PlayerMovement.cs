using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerMovement : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody rb;
    float speed = 5f;
    
    // oxygen part
    public int maxHealth = 100;
	public int currentHealth;
	public Oxygen healthBar;
    public float timeRemaining = 100;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        
        currentHealth = maxHealth;
		healthBar.SetMaxO2(maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        rb.velocity = new Vector3(horizontalInput*5f,rb.velocity.y,verticalInput*speed);

        if(Input.GetButton("Jump")){
            rb.velocity = new Vector3(rb.velocity.x,5f,rb.velocity.z);
        }
        
        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            TakeDamage(20);
        }
    }
    
    void TakeDamage(int damage)
	{
		currentHealth -= damage;
		healthBar.SetHealth(currentHealth);
	}
}
