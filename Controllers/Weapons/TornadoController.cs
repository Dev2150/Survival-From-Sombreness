using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TornadoController : MonoBehaviour {

	public static float damage = 10f;
	public PlayerHealth playerHealth;
	public static float enemyVampirism;

	protected void OnTriggerEnter (Collider col)
	{
		if (col.gameObject.CompareTag("Player"))
		{
			playerHealth.takeDamage(damage);
			if(enemyVampirism > 0)
				playerHealth.GetHP(damage);
		}
	}
}
