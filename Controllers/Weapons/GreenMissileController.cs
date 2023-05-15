using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenMissileController : EnemyMissile {

	public float f_time;

	private const float damage = 5f;
	public static float speed = 200f;
	Renderer rend;
	Transform t_player;
	PlayerHealth playerHealth;
	public bool isGoingToPlayer, isLaunching;
	PlayerController player;
	Vector3 v3_speed;

    private float distToBlackHole;
    private GameObject[] o_blackHole;
	public static float SpeedUp;
	public static bool Debuff = false;

	private void Start()
	{
		isGoingToPlayer = false;
		isLaunching = false;
		f_time = -1;

		t_player = FindObjectOfType<PlayerController>().transform;
		
        o_blackHole = new GameObject[5];
        for (var i = 0; i < 5; i++)
            o_blackHole[i] = GameObject.Find("BH" + i);
    }

	public void Attack() {
		f_time = 0;
		isGoingToPlayer = false;
		isLaunching = false;
	}

	protected void OnTriggerEnter (Collider col)
	{
		if (col.gameObject.CompareTag("Player"))
		{
			playerHealth = FindObjectOfType<PlayerHealth> ();
			playerHealth.takeDamage(damage);
			player.AddToxicity();
			if(Debuff)
				PowerCube.createNegativePower(player, FindObjectOfType<PlayerHealth>(), FindObjectOfType<PowerCube>());
		}
	}

	// Update is called once per frame
	void Update () {
		if (f_time < 0) return;
		
		for (int j = 0; j < 5; j++)
		{
			distToBlackHole = Vector3.Distance(o_blackHole[j].transform.position, transform.position);
			if (distToBlackHole < 250)
			{
				transform.Translate(1000f * (o_blackHole[j].transform.position - transform.position) /
				                    (distToBlackHole * distToBlackHole) * Time.deltaTime);
				if (distToBlackHole < 10)
					Destroy(this);
			}
		}

		if (!isLaunching && f_time == 0)
		{
			GetComponent<AudioSource>().Play();
			isLaunching = true;
			GetComponent<Rigidbody>().velocity = Vector3.up * SpeedUp;
		}
		else if (isLaunching && !isGoingToPlayer && f_time > 3)
		{
			player = FindObjectOfType<PlayerController> ();
			playerHealth = FindObjectOfType<PlayerHealth> ();
			isGoingToPlayer = true;
			//rb.AddForce (-Vector3.up * 2000f ,ForceMode.Force);
			transform.LookAt(t_player, Vector3.up);
			GetComponent<Rigidbody>().velocity = (-transform.position + t_player.position).normalized * speed;
			//transform.LookAt (GameObject.FindObjectOfType<PlayerController> ().transform, Vector3.up);
			//v3_acceleration = Vector3.forward * speed * Time.deltaTime;
		}
		else if (f_time > 5)
		{
			transform.position = Vector3.down * 9999;
			f_time = -1;
		}

		f_time += Time.deltaTime;
	}
		
}
