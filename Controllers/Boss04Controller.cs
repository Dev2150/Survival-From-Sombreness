using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Audio;

public class Boss04Controller : MobController {
	private float time;
	private static float attkTM;
	private float chDirT, f_chDirCD = 2.0f, f_movementType, f_warpT, f_warpCD = 0.25f;

	public GameObject explosionP;
	private Quaternion qdr;
	private Vector3 dir;

	private Vector3 v3_shiftDirection;
	private float f_currentDistance, f_attackType, f_radians;
	private float f_randomX, f_randomY;

	private AudioSource[] audioSources = new AudioSource[2];
	private int phase = 0;
    public static float damageMultiplier;
	
	public GameObject[] o_blackHole;
	public static float damagePowCoef;

	private void Awake() {
		agent = GetComponent<NavMeshAgent> ();
		NavMesh.SamplePosition(transform.position, out hit, 1000, 1);
		agent.Warp (hit.position);
		agent.speed = 30f;
		chDirT = 999;

		player = FindObjectOfType<PlayerController> ();
		playerTr = player.transform;

		currHealth = 200f;
		maxHealth = 200f;

		audioSources = GetComponents<AudioSource> ();

        damageMultiplier = 1.0f;
		
		o_blackHole = new GameObject[5];
		for (int i = 0; i < 5; i++)
			o_blackHole[i] = GameObject.Find("BH" + i);
	}

	// Update is called once per frame
	private void Update () {

		if (PlayerHealth.isDead)
			return;

		if (player.Paused) {
			return;
		}

		transform.LookAt (playerTr.transform);

		
		if (chDirT > f_chDirCD) {
			chDirT = 0;

			f_chDirCD = Random.Range (1, 3);
			f_radians = Random.Range (0f, 3.14f);
			f_currentDistance = Vector2.Distance (playerTr.position, transform.position);
            f_movementType = Random.Range(0, 4);
			if (f_movementType == 2) {
				chDirT = 1.00f;
			}
		}
			
        
		if (f_movementType == 0)
			transform.RotateAround(playerTr.position, Vector3.up, 300f * Time.deltaTime);
		else if (f_movementType == 1)
			transform.RotateAround(playerTr.position, Vector3.up, -300f * Time.deltaTime);
		else if (f_movementType == 2)
		{
			if (f_warpT > f_warpCD) {
				f_warpT = 0;
				f_radians = Random.Range (0f, 3.14f);
				transform.position = playerTr.position + 50 * new Vector3(Mathf.Cos(f_radians), 0, Mathf.Sin(f_radians));
				NavMesh.SamplePosition(transform.position, out hit, 1000, 1);
				agent.Warp (hit.position);
				audioSources[1].Play();
			}
		}
		else
		{
			f_chDirCD = Random.Range (2, 6);
			agent.Move(Vector3.forward * 5f * Time.deltaTime);
		}

		AttackUpdate ();
		EnemyHealthSystem.Put(this);


		time += Time.deltaTime;
		chDirT += Time.deltaTime;
		f_warpT += Time.deltaTime;
	}

	private void AttackUpdate() {
		attkTM += Time.deltaTime;
	

		if (attkTM > 3) {
			attkTM = 0f;
			f_randomX = Random.Range (-0.5f, 0.5f);
			f_randomY = Random.Range (-0.5f, 0.5f);
			f_attackType = Random.Range(0, 10);
			if (f_attackType == 0) {
				// Fire ();
			}
		} 


	}

	private void Fire () {
		for (int i = 0; i < 5; i++)
		{
			if (o_blackHole[i].tag == "BH-FREE") {
				o_blackHole[i].tag = "BH-BUSY";
				//o_blackHole[i].transform.position = this.transform.position;// .position.Set(transform.position.x, transform.position.y, transform.position.z);
				o_blackHole[i].transform.position = transform.position + Vector3.up * 30f;
				o_blackHole[i].transform.LookAt(playerTr, Vector3.up);
                
				break;
			}
		}
	}

	private void OnTriggerEnter(Collider col)
	{
        bool enemy_hit = false;

		player = FindObjectOfType<PlayerController>();
		
        if (col.gameObject.CompareTag("Player_Impulse"))
        {
            enemy_hit = true;
            damage(player.DamageIm * player.damagePowCoef * (10 + player.PointsDamage) / 10);
            Destroy(col.gameObject);
            EnemyHealthSystem.Put(this);
        }
        if (col.gameObject.CompareTag("Player_ConcreteShot"))
        {
            // TODO
            enemy_hit = true;
            Destroy(col.gameObject);
            EnemyHealthSystem.Put(this);
        }
        if (col.gameObject.CompareTag("Bullet"))
        {
            enemy_hit = true;
            damage(player.DamageMa * player.damagePowCoef * (10 + player.PointsDamage) / 10);
            Destroy(col.gameObject);
            EnemyHealthSystem.Put(this);
        }
		if (col.gameObject.CompareTag("Plasma"))
		{
			enemy_hit = true;
			damage(player.DamagePl * player.damagePowCoef * (10 + player.PointsDamage) / 10);
			Destroy(col.gameObject);
			EnemyHealthSystem.Put(this);
		}
        if (enemy_hit)
        {
            GameObject.Find("HitEnemy").GetComponent<AudioSource>().Play();
        }
    }
	private void OnCollisionStay (Collision col)
	{
		if (col.gameObject.CompareTag("Player"))
		{
			PlayerHealth.takeDamage(30 * Random.Range(0.5f, 1.5f));
		}
	}

	private new void damage(float v)
	{
		currHealth -= v;
		if (currHealth <= 0)
			RIP();
	}

	protected override void RIP(bool isKilledByPlayer = true)
	{
		DifficultySystem.bossDeathTimer = 0f;
		DifficultySystem.bossesBeaten++;
        
        GameObject explosion = Instantiate(explosionP, transform.position, transform.rotation);
		Destroy(explosion, 10f);

        base.RIP();
	}
}

