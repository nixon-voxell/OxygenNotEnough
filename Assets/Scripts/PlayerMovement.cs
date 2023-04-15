using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerMovement : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody rb;
    float speed = 5f;
    
    // oxygen part
    public float maxHealth = 100f;
	public float currentHealth;
	public Oxygen healthBar;

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
        
        TakeDamage(Mathf.Abs(horizontalInput)*0.05f);
        
        if(Input.GetButton("Jump")){
            rb.velocity = new Vector3(rb.velocity.x,5f,rb.velocity.z);
        }
        
    }
    public void TakeDamage(float damage)
	{
		currentHealth -= damage;
		healthBar.SetHealth(currentHealth);
	}
    public void AddHealth(float health)
	{
		currentHealth += health;
		healthBar.SetHealth(currentHealth);
	}
    // heath
    private void OnCollisionEnter(Collision collision){
        if(collision.gameObject.CompareTag("Oxygen")){
            Debug.Log("Oxygen get");
            Destroy(collision.gameObject);
            AddHealth(20f);

        }
        else{
            Debug.Log("Didnt get oxygen");
        }
    }

}
