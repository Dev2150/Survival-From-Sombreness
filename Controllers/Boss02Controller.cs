using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class Boss02Controller : MobController {


	float speed;
	float time;
	static float attkTM;
	float chDirT, f_chDirCD = 2.0f;

	public GameObject explosionP;
	GameObject[] g_explosionCluster = new GameObject[3];
	private Quaternion qdr;
	Vector3 dir;

	Vector3 v3_shiftDirection;
	float f_movementType, f_currentDistance, f_attackType;
	float f_randomX, f_randomY;
	private PlayerController player;

	bool isClusterExplosion;

	private GameObject[] g_clusterBombs;
	private GameObject[] g_rockets;
    private PlayerController playerController;
    public static float damageMultiplier;
	public static float damagePowCoef;

	public void Initialize (PlayerController p, PlayerHealth h, ScoringSystem s, LevelingSystem l, DifficultySystem d, GameObject b) {
		base.Initialize (p, h, s, l, d);

		player = FindObjectOfType<PlayerController> ();
		playerTr = player.transform;
	}


	void Awake() {
		//g_rocket = GameObject.Find ("R1");
		g_explosionCluster [0] = GameObject.Find ("EX0");
		g_explosionCluster [1] = GameObject.Find ("EX1");
		g_explosionCluster [2] = GameObject.Find ("EX2");
		agent = GetComponent<NavMeshAgent> ();
		NavMesh.SamplePosition(transform.position, out hit, 1000, 1);
		agent.Warp (hit.position);
		agent.speed = 20f;
		chDirT = 999;

		currHealth = 200f;
		maxHealth = 200f;

		g_clusterBombs = new GameObject[3];

		g_clusterBombs [0] = GameObject.Find ("CB0");
		g_clusterBombs [1] = GameObject.Find ("CB1");
		g_clusterBombs [2] = GameObject.Find ("CB2");

        playerController = FindObjectOfType<PlayerController>();

        g_rockets = new GameObject[9];
        for (int i = 1; i <= 9; i++)
			g_rockets [i - 1] = GameObject.Find ("R" + i);

        damageMultiplier = 1f;

    }

	// Update is called once per frame
	void Update () {
		if (player.Paused) {
			return;
		}

		transform.LookAt (playerTr.transform);

		if (chDirT > f_chDirCD) {
			chDirT = 0;

			f_chDirCD = Random.Range (1, 2);
			f_attackType = Random.Range (0, 2);
			f_currentDistance = Vector2.Distance (playerTr.position, transform.position);
			if (f_currentDistance < 10 || f_currentDistance > 1000)
				f_movementType = 0;
			else 
				f_movementType = Random.Range (0, 4);

			if (f_movementType == 1) {
				f_chDirCD = 2;
			}
			else if (f_movementType == 2) {
				speed *= -1;
				f_chDirCD = 1;
			}
			else if (f_movementType == 3 || f_movementType == 0) {
				agent.SetDestination (playerTr.position + new Vector3 (25 * (f_chDirCD - 3), 0, 25 * (f_chDirCD - 3)));
				f_chDirCD = 1;
			}
		}
			
		if (f_movementType == 1)
			transform.RotateAround( playerTr.position, Vector3.up, agent.speed * Time.deltaTime);
		else if (f_movementType == 2)
			agent.Move (Vector3.forward * speed * Time.deltaTime);
		

		AttackUpdate ();
		EnemyHealthSystem.Put(this);


		time += Time.deltaTime;
		chDirT += Time.deltaTime;
	}
		
	void AttackUpdate() {
		attkTM += Time.deltaTime;
	

		if (attkTM > 5f) {
			attkTM = 0f;
            f_attackType = Random.Range(0, 3);
            if (f_attackType <= 1) { // Explode all rockets
				Fire (); // Launch all rockets
				isClusterExplosion = false;
                
                for (int j = 0; j < 9; j++) {
                    Destroy (Instantiate (g_explosionCluster [Random.Range (0, 3)], g_rockets[j].transform.position, transform.rotation), 2f);

                    float f_distance = Vector2.Distance (g_rockets[j].transform.position, playerController.transform.position);
					float f_range = 50;
					float f_maxDamage = damageMultiplier * 100;
					if (f_distance < f_range) {
						float f_damageTaken = Mathf.Min (f_maxDamage, 3f * f_maxDamage / f_distance);
						Debug.Log ("Dist: " + f_distance + "; Damage: " + f_damageTaken);
						FindObjectOfType<PlayerHealth> ().takeDamage (f_damageTaken);
					}
					g_rockets[j].transform.position = Vector3.down * 10000;


                    int i = Random.Range(0, 3);
                    GameObject.Find("XS" + i.ToString()).GetComponent<AudioSource>().volume = Mathf.Max(1, 10 / f_distance);
                    GameObject.Find ("XS" + i.ToString ()).GetComponent<AudioSource> ().Play ();
				}
			}
		} else { // explode clusters into rockets
			if (!isClusterExplosion && attkTM > 1f) {
				isClusterExplosion = true;
                GameObject explosion;

                for (int i = 0; i < 3; i++) {
                    f_randomX = Random.Range(15f, 30f);
                    f_randomY = Random.Range(15f, 30f);
                   // f_randomX *= 1 - 2 * Random.Range(0, 1);
                    //f_randomY *= 1 - 2 * Random.Range(0, 1);
                    explosion = Instantiate (g_explosionCluster [i], g_clusterBombs [i].transform.position, transform.rotation);
                    Destroy(explosion, 2f);

                    g_rockets[3 * i + 0].transform.position = explosion.transform.position + new Vector3 (f_randomX, 0, f_randomY);//, transform.rotation);
					g_rockets[3 * i + 1].transform.position = explosion.transform.position;
					g_rockets[3 * i + 2].transform.position = explosion.transform.position + new Vector3 (-f_randomX, 0, -f_randomY);//, transform.rotation);

                    g_rockets[3 * i + 0].transform.LookAt(player.transform);
                    g_rockets[3 * i + 1].transform.LookAt(player.transform);
                    g_rockets[3 * i + 2].transform.LookAt(player.transform);                    

                    int j = Random.Range (0, 3);
					GameObject.Find("XS" + j.ToString()).GetComponent<AudioSource>().Play();
					g_clusterBombs [i].transform.position = new Vector3 (9999, -9999, 9999);
				}
					
			}
		}
			

		g_clusterBombs [0].transform.position += 2 * Vector3.up;
		g_clusterBombs [1].transform.position += 2 * new Vector3 (1, 1, 1);
		g_clusterBombs [2].transform.position += 2 * new Vector3 (-1, 1, -1);


	}

	void Fire () {

		g_clusterBombs [0].transform.position = transform.position;

        int i;
        for (i = 0; i < 3; i++) {
            f_randomX = Random.Range(15f, 30f);
            f_randomY = Random.Range(15f, 30f);
            f_randomX *= 1 - 2 * Random.Range(0, 1);
            f_randomY *= 1 - 2 * Random.Range(0, 1);
            v3_shiftDirection.x = Random.Range (-10, 10);
			v3_shiftDirection.y = 5;
			v3_shiftDirection.z = Random.Range (-10, 10);
			g_clusterBombs [i].transform.position = transform.position + v3_shiftDirection * Time.deltaTime; 
		}
		GetComponent<AudioSource>().Play();
	}

	void OnTriggerEnter(Collider col)
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
    private void OnCollisionStay(Collision col)
    {
        bool enemy_hit = false;

	    player = FindObjectOfType<PlayerController>();
	    
        if (col.gameObject.CompareTag("Player_Impulse"))
        {
            enemy_hit = true;
            damage(player.DamageIm * (10 + player.PointsDamage) / 10);
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
            damage(player.DamageMa * (10 + player.PointsDamage) / 10);
            Destroy(col.gameObject);
            EnemyHealthSystem.Put(this);
        }
        if (enemy_hit)
        {
            GameObject.Find("HitEnemy").GetComponent<AudioSource>().Play();
        }
    }

	protected override void RIP(bool isKilledByPlayer = true)
	{
		DifficultySystem.bossDeathTimer = 0f;
		DifficultySystem.bossesBeaten++;

	
		for (int i = 0; i < 3; i++)
			g_clusterBombs [i].transform.position = Vector3.down * 10000f;
		for (int i = 1; i <= 9; i++)
			g_rockets [i - 1].transform.position = Vector3.down * 10000f;

        GameObject explosion = Instantiate(explosionP, transform.position, transform.rotation);
		Destroy(explosion, 10f);

		base.RIP();
	}
}

