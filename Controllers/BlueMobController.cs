using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class BlueMobController : MobController
{
    public GameObject sonicWave;
    
    public static float bulletTimeCloseness = 10f;
    public static float bulletSpeed = 120f;
    public float attackDistanceCoef = 1;
    public static float speed;
    public float rotSpeed = 0.01f;	


    public static float sonicWaveCD = 24f;
    public static float sonicWaveCDTimer = 0f;
    public static bool shooting = false;

    public static float score = 100;
    public static float exp = 50f;
    public static string nombre;
    private float distToBlackHole;
    private GameObject[] o_blackHole;
    private PlayerController player;

    public void Initialize (PlayerController p, PlayerHealth h, ScoringSystem s, LevelingSystem l, DifficultySystem d, GameObject w)
	{
		base.Initialize (p, h, s, l, d);
		sonicWave = w;

		agent = GetComponent<NavMeshAgent> ();
		NavMesh.SamplePosition(transform.position, out hit, 10000, 1);
		agent.Warp (hit.position);
	}

    public void Start()
    {
        speed = 10;
        updateOrientation();
        currHealth = maxHealth = 100;

        o_blackHole = new GameObject[5];
        for (int i = 0; i < 5; i++)
            o_blackHole[i] = GameObject.Find("BH" + i);

        player = FindObjectOfType<PlayerController>();
    }

	private void updateOrientation()
	{
		float tries;
		do
		{
			tries = UnityEngine.Random.Range(0, (float)(2 * Math.PI));   
		}
		while (Mathf.Abs((float)(tries - orientation)) < 4);
		orientation = tries;

		goX = (float)(Math.Sin(orientation) * speed * Time.deltaTime);
		goY = (float)(Math.Cos(orientation) * speed * Time.deltaTime);
	}

    // Update is called once per frame
    void Update()
    {
        if (PlayerHealth.isDead)
            return;
        
		if (player.Paused)
			return;
		
        // Look @player
		transform.LookAt(playerTr);

        for (int i = 0; i < 5; i++)
        {
            distToBlackHole = Vector3.Distance(o_blackHole[i].transform.position, transform.position);
            if (distToBlackHole < 250)
            {
                agent.Warp(transform.position + 250f * (o_blackHole[i].transform.position - transform.position) / (distToBlackHole * distToBlackHole) * Time.deltaTime);
                if (distToBlackHole < 20)
                {
                    // transform.Translate(Vector3.up * -10000f);
                    // Destroy(this);
                    RIP(false);
                }
            }
        }

        if (shooting && Vector3.Distance(playerTr.position, transform.position) > 200)
        {
            if (Vector3.Distance(playerTr.position, transform.position) < bulletTimeCloseness * bulletSpeed * attackDistanceCoef)
            {
                if (sonicWaveCDTimer < 0)
                {
                    sonicWaveCDTimer = sonicWaveCD;
					GameObject bullet = Instantiate(sonicWave, transform.position, transform.rotation);
					bullet.transform.LookAt (playerTr);
                    Destroy(bullet, 30);
                }
                sonicWaveCDTimer -= Time.deltaTime;
            }
        }
        
        transform.Translate(goX, 0, goY);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            transform.Translate(-collCorrection * goX, 0, -collCorrection * goY);
            updateOrientation();
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
		
    private void OnCollisionStay(Collision col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            PlayerHealth.takeDamage(7 * UnityEngine.Random.Range(0.5f, 1.5f));
        }
    }
    
	protected override void RIP(bool isKilledByPlayer = true)
    {
        if (isKilledByPlayer)
        {
            LevelSys.getExperience(exp);
            ScoreSys.getScore(score);
        }

        SpawnController.BluesNo--;
		base.RIP(isKilledByPlayer);
    }
}