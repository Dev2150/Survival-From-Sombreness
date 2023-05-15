using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss02Cluster : EnemyMissile
{
	private float f_speed = 5f;
	public GameObject g_explosionP;
	private float speed_initial = 5f;
	public static float enemyVampirism;
	private Boss02Controller parent;

	void Update()
	{
		transform.Translate(Vector3.forward * f_speed * Time.deltaTime, Space.Self);
		f_speed += 15 * Time.deltaTime;
	}

	void Start()
	{
		parent = FindObjectOfType<Boss02Controller>();
	}

	void OnTriggerEnter(Collider collision)
	{
		if (!DifficultySystem.nuke_explodable)
			return;

		if (collision.gameObject.tag == "Terrain")
		{
			Destroy(Instantiate(g_explosionP, transform.position, transform.rotation), 2f);


			f_speed = speed_initial;
			//Destroy (this, 0f);
			float f_distance = Vector2.Distance(transform.position, FindObjectOfType<PlayerController>().transform.position);
			float f_range = 30;
			float f_maxDamage = 33;
			if (f_distance < f_range)
			{
				float f_damageTaken = Mathf.Min(f_maxDamage, f_maxDamage / f_distance);
				// Debug.Log ("Dist: " + f_distance + "; Damage: " + f_damageTaken);
				FindObjectOfType<PlayerHealth>().takeDamage(f_damageTaken);
				if (enemyVampirism > 0)
				{
					parent.currHealth += f_damageTaken * enemyVampirism;
					if (parent.currHealth > parent.maxHealth)
						parent.currHealth = parent.maxHealth;
				}

				transform.position = Vector3.down * 1000;

				int i = Random.Range(0, 3);
				GameObject.Find("XS" + i).GetComponent<AudioSource>().Play();
				GameObject.Find("XS" + i).GetComponent<AudioSource>().volume = Mathf.Max(1, 10 / f_distance);

			}
		}
	}
}
