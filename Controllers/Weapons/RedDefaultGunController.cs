using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedDefaultGunController : EnemyMissile
{
	public PlayerHealth playerHealth;
	private RedMobController parent;
    public static float damage = 4f;
    public static float speed = 40f;
	Renderer rend;
    private float distToBlackHole;
	public static float enemyVampirism;

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
			if (enemyVampirism > 0)
			{
				if (parent)
				{
					parent.currHealth += damage * enemyVampirism;
					if (parent.currHealth > parent.maxHealth)
						parent.currHealth = parent.maxHealth;
				}
				else
					Debug.Log("Parentless red bullet -> No vampirism benefit");
			}
		}
	}

    private void Update()
    {
        
    }

	public void setParent(RedMobController redMobController)
	{
		parent = redMobController;
	}
}
