using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class TimeSystem : MonoBehaviour {

    public Text timeLabel;
	// public Text FPS;
    public float continuedTime = 110f;
    public static float time = 0f;
    public static float gameTime = 0.0f;
    public static bool travels = false;

	public static string fileName;
	//public static StreamWriter writer;

	PlayerHealth ph;
	EnergySystem es;
	PlayerController pc;

	string s;

	private PlayerController player;

	void Start ()
	{
		player = FindObjectOfType<PlayerController>();
		gameTime = 0;
        time = continuedTime;
	    if (time >= 100)
		    DifficultySystem.teleportToStage = 2;
        if (time >= 200)
            DifficultySystem.teleportToStage = 3;
		if (time >= 300)
			DifficultySystem.teleportToStage = 4;
		if (time >= 400)
			DifficultySystem.teleportToStage = 5;
	    if (time >= 500)
		    DifficultySystem.teleportToStage = 6;
	    if (time >= 600)
		    DifficultySystem.teleportToStage = 7;

	    if (time >= 200)
		    DifficultySystem.bossesBeaten = DifficultySystem.teleportToStage - 2;
	    
		if (time > 5f) {
            travels = true; // automatically if time is after the intro :D :P

		}
		
		fileName = "SFG-" + System.DateTime.Now.Year + "-" + System.DateTime.Now.Month + "-" + System.DateTime.Now.Day + "-" + System.DateTime.Now.Hour + "-" + System.DateTime.Now.Minute + ".txt";
		// writer = File.CreateText (fileName);

		ph = FindObjectOfType<PlayerHealth>();
		es = FindObjectOfType<EnergySystem>();
		pc = FindObjectOfType<PlayerController>();
    }
	
	// Update is called once per frame
	private void Update ()
    {
        if (PlayerHealth.isDead)
            return;

		gameTime += Time.deltaTime;
        if (travels && Time.time > 4.9f)
        {
            travels = false;
            gameTime = time;
			LevelingSystem.experience = gameTime * (3 + 2 * DifficultySystem.teleportToStage);
        }

		// if (Time.frameCount % 30 == 0) System.GC.Collect ();

	    /*
		if (Time.frameCount % 60 == 0) {
			s = string.Empty + gameTime + " " +
			Time.deltaTime + " " +
			LevelingSystem.level + " " +
			ph.CurrentHP + " " +
			ph.MaxHealth + " " +
			es.Energy + " " +
			pc.bonusSpeed / 2 + " " +
			player.bonus_bulletspeed / 2 + " " +
			ph.BonusHP / 2 + " " +
			ph.bonusHPR / 2 + " " +
			UpgradeSystem.count + " " + // LOS
			player.points_damage / 2 + " " +
			player.points_manaregen + " " +
			player.points_manamax + " " +
				SpawnController.RedsNo + " " + SpawnController.BluesNo + " " + SpawnController.ECno + " " + SpawnController.LCno + "\n";
			writer.Write (s);
		}
*/
        timeLabel.text = "T: +" + (int)(gameTime / 60) + ":" + (gameTime % 60).ToString("00");
        
    }
}
