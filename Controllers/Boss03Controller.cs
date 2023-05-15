using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Audio;

public class Boss03Controller : MobController {
	private float time;
	private static float attkTM;
	private float chDirT, f_chDirCD = 2.0f;

	public GameObject explosionP;
	private GameObject g_neutronBlast;
	private GameObject[] g_explosionCluster = new GameObject[3];
	private Quaternion qdr;
	private Vector3 dir;

	private Vector3 v3_shiftDirection;
	private float f_currentDistance, f_attackType, f_radians;
	private float f_randomX, f_randomY;

	private PlayerController player;
	private AudioSource[] audioSources = new AudioSource[2];
	private int phase = 0;
    public static float damageMultiplier;
	public static float damagePowCoef;
	private float f_timeLastTeleport;

	public void Initialize (PlayerController p, PlayerHealth h, ScoringSystem s, LevelingSystem l, DifficultySystem d, GameObject b) {
		base.Initialize (p, h, s, l, d);
	}


	private void Awake() {
		g_explosionCluster [0] = GameObject.Find ("EX0");
		g_explosionCluster [1] = GameObject.Find ("EX1");
		g_explosionCluster [2] = GameObject.Find ("EX2");
		agent = GetComponent<NavMeshAgent> ();
		NavMesh.SamplePosition(transform.position, out hit, 1000, 1);
		agent.Warp (hit.position);
		agent.speed = 15f;
		chDirT = 999;

		player = FindObjectOfType<PlayerController> ();
		playerTr = player.transform;

		currHealth = 125f;
		maxHealth = 125f;

		g_neutronBlast = GameObject.Find ("Boss03Blast");
		audioSources = GetComponents<AudioSource> ();

        damageMultiplier = 1.0f;
	}

	// Update is called once per frame
	private void Update () {

		if (PlayerHealth.isDead)
			return;

		if (player.Paused) {
			return;
		}

		transform.LookAt (playerTr.transform);

		transform.position -= 10 * Time.deltaTime * new Vector3 (Mathf.Cos (f_radians), 0, Mathf.Sin (f_radians));
		g_neutronBlast.transform.Translate (Vector3.forward * 100 * Time.deltaTime);

		if (chDirT > f_chDirCD) {
			chDirT = 0;

			phase++;
			if (phase % 3 == 1) {
				transform.position = playerTr.position + new Vector3 (30 * Mathf.Cos (f_radians), 0, 30 * Mathf.Sin (f_radians));
				NavMesh.SamplePosition (transform.position, out hit, 1000, 1);
				agent.Warp (hit.position);
				audioSources [1].Play ();
				f_timeLastTeleport = 0f;
				GetComponent<AudioSource>().PlayOneShot(Resources.Load<AudioClip>("Sounds/Hit1"));
			}
			else {
				Destroy (Instantiate (g_explosionCluster [0], transform.position, transform.rotation), 3f);
				int i = Random.Range (0, 3);
				GameObject.Find ("XS" + i.ToString ()).GetComponent<AudioSource> ().Play ();
				transform.position.Set (transform.position.x, -9999, transform.position.z);
				float f_range = 25;
				float f_maxDamage = 25 * damageMultiplier;
				if (f_currentDistance < f_range) {
					float f_damageTaken = Mathf.Min (f_maxDamage, 3f * f_maxDamage / f_currentDistance);
//					Debug.Log ("Dist: " + (int)f_currentDistance + "; Damage: " + (int)f_damageTaken);
					FindObjectOfType<PlayerHealth> ().takeDamage (f_damageTaken);
				}
				transform.position += Vector3.up * 10000;
			}

			f_chDirCD = Random.Range (1, 3);
			f_attackType = Random.Range (0, 2);
			f_radians = Random.Range (0f, 3.14f);
			f_timeLastTeleport += Time.deltaTime;
			f_currentDistance = Vector2.Distance (playerTr.position, transform.position);
            //f_movementType = Random.Range(0, 3);
		}
			
        /*
		if (f_movementType == 1)
			transform.RotateAround( playerTr.position, Vector3.up, agent.speed * Time.deltaTime);
		else if (f_movementType == 2)
			agent.Move (Vector3.forward * 5f * Time.deltaTime);
		*/

		AttackUpdate ();
		EnemyHealthSystem.Put(this);


		time += Time.deltaTime;
		chDirT += Time.deltaTime;
	}

	private void AttackUpdate() {
		attkTM += Time.deltaTime;

		if (f_timeLastTeleport < 3)
			return;
		
		if (attkTM > 3) {
			attkTM = 0f;
			f_randomX = Random.Range (-0.5f, 0.5f);
			f_randomY = Random.Range (-0.5f, 0.5f);
			f_attackType = Random.Range (0, 2);
			if (f_attackType == 1) {
				Fire ();
			}
		} 


	}

	private void Fire () {
		g_neutronBlast.transform.position = transform.position;
		g_neutronBlast.transform.LookAt (playerTr);
		//g_neutronBlast.GetComponent<Rigidbody> ().AddForce ((g_neutronBlast.transform.position - playerTr.position).normalized * 100, ForceMode.VelocityChange);

		audioSources [0].Play ();
	}

	private void OnTriggerEnter(Collider col)
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
		if(player.lifeStealCoef >0)
			PlayerHealth.GetHP(v * player.lifeStealCoef );
		if (currHealth <= 0)
			RIP();
	}

	protected override void RIP(bool isKilledByPlayer = true)
	{
		DifficultySystem.bossDeathTimer = 0f;
		DifficultySystem.bossesBeaten++;
        

        GameObject explosion = Instantiate(explosionP, transform.position, transform.rotation);
		Destroy(explosion, 10f);
        g_neutronBlast.transform.position = Vector3.down * 100000;

        base.RIP();
	}
}

