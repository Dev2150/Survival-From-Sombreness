using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedSplashController : EnemyMissile
{
	public PlayerHealth playerHealth;
    public static float damage = 4f;
    public static float speed = 30f;
	Renderer rend;
    private float distToBlackHole;

    void Start()
    {
        GetComponent<Rigidbody>().velocity = transform.forward * speed;
	    if(!playerHealth)
			playerHealth = FindObjectOfType<PlayerHealth>();
        if (PlayerHealth.isDead) 
			speed = Mathf.Lerp(speed, 0, 0.1f);
    }

	protected void OnTriggerEnter (Collider col)
	{
		if (col.gameObject.CompareTag("Player"))
		{
			playerHealth.takeDamage(damage);
			// Destroy(gameObject);
		}
	}

    private void Update()
    {
        
    }
}
