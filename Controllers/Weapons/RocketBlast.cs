using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketBlast : MonoBehaviour {

	public static float bonusDamage = 0;
	float f_range = 40;
	public float f_maxDamage = 200;
	public GameObject g_explosionP;
	float f_distance;
	private GameObject reds, blues, greens, blacks;
	private PlayerController player;
	private PlayerHealth health;


	// Use this for initialization
	void Start () {

		reds = GameObject.Find("Red");
		blues = GameObject.Find("Blue");
		greens = GameObject.Find("Green");
		blacks = GameObject.Find("Black");

		Destroy(Instantiate(g_explosionP, transform.position, transform.rotation), 2f);
		player = FindObjectOfType<PlayerController>();
		health = FindObjectOfType<PlayerHealth>();
		f_distance = Vector2.Distance(transform.position, player.transform.position);
		if (f_distance < f_range)
		{
			float f_damageTaken = Mathf.Min(f_maxDamage, f_maxDamage / f_distance);
//			Debug.Log ("Dist: " + f_distance + "; Damage: " + f_damageTaken);
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
				var min = Mathf.Min(f_maxDamage, 20 * (1 + bonusDamage * 0.1f) * f_maxDamage / f_distance );
				t_current.GetComponent<RedMobController>().damage(min );
				if(player.lifeStealCoef > 0)
					health.GetHP(min * player.lifeStealCoef);
				Debug.Log(f_distance + " -> " + Mathf.Min(f_maxDamage, f_maxDamage / f_distance));
			}
		}
		for (i = 0; i < blues.transform.childCount; i++)
		{
			t_current = blues.transform.GetChild(i);
			f_distance = Vector2.Distance(transform.position, t_current.position);
			if (f_distance < f_range)
			{
				var min = Mathf.Min(f_maxDamage, 20 * (1 + bonusDamage * 0.1f) * f_maxDamage / f_distance );
				t_current.GetComponent<BlueMobController>().damage(min );
				if(player.lifeStealCoef >0)
					health.GetHP(min * player.lifeStealCoef);
				Debug.Log(f_distance + " -> " + Mathf.Min(f_maxDamage, f_maxDamage / f_distance));
			}
		}
		for (i = 0; i < greens.transform.childCount; i++)
		{
			t_current = greens.transform.GetChild(i);
			f_distance = Vector2.Distance(transform.position, t_current.position);
			if (f_distance < f_range)
			{
				var min = Mathf.Min(f_maxDamage, 20 * (1 + bonusDamage * 0.1f) * f_maxDamage / f_distance );
				t_current.GetComponent<GreenMobController>().damage(min );
				if(player.lifeStealCoef >0)
					health.GetHP(min * player.lifeStealCoef );
				Debug.Log(f_distance + " -> " + Mathf.Min(f_maxDamage, f_maxDamage / f_distance));
			}
		}
		for (i = 0; i < blacks.transform.childCount; i++)
		{
			t_current = blacks.transform.GetChild(i);
			f_distance = Vector2.Distance(transform.position, t_current.position);
			if (f_distance < f_range)
			{
				var min = Mathf.Min(f_maxDamage, 20 * (1 + bonusDamage * 0.1f) * f_maxDamage / f_distance );
				t_current.GetComponent<BlackMobController>().damage(min );
				if(player.lifeStealCoef >0)
					health.GetHP(min * player.lifeStealCoef );
				Debug.Log(f_distance + " -> " + Mathf.Min(f_maxDamage, f_maxDamage / f_distance));
			}
		}

		try
		{
			t_current = FindObjectOfType<Boss01Controller>().transform;
			f_distance = Vector2.Distance(transform.position, t_current.position);
			if (f_distance < f_range)
			{
				var min = Mathf.Min(f_maxDamage, 20 * (1 + bonusDamage * 0.1f) * f_maxDamage / f_distance );
				t_current.GetComponent<Boss01Controller>().damage(min );
				if(player.lifeStealCoef >0)
					health.GetHP(min * player.lifeStealCoef );
				Debug.Log(f_distance + " -> " + Mathf.Min(f_maxDamage, f_maxDamage / f_distance));
			}
		}
		catch (System.Exception ex) {}

		try
		{
			t_current = FindObjectOfType<Boss02Controller>().transform;
			f_distance = Vector2.Distance(transform.position, t_current.position);
			if (f_distance < f_range)
			{
				var min = Mathf.Min(f_maxDamage, 20 * (1 + bonusDamage * 0.1f) * f_maxDamage / f_distance );
				t_current.GetComponent<Boss02Controller>().damage(min );
				if(player.lifeStealCoef >0)
					health.GetHP(min * player.lifeStealCoef );
				Debug.Log(f_distance + " -> " + Mathf.Min(f_maxDamage, f_maxDamage / f_distance));
			}
		}
		catch (System.Exception ex) {}

		try
		{
			t_current = FindObjectOfType<Boss03Controller>().transform;
			f_distance = Vector2.Distance(transform.position, t_current.position);
			if (f_distance < f_range)
			{
				var min = Mathf.Min(f_maxDamage, 20 * (1 + bonusDamage * 0.1f) * f_maxDamage / f_distance );
				t_current.GetComponent<Boss03Controller>().damage(min );
				if(player.lifeStealCoef >0)
					health.GetHP(min * player.lifeStealCoef );
				Debug.Log(f_distance + " -> " + Mathf.Min(f_maxDamage, f_maxDamage / f_distance));
			}
		}
		catch (System.Exception ex) {}

		try
		{
			t_current = FindObjectOfType<Boss04Controller>().transform;
			f_distance = Vector2.Distance(transform.position, t_current.position);
			if (f_distance < f_range)
			{
				var min = Mathf.Min(f_maxDamage, 20 * (1 + bonusDamage * 0.1f) * f_maxDamage / f_distance );
				t_current.GetComponent<Boss04Controller>().damage(min );
				if(player.lifeStealCoef >0)
					health.GetHP(min * player.lifeStealCoef );
				Debug.Log(f_distance + " -> " + Mathf.Min(f_maxDamage, f_maxDamage / f_distance));
			}
		}
		catch (System.Exception ex) {}

		transform.position = Vector3.down * 1000;

		int j = Random.Range(0, 3);
		GameObject.Find("XS" + j).GetComponent<AudioSource>().Play();
		GameObject.Find("XS" + j).GetComponent<AudioSource>().volume = Mathf.Max(1, 10 / f_distance);
	}
}
