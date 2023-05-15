using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class RedMobController : MobController
{
    public GameObject bulletPrefab;
    public GameObject shotgunBulletPrefab;
    public static float bulletTimeCloseness;
    public float attackDistanceCoef = 1;

    public static float speed;

    private float setBulletCooldown = 10f;
    private float bulletCooldown = 0f;
    public static bool shooting = false;

    public static float exp = 15f, score = 40f;
    public static string nombre;
	public Boolean super;

	private PlayerController player;

	public float dist2Player;

	Quaternion rotation;
	RedDefaultGunController defaultBullet;
	RedSplashController splashBullet;

	Renderer rend;

	private UnityEngine.Random r;

	Quaternion qst = Quaternion.Euler (0, -8, 0), qdr = Quaternion.Euler (0, 16, 0);
    private float distToBlackHole;
    private GameObject[] o_blackHole;
	public static float damagePowCoef;
	public static float cooldownArbalest = 1f;
	private bool arbalest1, arbalest2;
	private double arbalestTimer;

	public void Initialize (PlayerController p, PlayerHealth h, ScoringSystem s, LevelingSystem l, DifficultySystem d, bool _super) {
		base.Initialize (p, h, s, l, d);
	    super = _super;
    }

    public void Start()
    {
        speed = 7;
		
        updateOrientation();
        currHealth = maxHealth = 28;

		bulletCooldown = UnityEngine.Random.Range(0, 9);

		rend = GetComponent<Renderer>();
		rend.enabled = true;

		agent = GetComponent<NavMeshAgent> ();
		NavMesh.SamplePosition(transform.position, out hit, 200, 1);
		agent.Warp (hit.position);
	    player = FindObjectOfType<PlayerController>();
        o_blackHole = new GameObject[5];
        for (int i = 0; i < 5; i++) 
            o_blackHole[i] = GameObject.Find("BH" + i);
	    playerTr = FindObjectOfType<PlayerController>().transform;
	    PlayerHealth = FindObjectOfType<PlayerHealth>();
    }

    private void updateOrientation()
    {
        float tries;
        do
        {
            tries = UnityEngine.Random.Range(0, (float)(2 * Math.PI));   
        }
        while (Mathf.Abs(tries - orientation) < 4);
        orientation = tries;

        goX = (float)(Math.Sin(orientation) * speed * Time.deltaTime);
        goY = (float)(Math.Cos(orientation) * speed * Time.deltaTime);
    }

    void Update()
    {
	    double max = 0;
		dist2Player = Vector3.Distance (playerTr.position, transform.position);
		if (dist2Player > max)
		{
			max = dist2Player;
		}
		if (max > 0)
		{
			// rend.enabled = true;

            // Look @player
            transform.LookAt(playerTr);

            // Move towards player
			Vector3 posCurrent = transform.position;
            agent.Warp(posCurrent + transform.forward * speed * Time.deltaTime);

			// Blackhole attraction
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

			Vector3 posNext = transform.position;

			float yDifference = posNext.y - posCurrent.y;
			if (arbalestTimer < 0)
			{
				arbalest1 = false;
				arbalest2 = false;
				bulletCooldown = -1f;
				arbalestTimer = setBulletCooldown;
			}
            if (shooting)
			{
				if (dist2Player < bulletTimeCloseness * RedDefaultGunController.speed * attackDistanceCoef)
				{
					if (player.cooldownArbalest > 1f)
					{
						if (!arbalest1 && arbalestTimer < setBulletCooldown - 0.25f)
						{
							arbalest1 = true;
							bulletCooldown = -1f;
						}
						if (!arbalest2 && arbalestTimer < setBulletCooldown - 0.5f)
						{
							arbalest2 = true;
							bulletCooldown = -1f;
						}
					}
					if (bulletCooldown < 0)
					{
						rotation = Quaternion.LookRotation(playerTr.position - transform.position);

						bulletCooldown = setBulletCooldown;

						if (!super)
						{
							defaultBullet = Instantiate(bulletPrefab, transform.position, rotation).GetComponent<RedDefaultGunController>();
							bulletPrefab.GetComponent<RedDefaultGunController>().setParent(this);
							Destroy(defaultBullet, bulletTimeCloseness);
						}
						else {
							splashBullet = Instantiate(shotgunBulletPrefab, transform.position, rotation).GetComponent<RedSplashController>();
							Destroy(splashBullet, bulletTimeCloseness);
							rotation *= qst;
							transform.rotation = Quaternion.Slerp (transform.rotation, rotation, Time.deltaTime * 0);
							splashBullet = Instantiate (shotgunBulletPrefab, transform.position, rotation).GetComponent<RedSplashController>();
							Destroy (splashBullet, bulletTimeCloseness);
							rotation *= qdr;
							transform.rotation = Quaternion.Slerp (transform.rotation, rotation, Time.deltaTime * 0);
							splashBullet = Instantiate (shotgunBulletPrefab, transform.position, rotation).GetComponent<RedSplashController>();
							Destroy (splashBullet, bulletTimeCloseness);
						}
					}
				}
				bulletCooldown -= Time.deltaTime;
				arbalestTimer -= Time.deltaTime;
			}
		}
		else // Idle, wandering
		{
			agent.Warp (transform.position + new Vector3(goX, 0, goY));
			rend.enabled = false;
		}        
    }

	private void OnCollisionEnter(Collision col)
    {
		if (col.gameObject.CompareTag("Wall"))
        {
            transform.Translate(- collCorrection * goX, 0, - collCorrection * goY);
            updateOrientation();
        }
    }

	private void OnCollisionStay(Collision col)
	{
		if (col.gameObject.CompareTag("Player"))
		{
			PlayerHealth.takeDamage(1.5f * UnityEngine.Random.Range(0.5f, 1.5f));
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

    protected override void RIP(bool isKilledByPlayer = true)
    {
	    if (isKilledByPlayer)
	    {
		    LevelSys.getExperience(exp);
		    ScoreSys.getScore(score);
	    }

	    SpawnController.RedsNo--;
		base.RIP(isKilledByPlayer);
    }
}
