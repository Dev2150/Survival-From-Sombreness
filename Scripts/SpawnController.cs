using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class SpawnController : MonoBehaviour
{
    private const int redSpawnPosY = 2;
    private PlayerController _player;
	private Transform _playerTr;
	private PlayerHealth _playerHealth;
	public GameObject EnergyCube, LifeCube, PowerCube;
	public Transform EnergySphere, Reds, Blues, Greens, Blacks, Bosses;
	private DifficultySystem _difficulty;
	private ScoringSystem _score;
	private LevelingSystem _level;

	public GameObject GRed, GRedSplash,
		GBlue,
		GGreen,
		GBlack,
		GOrange,
		GBoss01,
		GBoss02,
		GBoss03,
		GBoss04,
		GRedBullet,
		GWind,
		GSonicWave,
		GGreenMissile;

	public Text Pop;

	public static int RedsNo, BluesNo, GreensNo, BlacksNo, ECno, LCno, PCno;
	public static int OrangesNo;
	public Transform Oranges;
	private GameObject[] _oBlackHole;

	void Awake()
	{
		_player = FindObjectOfType<PlayerController>();
		_playerTr = _player != null ? _player.transform : null; //new Vector3(500, 0, 500);
		_playerHealth = FindObjectOfType<PlayerHealth>();
		EnergySphere = GameObject.Find("EnergySphere").GetComponent<Transform>();
		_level = FindObjectOfType<LevelingSystem>();
		_score = FindObjectOfType<ScoringSystem>();
		_difficulty = FindObjectOfType<DifficultySystem>();

		//redBullet = (GameObject)Resources.Load("Prefabs/RedBullet", typeof(GameObject));
		GRedBullet = GameObject.Find("RedBullet");
		GWind = GameObject.Find("Wind");
		GSonicWave = GameObject.Find("SonicWave");

		if (!GRedBullet)
			Debug.Log("RedBullet - null");

		_oBlackHole = new GameObject[5];
		for (int i = 0; i < 5; i++)
			_oBlackHole[i] = GameObject.Find("BH" + i);
	}

	void WEnergy()
	{
		float posX = UnityEngine.Random.Range(-600, +600);
		float posZ = UnityEngine.Random.Range(-600, +600);

		GameObject e = Instantiate(EnergyCube, new Vector3(posX, 0, posZ),
			Quaternion.Euler(45, 45, 45));
		e.transform.parent = EnergySphere.transform;

		ECno++;
		UpdateText();
	}

	void WLife()
	{
		float posX = UnityEngine.Random.Range(-600, +600);
		float posZ = UnityEngine.Random.Range(-600, +600);

		GameObject e = Instantiate(LifeCube, new Vector3(posX, 0, posZ),
			Quaternion.Euler(45, 45, 45));
		e.transform.parent = EnergySphere.transform;

		LCno++;
		UpdateText();
	}
	
	void WPower()
	{
		float posX = UnityEngine.Random.Range(-600, +600);
		float posZ = UnityEngine.Random.Range(-600, +600);

		GameObject e = Instantiate(PowerCube, new Vector3(posX, 0, posZ),
			Quaternion.Euler(45, 45, 45));
		e.transform.parent = EnergySphere.transform;

		PCno++;
		UpdateText();
	}

	void WBlue()
	{
		if (BluesNo >= 10)
			return;
		float posX, posZ;
		if (!_difficulty.close)
		{
			int okay = 50;
			while (okay > 0)
			{
				posX = UnityEngine.Random.Range(-600, +600);
				posZ = UnityEngine.Random.Range(-600, +600);

				if (IsFarFromPlayer(posX, posZ, 400))
				{
					GameObject e = Instantiate(GBlue, new Vector3(posX, 4, posZ), Quaternion.identity);

					BlueMobController b = e.GetComponent<BlueMobController>();
					b.Start();
					b.Initialize(_player, _playerHealth, _score, _level, _difficulty, GSonicWave);
					b.transform.parent = Blues.transform;

					okay = -1;
				}
				else
					okay--;
			}

			if (okay == 0)
				Debug.Log("Failed instantiating WBlue");
			else
			{
				BluesNo++;
				UpdateText();
			}
		}
		else
		{
			posX = UnityEngine.Random.Range(-100, +100);
			posZ = UnityEngine.Random.Range(-100, +100);

			GameObject e = Instantiate(GRed, new Vector3(posX, redSpawnPosY, posZ), Quaternion.identity);
			e.GetComponent<BlueMobController>().Start();
		}
	}

	void WBlack()
	{
		if (BlacksNo >= 2)
			return;

		float posX, posZ;
		if (!_difficulty.close)
		{
			int okay = 50;
			while (okay > 0)
			{
				posX = UnityEngine.Random.Range(-600, +600);
				posZ = UnityEngine.Random.Range(-600, +600);

				if (IsFarFromPlayer(posX, posZ, 450) && _playerTr.position.z + posZ < 666)
				{
					GameObject e = Instantiate(GBlack, _playerTr.position + new Vector3(posX, 4, posZ),
						Quaternion.identity);

					BlackMobController b = e.GetComponent<BlackMobController>();
					b.Start();
					b.Initialize(_player, _playerHealth, _score, _level, _difficulty);
					b.transform.parent = Blacks.transform;

					okay = -1;
				}
				else
					okay--;
			}

			if (okay == 0)
				Debug.Log("Failed instantiating WBlack!!!");
			else
			{
				BlacksNo++;
				UpdateText();
			}
		}
		else
		{
			posX = UnityEngine.Random.Range(-100, +100);
			posZ = UnityEngine.Random.Range(-100, +100);

			GameObject e = Instantiate(GBlack, new Vector3(posX, 1, posZ), Quaternion.identity);
			e.GetComponent<BlackMobController>().Start();
		}
	}

	void WOrange()
	{
		if (OrangesNo >= 5)
			return;
		float posX, posZ;
		if (!_difficulty.close)
		{
			int okay = 50;
			while (okay > 0)
			{
				posX = UnityEngine.Random.Range(-600, +600);
				posZ = UnityEngine.Random.Range(-600, +600);

				if (IsFarFromPlayer(posX, posZ, 400))
				{
					GameObject e = Instantiate(GOrange, new Vector3(posX, 4, posZ), Quaternion.identity);

					OrangeMobController b = e.GetComponent<OrangeMobController>();
					b.Start();
					b.Initialize(_player, _playerHealth, _score, _level, _difficulty);
					b.transform.parent = Oranges.transform;

					okay = -1;
				}
				else
					okay--;
			}

			if (okay == 0)
				Debug.Log("Failed instantiating WOrange");
			else
			{
				BluesNo++;
				UpdateText();
			}
		}
		else
		{
			posX = UnityEngine.Random.Range(-100, +100);
			posZ = UnityEngine.Random.Range(-100, +100);

			GameObject e = Instantiate(GRed, new Vector3(posX, 1, posZ), Quaternion.identity);
			e.GetComponent<RedMobController>().Start();
		}
	}

	void CGreen()
	{
		if (GreensNo >= 7)
			return;

		int okay = 10;
		int d = 80 + 20 * (int) (TimeSystem.gameTime / 60);
		float posX, posZ;
		while (okay > 0)
		{
			posX = _playerTr.position.x + UnityEngine.Random.Range(-d, d);
			posZ = _playerTr.position.z + UnityEngine.Random.Range(-d, d);

			if (posX > -600 && posZ < 600 && posZ > -600 && posZ < 600 && IsFarFromPlayer(posX, posZ, 50))
			{
				GameObject e = Instantiate(GGreen, new Vector3(posX, 1, posZ), Quaternion.identity);
				GreenMobController r = e.GetComponent<GreenMobController>();
				r.Start();
//				r.Initialize (player, playerHealth, score, level, difficulty, g_greenMissile);
				r.transform.parent = Greens.transform;
				okay = -1;
			}
			else
				okay -= 1;
		}

		if (okay == 0)
			Debug.Log("Failed spawning after 10 tries @CGreen");
		else
		{
			GreensNo++;
			UpdateText();
		}
	}

	void SpawnBoss01()
	{
		float posX = _playerTr.position.x - 50;
		float posZ = _playerTr.position.z - 50;
		GameObject e = Instantiate(GBoss01, new Vector3(posX, 3, posZ), Quaternion.identity);
		Boss01Controller b = e.GetComponent<Boss01Controller>();
		b.Initialize(_player, _playerHealth, _score, _level, _difficulty, GWind);
		e.transform.parent = Bosses.transform;
	}

	void SpawnBoss02()
	{

		float posX = _playerTr.position.x - 50;
		float posZ = _playerTr.position.z - 50;
		GameObject e = Instantiate(GBoss02, new Vector3(posX, 3, posZ), Quaternion.identity);
		Boss02Controller b = e.GetComponent<Boss02Controller>();
		b.Initialize(_player, _playerHealth, _score, _level, _difficulty);
		e.transform.parent = Bosses.transform;
	}

	void SpawnBoss03()
	{

		float posX = _playerTr.position.x - 50;
		float posZ = _playerTr.position.z - 50;
		GameObject e = Instantiate(GBoss03, new Vector3(posX, 3, posZ), Quaternion.identity);
		Boss03Controller b = e.GetComponent<Boss03Controller>();
		b.Initialize(_player, _playerHealth, _score, _level, _difficulty);
		e.transform.parent = Bosses.transform;
	}

	private void SpawnBoss04()
	{
		float posX = _playerTr.position.x - 50;
		float posZ = _playerTr.position.z - 50;
		GameObject e = Instantiate(GBoss04, new Vector3(posX, 3, posZ), Quaternion.identity);
		Boss04Controller b = e.GetComponent<Boss04Controller>();
		b.Initialize(_player, _playerHealth, _score, _level, _difficulty);
		e.transform.parent = Bosses.transform;
	}

	void CRed()
	{
		int okay = 10;
		int d = 80 + 20 * (int) (TimeSystem.gameTime / 60);
		float posX = 0, posZ = 0;
		float angleRadians = UnityEngine.Random.Range(0, 6.28f);
		float distance = UnityEngine.Random.Range(50, 60);
		while (okay > 0)
		{
			foreach (var a in FindObjectsOfType<PlayerController>())
			{
				_playerTr = a.transform;
				posX = _playerTr.position.x + (float) (Math.Cos(angleRadians) * distance);
				posZ = _playerTr.position.z + (float) (Math.Sin(angleRadians) * distance);

				if (IsFarFromPlayer(posX, posZ, 50))
				{
					GameObject e = Instantiate(GRed, new Vector3(posX, -0.23f, posZ), Quaternion.identity);
					RedMobController r = e.GetComponent<RedMobController>();
					r.Start();
					r.Initialize(_player, _playerHealth, _score, _level, _difficulty, false);
					r.transform.parent = Reds.transform;
					okay = -1;
				}
				else
					okay -= 1;
			}
			
		}

		if (okay == 0)
			Debug.Log("Failed spawning after 10 tries @CREd");
		else
		{
			RedsNo++;
			UpdateText();
		}
	}
	
	void CRedShotgun()
	{
		int okay = 10;
		int d = 80 + 20 * (int) (TimeSystem.gameTime / 60);
		float posX = 0, posZ = 0;
		float angleRadians = UnityEngine.Random.Range(0, 6.28f);
		float distance = UnityEngine.Random.Range(50, 60);
		while (okay > 0)
		{
			foreach (var a in FindObjectsOfType<PlayerController>())
			{
				_playerTr = a.transform;
				posX = _playerTr.position.x + (float) (Math.Cos(angleRadians) * distance);
				posZ = _playerTr.position.z + (float) (Math.Sin(angleRadians) * distance);

				if (IsFarFromPlayer(posX, posZ, 50))
				{
					GameObject e = Instantiate(GRedSplash, new Vector3(posX, -0.23f, posZ), Quaternion.identity);
					RedMobController r = e.GetComponent<RedMobController>();
					r.Start();
					r.Initialize(_player, _playerHealth, _score, _level, _difficulty, true);
					r.transform.parent = Reds.transform;
					okay = -1;
				}
				else
					okay -= 1;
			}
			
		}

		if (okay == 0)
			Debug.Log("Failed spawning after 10 tries @CREd");
		else
		{
			RedsNo++;
			UpdateText();
		}
	}

	private bool IsFarFromPlayer(float posX, float posZ, int dist)
	{
		bool answer = false;
		foreach (var a in FindObjectsOfType<PlayerController>())
			_playerTr = a.transform;
			if (Vector2.Distance(new Vector3(posX + _playerTr.position.x, 0, posZ + _playerTr.position.z), _playerTr.position) > dist)
						answer = true;
		return answer;
	}

	public void SpawnMany(string name, int repeats)
	{
		SpawnStop(name);
		for (int i = 0; i < repeats; i++)
			Invoke(name, 0);
	}

	public void SpawnRepeating(string name, float repeatTime)
	{
		SpawnStop(name);
		InvokeRepeating(name, 0, repeatTime);
	}

	public void SpawnStop(string name)
	{
		CancelInvoke(name);
	}

	public void SpawnBoss(int i)
	{
		if (i == 1)
			SpawnBoss01();
		else if (i == 2)
			SpawnBoss02();
		else if (i == 3)
			SpawnBoss03();
		else if (i == 4)
			SpawnBoss04();
		else
			throw new Exception("Unknown boss");
	}

	public void RemoveReds()
	{
		int i, c = 0, count = Reds.childCount;
		for (i = count - 1; i >= 0; i--)
		{
			Transform o = Reds.GetChild(i);
			Destroy(o.gameObject, 1.0f);
			c++;
		}
		//Debug.Log("Destroyed: " + c + " reds");

		RedsNo = count - c;

		UpdateText();
	}

	public void RemoveBlues()
	{
		int j, c = 0, count = Blues.childCount;
		for (j = count - 1; j >= 0; j--)
		{
			Transform o = Blues.GetChild(j);
			Destroy(o.gameObject, 0f);
			c++;
		}
		// Debug.Log("Destroyed: " + c + " blues");

		BluesNo = count - c;
		UpdateText();
	}


	public void RemoveGreens()
	{
		int j, c = 0, count = Greens.childCount;
		for (j = count - 1; j >= 0; j--)
		{
			Transform o = Greens.GetChild(j);
			Destroy(o.gameObject, 0f);
			c++;
		}
		// Debug.Log("Destroyed: " + c + " greens");

		GreensNo = count - c;
		UpdateText();
	}

	public void RemoveBlacks()
	{
		int j, c = 0, count = Blacks.childCount;
		for (j = count - 1; j >= 0; j--)
		{
			Transform o = Blacks.GetChild(j);
			Destroy(o.gameObject, 0f);
			c++;
		}

		BlacksNo = count - c;
		UpdateText();
	}

	public void RemoveOranges()
	{
		int j, c = 0, count = Oranges.childCount;
		for (j = count - 1; j >= 0; j--)
		{
			Transform o = Oranges.GetChild(j);
			Destroy(o.gameObject, 0f);
			c++;
		}

		OrangesNo = count - c;
		UpdateText();
	}
	
	public void UpdateText()
	{
/*
		Pop.text = "Pop: " +
		           "<color=#FF0000>" + "[" + RedsNo + "]" + "</color> " +
		           "<color=#0000FF>" + "[" + BluesNo + "]" + "</color> " +
		           "<color=#00FF00>" + "[" + GreensNo + "]" + "</color> " +
		           "<color=#000000>" + "[" + BlacksNo + "]" + "</color> " +
		           "<color=#FFFFFF>" + "[" + DifficultySystem.bossesBeaten + "]" + "</color> " +
		           "<color=#FF0000>" + "{" + LCno + "}" + "</color> " +
		           "<color=#0000FF>" + "{" + ECno + "}" + "</color>";
		           */
	}

	public void RemoveAmmo()
	{
		for (int i = 0; i < 5; i++)
		{
			_oBlackHole[i].tag = "BH-FREE";
			_oBlackHole[i].transform.Translate(Vector3.down * 10000f);
		}
	}
}
