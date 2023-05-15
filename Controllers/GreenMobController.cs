using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GreenMobController : MobController
{
	private GameObject pf_greenMissile;
	public static float bulletTimeCloseness;

	private static float speed;
	public static float aggroDistance = 100;
	public float rotSpeed;

	private float f_missileTimer = 0f, f_wanderTime = 0f, f_missileCD = 30f;


	public static float exp = 75f, score = 75f;
	public static string nombre;

	public float dist2Player;



	GameObject g_missile;
    GameObject[] o_blackHole;

    private Random r;
    private float distToBlackHole;

	public void Start()
	{
		speed = 10;

		UpdateOrientation();
		currHealth = maxHealth = 70;
		player = FindObjectOfType<PlayerController> ();
		playerTr = player.transform;

		agent = GetComponent<NavMeshAgent> ();
		NavMesh.SamplePosition(transform.position, out hit, 10000, 1);
		agent.Warp (hit.position);

		f_missileTimer = Mathf.Floor (Random.Range (0, f_missileCD) / 5) * 5;

		PlayerHealth = FindObjectOfType<PlayerHealth> ();

		pf_greenMissile = GameObject.Find ("GreenMissile");
		LevelSys = FindObjectOfType<LevelingSystem> ();
		ScoreSys = FindObjectOfType<ScoringSystem> ();

        o_blackHole = new GameObject[5];
        for (int i = 0; i < 5; i++)
            o_blackHole[i] = GameObject.Find("BH" + i);
    }

	private void UpdateOrientation()
	{
		orientation = Random.Range(0, (float)(2.0f * Mathf.PI));

		goX = Mathf.Sin(orientation) * speed * Time.deltaTime;
		goY = Mathf.Cos(orientation) * speed * Time.deltaTime;
	}

	void Update()
	{
		f_missileTimer -= Time.deltaTime;
		f_wanderTime += Time.deltaTime;
		if (PlayerHealth.isDead)
			return;

		if (player.Paused)
			return;

		dist2Player = Vector3.Distance (playerTr.position, transform.position);

        for (int i = 0; i < 5; i++)
        {
            distToBlackHole = Vector3.Distance(o_blackHole[i].transform.position, transform.position);
            if (distToBlackHole < 250)
            {
                agent.Warp(transform.position + 250f * (o_blackHole[i].transform.position - transform.position) / (distToBlackHole * distToBlackHole) * Time.deltaTime);
				if (distToBlackHole < 20) {
					// transform.Translate (transform.up * -1000f);
					// Destroy (this);
					RIP(false);
				}
            }
        }


        if (dist2Player < 200)
		{
			if (f_missileTimer < 0)
			{
				LaunchGreenMissile ();
				f_missileTimer = f_missileCD;
			}
		}	        
	}

	private void OnCollisionStay(Collision col)
	{
		if (col.gameObject.CompareTag("Player"))
		{
			PlayerHealth.takeDamage(8 * Random.Range(0.5f, 1.5f));
		}
	}

	void OnTriggerEnter(Collider col)
	{
        bool enemy_hit = false;

		player = FindObjectOfType<PlayerController>();
		
		if (col.gameObject.CompareTag("Player_Impulse"))
		{
			enemy_hit = true;
			var damagePoints = player.DamageIm * player.damagePowCoef * (10 + player.PointsDamage) / 10;
			damage(damagePoints);
			if (player.lifeStealCoef  > 0f)
				PlayerHealth.GetHP((int) (damagePoints * player.lifeStealCoef ));
			if(player.vampirism > 0f)
				PlayerHealth.GetHP((int) (damagePoints * 2 * player.vampirism));
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
			var damagePoints = player.DamageMa * player.damagePowCoef * (10 + player.PointsDamage) / 10;
			damage(damagePoints);
			if (player.lifeStealCoef  > 0f)
				PlayerHealth.GetHP((int) (damagePoints * player.lifeStealCoef ));
			if(player.vampirism > 0f)
				PlayerHealth.GetHP((int) (damagePoints * 2 * player.vampirism));
			Destroy(col.gameObject);
			EnemyHealthSystem.Put(this);
		}
		if (col.gameObject.CompareTag("Plasma"))
		{
			enemy_hit = true;
			var damagePoints = player.DamagePl * player.damagePowCoef * (10 + player.PointsDamage) / 10;
			damage(damagePoints);
			if (player.lifeStealCoef  > 0f)
				PlayerHealth.GetHP((int) (damagePoints * player.lifeStealCoef ));
			if(player.vampirism > 0f)
				PlayerHealth.GetHP((int) (damagePoints * 2 * player.vampirism));
			Destroy(col.gameObject);
			EnemyHealthSystem.Put(this);
		}
		if (enemy_hit)
		{
	        
			GameObject.Find("HitEnemy").GetComponent<AudioSource>().Play();
		}
    }

	protected void RIP(bool isKilledByPlayer = true)
	{
		if (isKilledByPlayer)
		{
			LevelSys.getExperience(exp);
			ScoreSys.getScore(score);	
		}
		
		SpawnController.GreensNo--;
		base.RIP(isKilledByPlayer);
	}

	private void LaunchGreenMissile () {
		for (int i = 0; i < 10; i++)
		{
			var currentMissile = GameObject.Find ("GM" + i).GetComponent<GreenMissileController> ();
			if (currentMissile.f_time < 0) {
				currentMissile.transform.position = transform.position + Vector3.up * 20f;
				currentMissile.Attack ();
				break;
			}
		}
	}
}
