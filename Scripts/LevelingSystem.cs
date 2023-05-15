using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class LevelingSystem : MonoBehaviour {

	private DifficultySystem diffSys;
    private EnergySystem energySys;
    private ScoringSystem scoreSys;
    private UpgradeSystem upgSys;
    private PlayerHealth health;
    private Slider xpSlider;
    private Text xpLabel;

    public static float experience;
    public static int level;
    private int lvlDownExperience = 0;
    private int lvlUpExperience = 0;

    private void Awake()
    {
		try { diffSys = FindObjectOfType<DifficultySystem>(); }
		catch { Debug.Log("No UpgradeSystem found"); }

        try { upgSys = FindObjectOfType<UpgradeSystem>(); }
        catch { Debug.Log("No UpgradeSystem found"); }

        try { energySys = FindObjectOfType<EnergySystem>(); }
        catch { Debug.Log("No EnergySystem found"); }

        try { scoreSys = FindObjectOfType<ScoringSystem>(); }
        catch { Debug.Log("No ScoringSystem found"); }

        try { health = FindObjectOfType<PlayerHealth>(); }
        catch { Debug.Log("No HealthSystem found"); }

        try { xpSlider = GameObject.Find("XP Slider").GetComponent<Slider>(); }
        catch { Debug.Log("No game object found"); }

        try { xpLabel = GameObject.Find("XP Label").GetComponent<Text>(); }
        catch { Debug.Log("No game object found"); }

        level = -1;
    }

    void Update () {
        if (experience >= lvlUpExperience)
        {
            level++;
            if (level >= 1)
            {
                scoreSys.getScore(100);
                health.upgradeHP(1f); //2.5f);
                energySys.getEnergy(25);
                upgSys.OneUP(level);
				FindObjectOfType<PlayerController>().GetComponent<AudioSource>().PlayOneShot(Resources.Load<AudioClip>("Sounds/LevelUp"));
                // RenderSettings.fogDensity = RenderSettings.fogDensity / 1.04f; //1.04f;
                // RenderSettings.ambientIntensity += 0.01f;
            }
            experience -= lvlUpExperience;
			xpSlider.maxValue = (30 + level * 5) * (1 + (0.3f + 0.2f * Load.Difficulty) * Convert.ToInt32 (diffSys.doublePower));
            lvlDownExperience += lvlUpExperience;
			lvlUpExperience = (int)xpSlider.maxValue;

        }

        xpSlider.value = experience;
        xpLabel.text = "Level " + level.ToString("0") + " : " + (lvlDownExperience + (int)experience).ToString("00") + " / " + (lvlDownExperience + lvlUpExperience);
    }

    public void getExperience(float v)
    {
        experience += v * UnityEngine.Random.Range(0.75f, 1.25f); 
    }
}
