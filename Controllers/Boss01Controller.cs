using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Boss01Controller : MobController {

    
	float windSpeed;
	float time;

	float attkCD, attkTM;
	float chDirT, chDirCD;

	public GameObject[] winds;
	public GameObject explosionP;
	public static int deaths;
    public static float damageMultiplier;
	public static float lifestealPowCoef;
	private PlayerController player;

	public void Initialize (PlayerController p, PlayerHealth h, ScoringSystem s, LevelingSystem l, DifficultySystem d, GameObject b) {
		base.Initialize (p, h, s, l, d);
		windSpeed = 50f;
		attkCD = 5f;
		chDirCD = 2.0f;
	}
		
    void Awake () {
		currHealth = 180f;
		maxHealth = 180f;

		winds = new GameObject[3];
		winds [0] = GameObject.Find ("TM1");
		winds [1] = GameObject.Find ("TM2");
		winds [2] = GameObject.Find ("TM3");

		player = FindObjectOfType<PlayerController> ();
		playerTr = player.transform;
		agent = GetComponent<NavMeshAgent> ();
		NavMesh.SamplePosition(transform.position, out hit, 1000, 1);
		agent.Warp (hit.position);

        time = 0;
        damageMultiplier = 1.0f;
    }
	
	// Update is called once per frame
	void Update () {

		if (PlayerHealth.isDead)
			return;

		if (player.Paused) {
			return;
		}
		
		if (chDirT > chDirCD) {
			chDirT = 0;
			chDirCD = Random.Range (1, 5);

			transform.rotation = Quaternion.LookRotation(playerTr.position - transform.position);
			transform.rotation = Quaternion.LookRotation((new Vector3(playerTr.position.x, 3, playerTr.position.z)) - transform.position);

			TornadoController.damage = 10f * damageMultiplier * (1 + 2 * (int)(time / 120f));
		}

		agent.SetDestination (playerTr.position + new Vector3(25 * (chDirCD - 3), 0, 25 * (chDirCD - 3)));
		//agent.Move (Vector3.forward * speed * Time.deltaTime);

		AttackUpdate ();

		EnemyHealthSystem.Put(this);


		time += Time.deltaTime;
		chDirT += Time.deltaTime;
    }

	void AttackUpdate() {
		attkTM += Time.deltaTime;


		if (attkTM > attkCD) {
			attkTM = 0f;


			for (int i = 0; i < 3; i++)
				winds [i].transform.position = transform.position + new Vector3 (Random.Range (10, 50), -3, Random.Range (10, 50));	

			winds [0].transform.rotation = Quaternion.LookRotation ((playerTr.position - transform.position));
			winds [1].transform.rotation = winds [0].transform.rotation;
			winds [2].transform.rotation = winds [0].transform.rotation;
		} 
		else if (attkTM > 1f) 
		{
			for (int i = 0; i < 3; i++)
				winds [i].transform.Translate (Vector3.forward * windSpeed * Time.deltaTime);
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
			Destroy(col.gameObject);
			EnemyHealthSystem.Put(this);
		}
		if (enemy_hit)
		{
	        
			GameObject.Find("HitEnemy").GetComponent<AudioSource>().Play();
		}
    }
    private void OnCollisionStay(Collision col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            PlayerHealth.takeDamage(5 * Random.Range(0.5f, 1.5f));
        }
    }

    new void damage(float v)
	{
		currHealth -= v;
		if(player.lifeStealCoef >0)
			PlayerHealth.GetHP(v * player.lifeStealCoef );
		if (currHealth <= 0)
			RIP();
	}

	protected override void RIP(bool isKilledByPlayer = true)
	{
		if (deaths == 0)
			DifficultySystem.bossDeathTimer = 0000f;
		else
			DifficultySystem.bossDeathTimer = 0000f;
		
		DifficultySystem.bossesBeaten++;

		for (int i = 0; i < 3; i++) {
            winds[i].transform.Translate(Vector3.down * 10000f);
		}

		GameObject explosion = Instantiate(explosionP, transform.position, transform.rotation);

		deaths++;
		Destroy(explosion, 10f);

		base.RIP();
	}
}
