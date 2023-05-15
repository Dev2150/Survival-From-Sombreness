using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DifficultySystem : MonoBehaviour {

    /*
     * SpawnW - Spawn across all the world
     * SpawnC - Spawn close to the player
     */
	
	int stage;
	public static int bossesBeaten;

	public bool doublePower;
    public bool debugging;
    public bool close;
    public Material[] sky;
    public Material mySky;
    public Color skyColor;
    public Transform e;

	public static float bossDeathTimer = -999999f;
	public static bool[] bossDead = new bool[10];

    public RedMobController red;
    public BlueMobController blue;
    public RedDefaultGunController bulletRed;

    private SpawnController spawnSys; 
    public ScoringSystem Score;
    public MusicControl Music;
    public Text difficultyLabel, descriptionLabel;

    static bool[] swEvents;

    bool[] swSkies;
    private float introTimer = 5f;
    private float basicOscillationTimer;
    private float  descriptIndex;
    public static float timeMusicContinued;
    public Color descriptionColor;
    float formerState, currentState;
    private Color currentColor = Color.white;

	private LevelingSystem levelSys;
	private ScoringSystem scorSys;
	private PlayerHealth _playerHealth;
    public static int teleportToStage = 1;

    public static bool nuke_explodable = false;
    private bool bossHealthReward;

    private void Awake()
    {
		bossesBeaten = teleportToStage - 1;
        spawnSys = FindObjectOfType<SpawnController>();
		levelSys = FindObjectOfType<LevelingSystem>();
		scorSys = FindObjectOfType<ScoringSystem>();
		_playerHealth = FindObjectOfType<PlayerHealth>();

        swEvents = new bool[30];
        for (int i = 0; i < 30; i++)
            swEvents[i] = false;
        swSkies = new bool[7];
        for (int i = 0; i < 7; i++)
            swSkies[i] = false;

        //bossesBeaten = (int)Mathf.Floor(TimeSystem.time / 120);
        //bossDeathTimer = TimeSystem.time - bossesBeaten * 120;
        bossHealthReward = false;
    }

    void Update()
    {
        if (PlayerHealth.isDead)
            return;

        timersUpdate();



        if (!swEvents[0] && TimeSystem.gameTime > 60 * 0 + 0f)
        {
            ChangeGears(0, "", 0, 0, "Intro", "Primary Goal : Survive 20 minutes!\nWatch your back!");

            RenderSettings.ambientIntensity = 2;

            spawnSys.SpawnRepeating("WEnergy", 4.5f / 150f);
            spawnSys.SpawnRepeating("WLife", 4.5f / 25f);
			spawnSys.SpawnRepeating("WPower", 4.5f / 20f);

            //RedMobController.aggroDistance = 0f;
            RedMobController.exp = 15f;
            RedMobController.speed = 40f;
            RedMobController.nombre = "Marksman";

            UpdateSkyColor(new Color(1, 1, 1));

            // MusicControl.PlayAmbient();
        }

        if (!swEvents[1] && bossesBeaten == 0 && TimeSystem.gameTime > 04.50f)
        {
            ChangeGears(1, "Genesis", 1, 1, "UCSNight2", "Level 1");

            spawnSys.SpawnRepeating("WLife", 30f);
            spawnSys.SpawnRepeating("WEnergy", 10f);
	        spawnSys.SpawnRepeating("WPower", 15f);
            spawnSys.SpawnRepeating("CRed", 2f); // TODO

            LevelingSystem.experience = 0;
            BlueMobController.exp = 100;
            BlueMobController.score = 200f;
            SonicWave.speed = 100f;
            SonicWave.damage = 15.0f;
            RedMobController.bulletTimeCloseness = 2f;
            RedDefaultGunController.speed = 50;
            RedDefaultGunController.damage = 5.0f;
            //RedMobController.aggroDistance = 1.00f * RedMobController.bulletTimeCloseness * RedDefaultGunController.speed;
            RedMobController.speed *= 1.1f;


            UpdateSkyColor(new Color(1, 100f / 250, 100 / 250f));
        }
        if (!swEvents[2] && bossesBeaten == 0 && TimeSystem.gameTime > 4.50f + 34) //27
        {
            ChangeGears(2, "Marksmen II", 1, 2, "The Marksmen can shoot you");
            spawnSys.SpawnRepeating("CRed", 3f);
            RedMobController.bulletTimeCloseness = 1.3f;
            //RedMobController.aggroDistance = 1.00f + RedMobController.bulletTimeCloseness * RedDefaultGunController.speed;
            RedMobController.shooting = true;

            RedMobController.speed *= 1.1f;
            RedMobController.nombre = "Marksmen II";

            UpdateSkyColor(new Color(1, 90f / 250, 90 / 250f));
            RedMobController.exp *= 1.1f;
            RedMobController.score *= 1.1f;
        }
        if (!swEvents[3] && bossesBeaten == 0 && TimeSystem.gameTime > 4.50f + 58)
        {
            ChangeGears(3, "Marksmen III", 1, 3, "They...can see you");
	        spawnSys.SpawnRepeating("CRed", 4f); // TODO
            RedMobController.bulletTimeCloseness = 1.4f;
            RedDefaultGunController.speed = 65;
	        //RedMobController.aggroDistance = 1.00f + RedMobController.bulletTimeCloseness * RedDefaultGunController.speed;
            RedDefaultGunController.damage = 5.5f;
            RedMobController.speed *= 1.2f;

            RedMobController.nombre = "Marksmen III";
            spawnSys.SpawnRepeating("CRed", 2.0f);

            UpdateSkyColor(new Color(1, 80f / 250, 80 / 250f));
            RedMobController.exp *= 1.1f;
            RedMobController.score *= 1.1f;
        }

	    /*if (!swEvents[4] && bossesBeaten == 0 && TimeSystem.gameTime > 4.50f + 103)
	    {
		    spawnSys.SpawnStop("CRed");
		    spawnSys.SpawnRepeating("CRedShotgun", 5f);
		    RedMobController.nombre = "MarksMen III";
		    //RedMobController.super = true;
		    ChangeGears(4, RedMobController.nombre, 1, 4, "New species have evolved!");
		    RedMobController.bulletTimeCloseness = 2.0f;
		    RedDefaultGunController.speed = 80;
		    // red.aggroDistance = 1.125f * red.bulletTimeCloseness * bulletRed.speed;
		    //RenderSettings.ambientSkyColor = Color.white;

		    RedDefaultGunController.damage = 7.0f;
		    RedMobController.speed *= 1.1f;

		    RedMobController.exp *= 1.1f;
		    RedMobController.score *= 1.1f;

		    UpdateSkyColor(new Color(1, 60f / 250, 60 / 250f));

		    //e.position.y += 50f;
	    }*/
	    
        if (teleportToStage <= 2)
        {
			stage = 4;
            if (!swEvents[stage] && bossesBeaten == 0 && TimeSystem.gameTime > 4.50f + 115) //157 // <- just teleport in time at start to get to the 1st boss fight

			{
                //RedMobController.nombre = "";
                // spawnSys.SpawnRepeating("CRed", 2.5f);
                ChangeGears(stage, "Boss Anticipation", 1, 0, "Boss01", "Get ready"); 
                // RedMobController.bulletTimeCloseness = 2.00f;
                // RedDefaultGunController.speed = 80;
                //red.aggroDistance = 1.25f * red.bulletTimeCloseness * bulletRed.speed;

                spawnSys.SpawnStop("CRed");
                spawnSys.SpawnStop("CRedShotgun");
	            spawnSys.SpawnRepeating("WPower", 15f);

                //RenderSettings.ambientSkyColor = Color.white;
                // updateSkyColor(new Color(1, 60f / 250, 60 / 250f));

            }

	        stage++;
            if (!swEvents[stage] && bossesBeaten == 0 && TimeSystem.gameTime > 4.50f + 115 + 21)
            {
                ChangeGears(stage, "Charlie Winder", 1, -6, "Get rekt");

                spawnSys.RemoveReds();
                spawnSys.SpawnBoss(1);

            }

	        stage++;
            if (!swEvents[stage] && bossesBeaten == 1)
            {
                ChangeGears(stage, "Charlie Winder", 0, 0, "");
				playBossDeath();
	

                //MusicControl.changeTo("bossDeath");

            }

	        stage++;
            if (!swEvents[stage] && bossesBeaten == 1 && bossDeathTimer >= 15)
            {
                RenderSettings.skybox = sky[6];
                BlueMobController.nombre = "Sonicsnipers O";
                ChangeGears(8, "Genesis", 1, 8, "Follow", "Level 2");
                spawnSys.SpawnRepeating("WBlue", 60f);
                spawnSys.SpawnRepeating("CRedShotgun", 5f);
	            spawnSys.SpawnRepeating("CRed", 3f);
                RenderSettings.ambientSkyColor = Color.blue;

                UpdateSkyColor(new Color(150f / 255, 150f / 255, 1));
                RedMobController.exp *= 1.1f;
                RedMobController.score *= 1.1f;
            }

	        stage++;
            if (!swEvents[stage] && bossesBeaten == 1 && bossDeathTimer >= 15 + 31)
            { //89 : 242
                BlueMobController.nombre = "SonicSnipers I";
                ChangeGears(9, BlueMobController.nombre, 1, 9, "Sonicsnipers can shoot now");
                //spawnSys.SpawnRepeating("CRed", 5f);
                BlueMobController.speed *= 1.2f;
                SonicWave.speed = 225f;
                SonicWave.damage = 25.0f;
                BlueMobController.shooting = true;
                BlueMobController.bulletTimeCloseness = 7.00f;

                UpdateSkyColor(new Color(80f / 255, 80f / 255, 1));
                RedMobController.exp *= 1.1f;
                BlueMobController.exp *= 1.1f;
                RedMobController.score *= 1.1f;
                BlueMobController.score *= 1.1f;

            }

	        stage++;
            if (!swEvents[stage] && bossesBeaten == 1 && bossDeathTimer >= 15 + 79f)
            {
	            ChangeGears(stage, BlueMobController.nombre, 1, 10, "More powerful sonic launches");
                BlueMobController.nombre = "Sonicsnipers II";
                
                //spawnSys.SpawnRepeating("CRed", 6f);
                BlueMobController.speed *= 1.2f;
                BlueMobController.bulletTimeCloseness = 10.00f;
                SonicWave.damage = 30.0f;
                SonicWave.speed = 250f;

                UpdateSkyColor(new Color(70f / 255, 70f / 255, 1));
                RedMobController.exp *= 1.1f;
                BlueMobController.exp *= 1.1f;
                RedMobController.score *= 1.1f;
                BlueMobController.score *= 1.1f;
            }

	        stage++;
            if (!swEvents[stage] && bossesBeaten == 1 && bossDeathTimer >= 15 + 120f)
            {
                BlueMobController.nombre = "Sonicsnipers III";
                ChangeGears(stage, BlueMobController.nombre, 1, 11, "Killer precision");
                //spawnSys.SpawnRepeating("CRed", 8f);
                BlueMobController.bulletTimeCloseness = 18.00f;
                BlueMobController.sonicWaveCD /= 1.1f;
                BlueMobController.speed *= 1.2f;
                SonicWave.damage = 35.0f;
                SonicWave.speed = 300f;

                RedMobController.exp *= 1.1f;
                BlueMobController.exp *= 1.1f;
                RedMobController.score *= 1.1f;
                BlueMobController.score *= 1.1f;
                UpdateSkyColor(new Color(50f / 255, 50f / 255, 1));
            }
        }


	    if (teleportToStage <= 3)
	    {
		    stage = teleportToStage == 3 ? 4 : 12;
		    if (!swEvents[stage] && (bossesBeaten == 1 && bossDeathTimer >= 15 + 167f || teleportToStage == 3 && TimeSystem.gameTime >= 200f)) {

			    spawnSys.SpawnRepeating("WLife", 30f);
			    spawnSys.SpawnRepeating("WEnergy", 10f);
			    spawnSys.SpawnRepeating("WPower", 15f);
			    
			    RenderSettings.skybox = sky [6];
				ChangeGears (stage, "Boss anticipation", 1, 0, "Boss02", "Dinner time!");

				spawnSys.SpawnStop ("WBlue");
				spawnSys.SpawnStop ("CRed");
				spawnSys.SpawnStop ("CRedShotgun");

				RenderSettings.ambientSkyColor = Color.white;
				// updateSkyColor(new Color(1, 60f / 250, 60 / 250f));
			}
			stage++;
		    if (!swEvents[stage] && (bossesBeaten == 1 && bossDeathTimer >= 25 + 167f || teleportToStage == 3 && TimeSystem.gameTime >= 210f)) {
				nuke_explodable = true;
				ChangeGears (stage, "Cluster Bomber", 1, -12, "Have a nuke!");
				spawnSys.RemoveReds ();
				spawnSys.RemoveBlues ();
				spawnSys.RemoveAmmo();
				spawnSys.SpawnBoss (2);

			}

			stage++;
			if (!swEvents [stage] && bossesBeaten == 2) {
				ChangeGears (stage, "Cluster Bomber", 0, 0, "");
				playBossDeath();
			}

			stage++;
			if (!swEvents [stage] && bossesBeaten == 2 && bossDeathTimer >= 15f) {       
				ChangeGears (stage, "Toxicity event", 1, 14, "Speakers", "Level 3. Toxicity level up!");
				FindObjectOfType<PlayerController> ().AddToxicity (); ///////////////////////
				spawnSys.SpawnRepeating ("CRed", 7f);
				spawnSys.SpawnRepeating ("WBlue", 25f);
				BlueMobController.nombre = "SSS";
				GreenMobController.exp = 75f;
				GreenMobController.score = 75f;

				BlueMobController.shooting = true;
				BlueMobController.bulletTimeCloseness = 7.00f;
				SonicWave.damage = 45.0f;
				SonicWave.speed = 333f;
				UpdateSkyColor(new Color(100f / 255, 1, 100f / 255));
			}

			stage++;
			if (!swEvents [stage] && bossesBeaten == 2 && bossDeathTimer >= 15 + 21f) {
				GreenMobController.nombre = "Antiperceptor";
				spawnSys.SpawnRepeating ("CGreen", 15f);
				ChangeGears (stage, "Genesis", 1, 15, "Antiperceptors are here to facilitate Sonicsniper's hunting");
				GreenMissileController.SpeedUp = 20f;
				GreenMissileController.speed = 150f;
				UpdateSkyColor(new Color(80f / 255, 0.9f, 80f / 255));
			}

			stage++;
			if (!swEvents [stage] && bossesBeaten == 2 && bossDeathTimer >= 15 + 77f) {
				ChangeGears (stage, "Antiperceptors II", 1, 16, "Antiperceptors' range increased");
				GreenMissileController.SpeedUp = 20f;
				GreenMissileController.speed = 175f;
				UpdateSkyColor(new Color(60f / 255, 0.8f, 60f / 255));
			}

			stage++;
			if (!swEvents [stage] && bossesBeaten == 2 && bossDeathTimer >= 15 + 120f) {
				ChangeGears (stage, "Antiperceptors III", 1, 17, "Antiperceptors' attack also give a debuff!");
				GreenMissileController.Debuff = true;
				GreenMissileController.SpeedUp = 20f;
				GreenMissileController.speed = 200f;
				UpdateSkyColor(new Color(40f / 255, 0.6f, 40f / 255));
			}
		}

		if (teleportToStage <= 4) {
			stage = teleportToStage == 3 ? 11 : teleportToStage == 4 ? 4 : 19;
			if (!swEvents[stage] && (bossesBeaten == 2 && bossDeathTimer >= 15 + 175f || teleportToStage == 4 && TimeSystem.gameTime >= 300f)) {
				ChangeGears (stage, "Boss anticipation", 1, 0, "Boss03", "Thrill time!");
				
				GreenMissileController.SpeedUp = 20f;
				GreenMissileController.speed = 150f;
				
				spawnSys.SpawnRepeating("WLife", 30f);
				spawnSys.SpawnRepeating("WEnergy", 10f);
				spawnSys.SpawnRepeating("WPower", 15f);
				
				spawnSys.SpawnStop ("WBlue");
				spawnSys.SpawnStop ("CRed");
				spawnSys.SpawnStop ("CRedShotgun");
				spawnSys.SpawnStop ("CGreen");

				RenderSettings.ambientSkyColor = Color.white;
				// updateSkyColor(new Color(1, 60f / 250, 60 / 250f));
			}

			stage++;
			if (!swEvents [stage] && (bossesBeaten == 2 && bossDeathTimer >= 25 + 175f || teleportToStage == 4 && TimeSystem.gameTime >= 310f)) {
				ChangeGears (stage, "Neutron Blaster", 1, -19, "Have a nice death!");
				spawnSys.RemoveReds ();
			 	spawnSys.RemoveBlues ();
				spawnSys.RemoveGreens ();
				spawnSys.RemoveAmmo();
				spawnSys.SpawnBoss (3);
			}
			
			stage++;
			if (!swEvents [stage] && bossesBeaten == 3) {
				ChangeGears (stage, "Neutron Blaster", 0, 0, "");
				playBossDeath();
			}
			
			stage++;
			if (!swEvents [stage] && bossesBeaten == 3 && bossDeathTimer >= 15f) {
				ChangeGears (stage, "Blackhole event", 1, 21, "ThingsIDo", "Level 4");
				spawnSys.SpawnRepeating ("CRed", 10f);
				spawnSys.SpawnRepeating ("CRedShotgun", 5f);
				spawnSys.SpawnRepeating ("WBlue", 20f);
				spawnSys.SpawnRepeating ("CGreen", 15f);
				spawnSys.SpawnRepeating ("WBlack", 120f);
				SonicWave.speed = 250f;
			}
		}

	    if (teleportToStage <= 5)
	    {
		    stage = teleportToStage == 3 ? 15 : teleportToStage == 4 ? 8 : teleportToStage == 5 ? 4 : 23;
		    if (!swEvents[stage] && (bossesBeaten == 3 && bossDeathTimer >= 15 + 337f || teleportToStage == 5 && TimeSystem.gameTime >= 400f))
		    {			    
			    ChangeGears(stage, "Boss anticipation", 1, 0, "RockAndAwe", " ");
			    spawnSys.SpawnRepeating("WLife", 30f);
			    spawnSys.SpawnRepeating("WEnergy", 10f);
			    spawnSys.SpawnRepeating("WPower", 15f);
			    spawnSys.SpawnStop ("WBlue");
			    spawnSys.SpawnStop ("CRed");
			    spawnSys.SpawnStop ("CRedShotgun");
			    spawnSys.SpawnStop ("CGreen");
			    spawnSys.SpawnStop ("WBlack");
		    }
		    
		    stage++;
		    if (!swEvents [stage] && (bossesBeaten == 3 && bossDeathTimer >= 16 + 337f || teleportToStage == 5 && TimeSystem.gameTime >= 401f)) {
			    ChangeGears (stage, "Speed Demon", 1, -23, "Uu, chase me, chase me!");
			    spawnSys.RemoveReds ();
			    spawnSys.RemoveBlues ();
			    spawnSys.RemoveGreens ();
			    spawnSys.RemoveBlacks ();
			    spawnSys.RemoveAmmo();
			    spawnSys.SpawnBoss(4);
		    }
			
		    stage++;
		    if (!swEvents [stage] && bossesBeaten == 4) {
			    ChangeGears (stage, "Speed Demon", 0, 0, "");
			    playBossDeath();
		    }

		    stage++;
			// if (!swEvents[stage] && (bossesBeaten == 3 && bossDeathTimer >= 255f || teleportToStage == 5 && TimeSystem.gameTime >= 400f))
			if (!swEvents[stage] && bossesBeaten == 4 && bossDeathTimer >= 15f)
		    {
			    ChangeGears(stage, "Napalm Stage", 1, 25, "FireStage", "Level 5");
			    SonicWave.speed = 250f;
			    spawnSys.SpawnStop("WLife");
			    spawnSys.SpawnStop("WEnergy");
			    spawnSys.SpawnRepeating("WOrange", 40f);
			    spawnSys.SpawnRepeating ("WBlue", 20f);
			    spawnSys.SpawnRepeating ("CGreen", 15f);
			    spawnSys.SpawnRepeating ("WBlack", 120f);
			    SonicWave.speed = 250f;
			    BlueMobController.shooting = true;
			    BlueMobController.bulletTimeCloseness = 7.00f;
			    SonicWave.damage = 45.0f;
			    GreenMissileController.SpeedUp = 20f;
			    spawnSys.SpawnRepeating("WLife", 30f);
			    spawnSys.SpawnRepeating("WEnergy", 10f);
			    OrangeMobController.count = 10;
		    }
		    
		    stage++;
		    if (!swEvents[stage] && bossesBeaten == 4 && bossDeathTimer >= 15f + 78f)
		    {
			    ChangeGears(stage, "Elite Solar Stormer", 1, 26, "Level 5");
			    OrangeMobController.count = 14;
			    OrangeMobController.exp *= 1.1f;
		    }

		    stage++;
		    if (!swEvents[stage] && bossesBeaten == 4 && bossDeathTimer >= 15f + 178f)
		    {
			    ChangeGears(stage, "Imperial Solar Stormer", 1, 27, "Level 5");
			    OrangeMobController.count = 20;
			    OrangeMobController.exp *= 1.1f;
		    }
	    }

	    if (!debugging)
            Intro();

        UpdateDescript();
        labelColorOscillation();
        currentColor = Color.Lerp(currentColor, skyColor, Time.deltaTime);
        mySky.SetColor("_Tint", currentColor);

        if (formerState != currentState)
	        FindObjectOfType<PlayerController>().GetComponent<AudioSource>().PlayOneShot(Resources.Load<AudioClip>("Sounds/wonderbeguns"));

        formerState = currentState;
    }

	private void UpdateSkyColor(Color color)
    {
        skyColor = color;
    }

    private void UpdateDescript()
    {
        
        descriptIndex -= Time.deltaTime;
        if (descriptIndex < 2.5f)
        {
            descriptionLabel.color = Color.Lerp(descriptionColor, Color.clear, (2.5f - descriptIndex) / 2.5f);
        }
        else
        {
            descriptionLabel.color = descriptionColor;
        }
    }

    private void delta(string v)
    {
        descriptIndex = 5f;
        descriptionLabel.text = v;
    }

    private void labelColorOscillation()
    {
        if (basicOscillationTimer < 0.1f)
            difficultyLabel.color = Color.white;
        else if (basicOscillationTimer > 0)
        {
            if ((int)(basicOscillationTimer * 6) % 2 == 0)
                difficultyLabel.color = Color.red;
            else
                difficultyLabel.color = Color.white;
        }
        if (basicOscillationTimer > 0)
            basicOscillationTimer -= 1*Time.deltaTime;
    }

    private void timersUpdate()
    {
		bossDeathTimer += Time.deltaTime;
		float incFreq = 0.125f, hpFreq = 0.033f, eT = 5.5f, sT = 8.5f, hT = 11.5f;

		if (0f <= bossDeathTimer && bossDeathTimer <= 15f) {
			for (int i = 0; i < 24; i++)
				if (eT + incFreq * i <= bossDeathTimer && bossDeathTimer <= eT + Time.deltaTime + incFreq * i) {

                        if (i < Math.Max(0, 10)) 
							levelSys.getExperience (bossesBeaten);
						else if (i < 24)
							levelSys.getExperience (10 * bossesBeaten);
						else 
							levelSys.getExperience (100 * bossesBeaten);
				}
			
			if (sT <= bossDeathTimer && bossDeathTimer <= sT + Time.deltaTime)
				ScoringSystem.scoreUpdateCDMax = 0f;

			for (int i = 0; i < 24; i++)
				if (sT + incFreq * i <= bossDeathTimer && bossDeathTimer <= sT + Time.deltaTime + incFreq * i) {
					if (i < 6)
						scorSys.getScore ((float)Math.Pow(2,bossesBeaten - 1));
					else if (i < 16) 
						scorSys.getScore(10 * (float)Math.Pow(2, bossesBeaten - 1));
                    else
						scorSys.getScore (100 * (float)Math.Pow(2, bossesBeaten - 1));
                }

			if (hT <= bossDeathTimer && bossDeathTimer <= hT + Time.deltaTime)
				ScoringSystem.scoreUpdateCDMax = 0.333f;

            if (bossHealthReward)
                return;

			for (int i = 0; i < 33 * 3; i++)
				if (hT + hpFreq * i <= bossDeathTimer && bossDeathTimer <= hT + Time.deltaTime + hpFreq * i) {
					_playerHealth.GetHP(1);

					if (_playerHealth.CurrentHP >= _playerHealth.MaxHealth)
						bossDeathTimer = 15f;
				}
		}
	}

    private void ChangeGears(int evNo, String lblText, int oscillateType, int sOT, string message)
    {
        swEvents[evNo] = true;
        difficultyLabel.text = lblText;
        // Score.scoreOverTime = sOT;
        delta(message);

		if (oscillateType == 1)
			basicOscillationTimer = 2f;
		else if (oscillateType == 0) {
			basicOscillationTimer = 0f;

		}

        currentState = evNo;

    }

    private void ChangeGears(int evNo, String lblText, int oscillateType, int sOT, String music, string message)
    {
        ChangeGears(evNo, lblText, oscillateType, sOT, message);
	    MusicControl.stopAll();
        MusicControl.changeTo(music); //, timeMusicContinued);
    }

    private void Intro()
    {
        if (introTimer > 0)
        {
            introTimer -= Time.deltaTime;

            for (int i = 0; i < 6; ++i)
				if (!swSkies[i] && TimeSystem.gameTime > i * 0.5f)
                {
                    swSkies[i] = true;
                    RenderSettings.skybox = sky[5 - i];
                    if (i == 7)
                    {	
                        RenderSettings.fog = false;
                        RenderSettings.fogMode = FogMode.ExponentialSquared;
                        RenderSettings.fogDensity = 0.0f;
                    }
                }

            float final = 5.0f - 2.5f; // interval of time
			if (introTimer < final) {
				if (RenderSettings.ambientIntensity > 0.8f)
					RenderSettings.ambientIntensity -= 4f * Time.deltaTime / final;
				RenderSettings.fogDensity += Time.deltaTime * 0.05f / final;
			} else
				RenderSettings.ambientIntensity = 0.8f;
        }
    }

    public String Get()
    {
        return difficultyLabel.text;
    }

	private void playBossDeath() {
		MusicControl.stopAll();
		switch (Load.Difficulty) {
		case -2 : 	FindObjectOfType<PlayerController>().GetComponent<AudioSource>().PlayOneShot(Resources.Load<AudioClip>("Sounds/bossDeath")); break;
		case -1 : 	FindObjectOfType<PlayerController>().GetComponent<AudioSource>().PlayOneShot(Resources.Load<AudioClip>("Sounds/bossDeath_2")); break;
		case 0 : 	FindObjectOfType<PlayerController>().GetComponent<AudioSource>().PlayOneShot(Resources.Load<AudioClip>("Sounds/bossDeathHard")); break;
		case +2 : 	FindObjectOfType<PlayerController>().GetComponent<AudioSource>().PlayOneShot(Resources.Load<AudioClip>("Sounds/bossDeathExpert")); break;
		case +4 : 	FindObjectOfType<PlayerController>().GetComponent<AudioSource>().PlayOneShot(Resources.Load<AudioClip>("Sounds/bossDeathRealLife")); break;
		default:	FindObjectOfType<PlayerController>().GetComponent<AudioSource>().PlayOneShot(Resources.Load<AudioClip>("Sounds/bossDeath")); break;
		}
	}
}
