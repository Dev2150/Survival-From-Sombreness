using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackMobController : MobController {
    public GameObject[] o_blackHole;
    private int speed = 20;
    public float blackHoleCDTimer = -0.1f;
    public float blackHoleCD = 180f;
    public float exp = 125;
    public float score = 200;
    private PlayerController player;

    public void Initialize(PlayerController p, PlayerHealth h, ScoringSystem s, LevelingSystem l, DifficultySystem d, GameObject b)
    {
        base.Initialize(p, h, s, l, d);
        
        player = FindObjectOfType<PlayerController>();
        playerTr = player.transform;
    }

    public void Start()
    {
        speed = 7;

        UpdateOrientation();
        currHealth = maxHealth = 200;

        blackHoleCDTimer = UnityEngine.Random.Range(0, 9);

        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        UnityEngine.AI.NavMesh.SamplePosition(transform.position, out hit, 1000, 1);
        agent.Warp(hit.position);

        o_blackHole = new GameObject[5];
        for (int i = 0; i < 5; i++)
            o_blackHole[i] = GameObject.Find("BH" + i);
    }

    private void UpdateOrientation()
    {
        orientation = UnityEngine.Random.Range(0, 2.0f * Mathf.PI);

        goX = Mathf.Sin(orientation) * speed * Time.deltaTime;
        goY = Mathf.Cos(orientation) * speed * Time.deltaTime;
    }

    void Update()
    {
        if (PlayerHealth.isDead)
            return;
        
        // Look @player
        transform.LookAt(playerTr);

        if (Vector3.Distance(playerTr.position, transform.position) > 400) {
            if (blackHoleCDTimer < 0)
            {
                for (int i = 0; i < 5; i++)
                {
                    if (o_blackHole[i].tag == "BH-FREE") {
                        o_blackHole[i].tag = "BH-BUSY";
                        blackHoleCDTimer = blackHoleCD;
                        //o_blackHole[i].transform.position = this.transform.position;// .position.Set(transform.position.x, transform.position.y, transform.position.z);
                        o_blackHole[i].transform.position = transform.position;
                        o_blackHole[i].transform.LookAt(playerTr, Vector3.up);
                        
                        break;
                    }
                }
            }
        }

        
        blackHoleCDTimer -= Time.deltaTime;
        transform.Translate(goX, 0, goY);
    }

    private void OnCollisionStay(Collision col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            PlayerHealth.takeDamage(5 * UnityEngine.Random.Range(0.5f, 1.5f));
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

    protected override void RIP(bool isKilledByPlayer = false)
    {
        if (isKilledByPlayer)
        {
            LevelSys.getExperience(exp);
            ScoreSys.getScore(score);
        }

        SpawnController.BlacksNo--;
        base.RIP(isKilledByPlayer);
    }

}
