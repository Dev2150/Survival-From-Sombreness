using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigShockController : MonoBehaviour {

	public GameObject o_mediumShockController;
	private float bonusDamage = 0f;
	private float speed = 15f;
	public float BonusSpeed = 0f;
	private float currentSpeed;
	private bool hasMadeContactWithTerrain;

	private GameObject[] o_blackHole;
	private float distToBlackHole, f_distance, f_range = 15f, f_maxDamage = 30f;
	private GameObject reds, blues, greens, blacks, oranges;
	private PlayerController player;

	void Start () {
		o_blackHole = new GameObject[5];
		for (int i = 0; i < 5; i++)
			o_blackHole[i] = GameObject.Find("BH" + i);

		reds = GameObject.Find("Red");
		blues = GameObject.Find("Blue");
		greens = GameObject.Find("Green");
		blacks = GameObject.Find("Black");
		oranges = GameObject.Find("Orange");

		player = FindObjectOfType<PlayerController>();
		GetComponent<Rigidbody>().velocity = transform.forward * speed * (1 + player.BonusBulletspeed / 10);
	}

	private void Update()
	{
		for (int i = 0; i < 5; i++)
		{
			distToBlackHole = Vector3.Distance(o_blackHole[i].transform.position, transform.position);
			if (distToBlackHole < 250)
			{
				transform.Translate(10000f * (o_blackHole[i].transform.position - transform.position) / (distToBlackHole * distToBlackHole) * Time.deltaTime);
				if (distToBlackHole < 30)
					Destroy(this);
			}
		}
	}

	void OnTriggerEnter (Collider collision) {

		if (hasMadeContactWithTerrain)
			return;
		
		GameObject o = null;
		GetComponent<AudioSource>().Play();

		if (collision.gameObject.tag == "Terrain" || collision.gameObject.tag == "Enemy") {
			hasMadeContactWithTerrain = true;
			o = Instantiate (o_mediumShockController, transform.position + Vector3.up, Quaternion.Inverse(transform.rotation));
			o.GetComponent<Rigidbody>().velocity = (transform.forward + 0.1f * Vector3.up) * speed * (1 + player.BonusBulletspeed / 10);

			o = Instantiate (o_mediumShockController, transform.position + Vector3.up, Quaternion.Inverse (transform.rotation));// * Quaternion.Euler(-120, 0, -120));
			o.transform.Rotate (0, 120, 0);
			o.GetComponent<Rigidbody>().velocity = (-0.5f * transform.forward + 0.2f * Vector3.up + 0.75f * transform.right) * speed * (1 + player.BonusBulletspeed / 10);

			o = Instantiate (o_mediumShockController, transform.position + Vector3.up, Quaternion.Inverse (transform.rotation));// * Quaternion.Euler(120, 0, 120));
			o.transform.Rotate (0, -120, 0);
			o.GetComponent<Rigidbody>().velocity = (-0.5f * transform.forward + 0.2f * Vector3.up - 0.75f * transform.right) * speed * (1 + player.BonusBulletspeed / 10);

			f_distance = Vector2.Distance(transform.position, player.transform.position);
			if (f_distance < f_range)
			{
				float f_damageTaken = Mathf.Min(f_maxDamage, (f_maxDamage) / f_distance);
				FindObjectOfType<PlayerHealth>().takeDamage(f_damageTaken);
			}

			Transform t_current;
			int i;
			for (i = 0; i < reds.transform.childCount; i++)
			{
				t_current = reds.transform.GetChild(i);
				f_distance = Vector2.Distance(transform.position, t_current.position);
				if (f_distance < f_range)
				{
					t_current.GetComponent<RedMobController>().damage(Mathf.Min(f_maxDamage, 10 * (1 + bonusDamage * 0.1f) * f_maxDamage / f_distance ) );
				}
			}
			for (i = 0; i < blues.transform.childCount; i++)
			{
				t_current = blues.transform.GetChild(i);
				f_distance = Vector2.Distance(transform.position, t_current.position);
				if (f_distance < f_range)
				{
					t_current.GetComponent<BlueMobController>().damage(Mathf.Min(f_maxDamage, 10 * (1 + bonusDamage * 0.1f) * f_maxDamage / f_distance ) );
				}
			}
			for (i = 0; i < greens.transform.childCount; i++)
			{
				t_current = greens.transform.GetChild(i);
				f_distance = Vector2.Distance(transform.position, t_current.position);
				if (f_distance < f_range)
				{
					t_current.GetComponent<GreenMobController>().damage(Mathf.Min(f_maxDamage, 10 * (1 + bonusDamage * 0.1f) * f_maxDamage / f_distance ) );
				}
			}
			for (i = 0; i < blacks.transform.childCount; i++)
			{
				t_current = blacks.transform.GetChild(i);
				f_distance = Vector2.Distance(transform.position, t_current.position);
				if (f_distance < f_range)
				{
					t_current.GetComponent<BlackMobController>().damage(Mathf.Min(f_maxDamage, 10 * (1 + bonusDamage * 0.1f) * f_maxDamage / f_distance ) );
				}
			}
			for (i = 0; i < oranges.transform.childCount; i++)
			{
				t_current = oranges.transform.GetChild(i);
				f_distance = Vector2.Distance(transform.position, t_current.position);
				if (f_distance < f_range)
				{
					t_current.GetComponent<OrangeMobController>().damage(Mathf.Min(f_maxDamage, 2.5f * (1 + bonusDamage * 0.1f) * f_maxDamage / f_distance ) );
				}
			}
			
			try
			{
				t_current = FindObjectOfType<Boss01Controller>().transform;
				f_distance = Vector2.Distance(transform.position, t_current.position);
				if (f_distance < f_range)
				{
					t_current.gameObject.GetComponent<Boss01Controller>().damage(Mathf.Min(f_maxDamage, 10 * (1 + bonusDamage * 0.1f) * f_maxDamage / f_distance ) );
				}
			}
			catch (System.Exception ex) {}

			try
			{
				t_current = FindObjectOfType<Boss02Controller>().transform;
				f_distance = Vector2.Distance(transform.position, t_current.position);
				if (f_distance < f_range)
				{
					t_current.gameObject.GetComponent<Boss02Controller>().damage(Mathf.Min(f_maxDamage, 10 * (1 + bonusDamage * 0.1f) * f_maxDamage / f_distance ) );
				}
			}
			catch (System.Exception ex) {}

			try
			{
				t_current = FindObjectOfType<Boss03Controller>().transform;
				f_distance = Vector2.Distance(transform.position, t_current.position);
				if (f_distance < f_range)
				{
					t_current.gameObject.GetComponent<Boss03Controller>().damage(Mathf.Min(f_maxDamage, 10 * (1 + bonusDamage * 0.1f) * f_maxDamage / f_distance ) );
				}
			}
			catch (System.Exception ex) {}
			
			try
			{
				t_current = FindObjectOfType<Boss04Controller>().transform;
				f_distance = Vector2.Distance(transform.position, t_current.position);
				if (f_distance < f_range)
				{
					t_current.gameObject.GetComponent<Boss04Controller>().damage(Mathf.Min(f_maxDamage, 10 * (1 + bonusDamage * 0.1f) * f_maxDamage / f_distance ) );
				}
			}
			catch (System.Exception ex) {}
			
			Destroy (this, 2f);
		}
	}
}
