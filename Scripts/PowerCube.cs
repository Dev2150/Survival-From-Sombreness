using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class PowerCube : MonoBehaviour
{
	public Sprite[] powerSprites;
	NavMeshHit hit;
	private Vector3 randomPoint;
	private NavMeshAgent navMeshAgent;
	private Color targetColor;
	private Material material;
	private float colorFlipFlopTimer, colorFlipFlopCD = 0.5f;
	private bool isColoringToPink;
	private static int _positivePowers = 8;
	private static int _neutralPowers = 3;
	private static readonly int _negativePowers = 9;
	private int currentPower;
	private PlayerController player;
	private string textPowerSpeedMore;
	
	private Image[] imageUI;
	private PlayerHealth health;

	// Use this for initialization
	void Start()
	{
		player = FindObjectOfType<PlayerController>();
		health = FindObjectOfType<PlayerHealth>();
		randomPoint = transform.position;
		NavMesh.SamplePosition(randomPoint, out hit, 200f, NavMesh.AllAreas);
		navMeshAgent = GetComponent<NavMeshAgent>();
		navMeshAgent.Warp(hit.position);

		imageUI = new Image[5];

		for (int i = 0; i < imageUI.Length; i++)
			imageUI[i] = GameObject.Find("Power0" + (i + 1)).GetComponent<Image>();



/*		powerSprites = new Sprite[13];
		powerSprites[0] = Resources.Load<Sprite>("/Powers/power_null");
		powerSprites[1] = Resources.Load<Sprite>("/Powers/power_speed_up");
		powerSprites[2] = Resources.Load<Sprite>("/Powers/power_speed_down");
		powerSprites[3] = Resources.Load<Sprite>("/Powers/power_afterburner_up");
		powerSprites[4] = Resources.Load<Sprite>("/Powers/power_afterburner_down");
		powerSprites[5] = Resources.Load<Sprite>("/Powers/power_health_up");
		powerSprites[6] = Resources.Load<Sprite>("/Powers/power_health_down");
		powerSprites[7] = Resources.Load<Sprite>("/Powers/power_attspd_up");
		powerSprites[8] = Resources.Load<Sprite>("/Powers/power_attspd_down");
		powerSprites[9] = Resources.Load<Sprite>("/Powers/power_mana_up");
		powerSprites[10] = Resources.Load<Sprite>("/Powers/power_mana_down");
		powerSprites[11] = Resources.Load<Sprite>("/Powers/power_damage_up");
		powerSprites[12] = Resources.Load<Sprite>("/Powers/power_damage_down");*/
	}

	// Update is called once per frame
	void Update()
	{
		material = GetComponent<MeshRenderer>().material;
		material.SetColor("_Color", Color.Lerp(material.color, targetColor, 0.15f));

		if (colorFlipFlopTimer > colorFlipFlopCD)
		{
			colorFlipFlopTimer = 0f;
			isColoringToPink = !isColoringToPink;
			targetColor = isColoringToPink ? new Color(1.0f, 0f, 0.630f, 1.0f) : new Color(0.55f, 0f, 1.0f, 1.0f);
		}

		colorFlipFlopTimer += Time.deltaTime;
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			var e = Random.Range(0.0f, 100f);
			float duration = 60f;
			if (e < 55f)
				getPositivePower(duration);
			else if (e < 65f) //62.5f)
				getNeutralPower(duration);
			else
				createNegativePower(player, health, this);	

			Destroy(gameObject);
		}
	}

	public static void createNegativePower(PlayerController player, PlayerHealth health, PowerCube powerCube)
	{
		Sprite[] powerSprites = powerCube.powerSprites;
		int currentPower = Random.Range(0, _negativePowers - Convert.ToInt32(player.PointsSh == 0));
		float duration = 60f;
		switch (currentPower)
		{
			case 0:
				createPower(player, health, PlayerController.Powers.SpeedLess, duration, powerSprites, 2);
				break;
			case 1:
				createPower(player, health, PlayerController.Powers.NoShift, duration, powerSprites, 4);
				break;
			case 2:
				createPower(player, health, PlayerController.Powers.HealthLess, duration, powerSprites, 6);
				break;
			case 3:
				createPower(player, health, PlayerController.Powers.AttackSpeedLess, duration, powerSprites, 8);
				break;
			case 4:
				createPower(player, health, PlayerController.Powers.ManaLess, duration, powerSprites, 10);
				break;
			case 5:
				createPower(player, health, PlayerController.Powers.DamageLess, duration, powerSprites, 12);
				break;
			case 6:
				createPower(player, health, PlayerController.Powers.LifestealEnemy, duration, powerSprites, 14);
				break;
			case 7:
				createPower(player, health, PlayerController.Powers.BulletGravity, duration, powerSprites, 15);
				break;
			/*case 8:
						createPower(player, health, PlayerController.Powers.EnemyRegen, duration, powerSprites[20]);
						break;*/
			case 8:
				createPower(player, health, PlayerController.Powers.ShieldBreak, duration, powerSprites, 17);
				break;
		}
	}

	private void getNeutralPower(float duration)
	{
		currentPower = Random.Range(0, _neutralPowers);
		switch (currentPower)
		{
			case 0:
				createPower(player, health, PlayerController.Powers.Vampirism, duration, powerSprites, 16);
				break;
			case 1:
				createPower(player, health, PlayerController.Powers.Arbalest, duration, powerSprites, 19);
				break;
			case 2:
				createPower(player, health, PlayerController.Powers.Glass, duration, powerSprites, 20);
				break;
		}
	}

	private void getPositivePower(float duration)
	{
		currentPower = Random.Range(0, _positivePowers);
		switch (currentPower)
		{
			case 0:
				createPower(player, health, PlayerController.Powers.SpeedMore, duration, powerSprites, 1);
				break;
			case 1:
				createPower(player, health, PlayerController.Powers.DoubleShift, duration, powerSprites, 3);
				break;
			case 2:
				createPower(player, health, PlayerController.Powers.HealthMore, duration, powerSprites, 5);
				break;
			case 3:
				createPower(player, health, PlayerController.Powers.AttackSpeedMore, duration, powerSprites, 7);
				break;
			case 4:
				createPower(player, health, PlayerController.Powers.ManaMore, duration, powerSprites, 9);
				break;
			case 5:
				createPower(player, health, PlayerController.Powers.DamageMore, duration, powerSprites, 11);
				break;
			case 6:
				createPower(player, health, PlayerController.Powers.LifestealPlayer, duration, powerSprites, 13);
				break;
			case 7:
				createPower(player, health, PlayerController.Powers.BulletDoubleSpeed, duration, powerSprites, 18);
				break;
		}
	}

	public static void createPower(PlayerController player, PlayerHealth health, PlayerController.Powers power, float duration, Sprite[] powerSprites, int currentPower)
	{
		for (int i = 0; i < player.PowerNames.Length; i++)
		{
			if (player.PowerNames[i] == PlayerController.Powers.None ||
			    (player.PowerNames[i] == PlayerController.Powers.SpeedLess ||
			     player.PowerNames[i] == PlayerController.Powers.SpeedMore) &&
			    (power == PlayerController.Powers.SpeedMore || power == PlayerController.Powers.SpeedLess) ||
			    (player.PowerNames[i] == PlayerController.Powers.HealthMore ||
			     player.PowerNames[i] == PlayerController.Powers.HealthLess) &&
			    (power == PlayerController.Powers.HealthLess || power == PlayerController.Powers.HealthMore) ||
			    (player.PowerNames[i] == PlayerController.Powers.NoShift ||
			     player.PowerNames[i] == PlayerController.Powers.DoubleShift) &&
			    (power == PlayerController.Powers.NoShift || power == PlayerController.Powers.DoubleShift) ||
			    (player.PowerNames[i] == PlayerController.Powers.AttackSpeedMore ||
			     player.PowerNames[i] == PlayerController.Powers.AttackSpeedLess) &&
			    (power == PlayerController.Powers.AttackSpeedMore || power == PlayerController.Powers.AttackSpeedLess) ||
			    (player.PowerNames[i] == PlayerController.Powers.ManaMore ||
			     player.PowerNames[i] == PlayerController.Powers.ManaLess) &&
			    (power == PlayerController.Powers.ManaMore || power == PlayerController.Powers.ManaLess) ||
			    (player.PowerNames[i] == PlayerController.Powers.DamageMore ||
			     player.PowerNames[i] == PlayerController.Powers.DamageLess) &&
			    (power == PlayerController.Powers.DamageMore || power == PlayerController.Powers.DamageLess) ||
			    (player.PowerNames[i] == PlayerController.Powers.LifestealPlayer ||
			     player.PowerNames[i] == PlayerController.Powers.LifestealEnemy) &&
			    (power == PlayerController.Powers.LifestealPlayer || power == PlayerController.Powers.LifestealEnemy) ||
			    (player.PowerNames[i] == PlayerController.Powers.BulletGravity ||
			     player.PowerNames[i] == PlayerController.Powers.BulletDoubleSpeed) &&
			    (power == PlayerController.Powers.BulletGravity || power == PlayerController.Powers.BulletDoubleSpeed) ||
			    player.PowerNames[i] == PlayerController.Powers.Arbalest && power == PlayerController.Powers.Arbalest ||
			    player.PowerNames[i] == PlayerController.Powers.ShieldBreak && power == PlayerController.Powers.ShieldBreak ||
			    player.PowerNames[i] == PlayerController.Powers.Vampirism && power == PlayerController.Powers.Vampirism ||
			    player.PowerNames[i] == PlayerController.Powers.Glass && power == PlayerController.Powers.Glass
			    
			)
			{
				player.PowerNames[i] = power;
				player.PowerTimes[i] = duration;
				player.PowerDurations[i] = duration;
    				Debug.Log("Power #" + i + " : " + currentPower);
				player.CurrentPowerImages[i].sprite = powerSprites[currentPower];
				player.BackPowerImages[i].sprite = powerSprites[currentPower];
				break;
			}
		}

		switch (power)
		{
			case PlayerController.Powers.SpeedMore:
				player.speedPowCoef = 1.33f;
				break;
			case PlayerController.Powers.SpeedLess:
				player.speedPowCoef = 0.75f;
				break;
			case PlayerController.Powers.DoubleShift:
				player.afterburnerPowCoef = 2;
				break;
			case PlayerController.Powers.NoShift:
				player.afterburnerPowCoef = 0;
				break;
			case PlayerController.Powers.HealthMore:
				health.healthRegenPowCoef = 2f;
				break;
			case PlayerController.Powers.HealthLess:
				health.healthRegenPowCoef = 0.5f;
				break;
			case PlayerController.Powers.AttackSpeedMore:
				player.cooldownPowCoef = 1.5f;
				break;
			case PlayerController.Powers.AttackSpeedLess:
				player.cooldownPowCoef = 0.66f;
				break;
			case PlayerController.Powers.ManaMore:
				FindObjectOfType<EnergySystem>().energyRegenCoef = 2f;
				break;
			case PlayerController.Powers.ManaLess:
				FindObjectOfType<EnergySystem>().energyRegenCoef = 0.5f;
				break;
			case PlayerController.Powers.LifestealPlayer:
				player.lifeStealCoef = 0.1f;
				break;
			case PlayerController.Powers.LifestealEnemy:
				RedDefaultGunController.enemyVampirism = 0.2f;
				
				TornadoController.enemyVampirism = 0.2f;
				Boss02Cluster.enemyVampirism = 0.2f;
				NeutronBlast.enemyVampirism = 0.2f;
				CollideSlash.enemyVampirism = 0.2f;
				break;
			case PlayerController.Powers.BulletGravity:
				BulletController.gravity = 1f;
				break;
			case PlayerController.Powers.ShieldBreak:
				health.isShieldDisabled = false;
				break;
			case PlayerController.Powers.Vampirism:
				player.vampirism = 0.2f;
				break;
			case PlayerController.Powers.DamageMore:
				player.damagePowCoef = 2f;
				break;
			case PlayerController.Powers.DamageLess:
				player.damagePowCoef = 0.5f;
				break;
			case PlayerController.Powers.BulletDoubleSpeed:
				BulletController.bulletSpeedCoef = 2f;
				break;
			case PlayerController.Powers.Arbalest:
				FindObjectOfType<EnergySystem>().energyRegenArbalest = 4f;
				player.cooldownArbalest = 2f;
				RedMobController.cooldownArbalest = 4f;
				break;
			/*case PlayerController.Powers.EnemyRegen:
				MobController.regen = 1f;
				RedMobController.cooldownArbalest = 4f;
				break;*/
			case PlayerController.Powers.Glass:
				health.baseMaxHPPowCoef = 0.5f;
				health.CurrentHP *= 0.5f;
				health.BaseMaxHP += 0.1f;
				player.damageGlassCoef = 1.5f;
				break;
			default:
				Debug.Log("Power undefined: " + power);
				break;
		}
	}
}