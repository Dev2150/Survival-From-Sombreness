using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergySystem : MonoBehaviour {

    private Slider energySlider;
    private PlayerController player;
    
    private float energy = 100f;
    public float Energy
    {
        get
        {
            return energy;
        }
        set
        {
            if (value < maxEnergy)
            {
                energy = value;
            }
            else
                energy = maxEnergy;

            energySlider.value = Energy;
			energySlider.GetComponent<RectTransform> ().sizeDelta = new Vector2 ((1 + 0.2f * player.PointsManamax) * 100, 40);
        }
    }

    public void getEnergy(int v)
    {
        Energy += v;
    }

    public bool isEnough(float v)
    {
        if (Energy > v)
        {
            Energy -= v;
            return true;
        }
        return false;
    }

	private float maxEnergy = 100;
    public float energyRegenCoef = 1.0f;
    public float energyRegenArbalest = 1f;


    void Start ()
    {
        player = FindObjectOfType<PlayerController>();
        energySlider = GameObject.Find("EnergySlider").GetComponent<Slider>();
	    player = FindObjectOfType<PlayerController>();
	}
	

	// Update is called once per frame
	void Update ()
	{

        if (PlayerHealth.isDead)
            return;
        regenEnergy();	    
	}

    private void regenEnergy()
    {
		Energy += energyRegenArbalest * energyRegenCoef * (1 + 0.2f * player.PointsManaregen) * Time.deltaTime;

        if (Energy > maxEnergy)
            Energy = maxEnergy;
    }
}
