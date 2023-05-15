using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollideSlash : MonoBehaviour
{
	private PlayerHealth health;
	public static float enemyVampirism;
	private Boss04Controller parent;
	private float damage = 10f;

	void Start ()
	{
		health = FindObjectOfType<PlayerHealth>();
	}

	private void OnParticleCollision(GameObject other)
	{
		health.takeDamage(damage);
		if(enemyVampirism > 0)
		{
			parent.currHealth += damage * enemyVampirism;
			if (parent.currHealth > parent.maxHealth)
				parent.currHealth = parent.maxHealth;

		}
	}
}
