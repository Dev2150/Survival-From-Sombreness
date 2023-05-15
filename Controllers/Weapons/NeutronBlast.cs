using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeutronBlast : EnemyMissile
{
	private bool approached = false;

	public static float damage = 50f;
	public static float speed = 50f;

	private PlayerController player;
	private PlayerHealth playerHealth;
	public static float enemyVampirism;

	void Start()
	{
		player = FindObjectOfType<PlayerController> ();
		//transform.rotation *= qdr;
		if (PlayerHealth.isDead) 
			speed = Mathf.Lerp(speed, 0, 0.1f);


	}


	protected void OnTriggerEnter (Collider col)
	{
		if (col.gameObject.CompareTag("Player"))
		{
			if(playerHealth = FindObjectOfType<PlayerHealth> ())
				playerHealth.takeDamage(damage);
			// Destroy(gameObject);
		}
	}

	private void Update()
	{
		// TODO: Play Neutron Blast Sound
	}
}
