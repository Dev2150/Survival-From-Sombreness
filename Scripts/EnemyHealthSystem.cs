using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthSystem : MonoBehaviour {

    static string s = null;
    private static Slider slider;
    static float timer = 0f;
    static MobController e;
	static Boss01Controller f;
    static Image i_enemyHeart, i_fillHP, i_bG;
    static Text t_enemyName;
    private static bool dead;

    void Start () {

        i_bG = GameObject.Find("EnemyBG").GetComponent<Image>();
        i_enemyHeart = GameObject.Find("EnemyHeart").GetComponent<Image>();
        i_fillHP = GameObject.Find("FillEnemyHP").GetComponent<Image>();
        t_enemyName = GameObject.Find("EnemyName").GetComponent<Text>();
        slider = GameObject.Find("EnemySlider").GetComponent<Slider>();

        i_bG.color = Color.clear;
        t_enemyName.color = Color.clear;
        i_enemyHeart.color = Color.clear;
        i_fillHP.color = Color.clear;

    }
	
	void Update () {
        timer -= Time.deltaTime;

        if (dead || timer <= 0f)
        {
            try
            {
                t_enemyName.color = Color.Lerp(t_enemyName.color, Color.clear, 0.05f);
                i_enemyHeart.color = Color.Lerp(i_enemyHeart.color, Color.clear, 0.05f);
                i_fillHP.color = Color.Lerp(i_fillHP.color, Color.clear, 0.05f);
                i_bG.color = Color.Lerp(i_bG.color, Color.clear, 0.1f);
            }
            catch { Debug.Log("Error with updating Enemy HUD"); }
        }
        else if (e != null)
        {
            slider.value = e.currHealth;
            slider.maxValue = e.maxHealth;
        }
	}

    public static void Put(MobController mB)
    {
		if (mB is Boss01Controller) {
			// slider.value = 5f;
			t_enemyName.color = new Color(230, 255, 255);
			i_enemyHeart.color = new Color(255, 255, 255);
			i_fillHP.color = new Color(200, 255, 200);
			i_bG.color = new Color(255, 255, 255, 10f);
			e = mB;
			timer = 3f;
			t_enemyName.text = "~Boss~";
			dead = false;
		}
		if (mB is Boss02Controller) {
			// slider.value = 5f;
			t_enemyName.color = new Color(255, 255, 255);
			i_enemyHeart.color = new Color(255, 69, 0);
			i_fillHP.color = new Color(255, 70, 0);
			i_bG.color = new Color(255, 255, 255, 10f);
			e = mB;
			timer = 3f;
			t_enemyName.text = "~Boss~";
			dead = false;
		}
		if (mB is Boss03Controller) {
			t_enemyName.color = new Color(255, 255, 255);
			i_enemyHeart.color = new Color(150, 255, 255);
			i_fillHP.color = new Color(0, 255, 230);
			i_bG.color = new Color(255, 255, 255, 255f);
			e = mB;
			timer = 3f;
			t_enemyName.text = "~Boss~";
			dead = false;
		}
	    if (mB is Boss04Controller) {
		    t_enemyName.color = new Color(15, 15, 15);
		    i_enemyHeart.color = new Color(15, 15, 15);
		    i_fillHP.color = new Color(255, 255, 255);
		    i_bG.color = new Color(0, 0, 0);
		    e = mB;
		    timer = 3f;
		    t_enemyName.text = "~Boss~";
		    dead = false;
	    }
		
        else if (mB.Dead)
        {
			slider.value = (e is Boss01Controller) ? 5f : 0f;
            i_bG.color = new Color(0f, 0f, 0f, 50f);
            dead = true;
        }
        else if (mB is RedMobController)
        {
            s = RedMobController.nombre;
            t_enemyName.color = new Color(255, 230, 230);
            i_enemyHeart.color = new Color(255, 118, 118);
            i_fillHP.color = new Color(255, 0, 0);
            i_bG.color = new Color(255, 255, 255, 10f);
            e = mB;
            timer = 3f;
            t_enemyName.text = s;
            dead = false;

        }
        else if (mB is BlueMobController)
        {
            s = BlueMobController.nombre;
            t_enemyName.color = new Color(230, 230, 255);
            i_enemyHeart.color = new Color(0, 118, 255);
            i_fillHP.color = new Color(0, 5, 53);
            i_bG.color = new Color(255, 255, 255, 10f);
            e = mB;
            timer = 3f;
            t_enemyName.text = s;
            dead = false;
        }
		else if (mB is GreenMobController)
		{
			s = GreenMobController.nombre;
			t_enemyName.color = new Color(230, 255, 230);
			i_enemyHeart.color = new Color(230, 255, 230);
			i_fillHP.color = new Color(230, 255, 230);
			i_bG.color = new Color(0, 200, 0, 10f);
			e = mB;
			timer = 3f;
			t_enemyName.text = s;
			dead = false;
		}
        else if (mB is BlackMobController)
        {
            s = "Black Hole Gen";
            t_enemyName.color = new Color(15, 15, 15);
            i_enemyHeart.color = new Color(15, 15, 15);
            i_fillHP.color = new Color(0, 0, 0);
            i_bG.color = new Color(255, 255, 255); 
            e = mB;
            timer = 3f;
            t_enemyName.text = s;
            dead = false;
        }
	    else if (mB is OrangeMobController)
	    {
		    s = "Solar Stormer";
		    t_enemyName.color = new Color(255, 255, 255);
		    i_enemyHeart.color = new Color(255, 69, 0);
		    i_fillHP.color = new Color(255, 70, 0);
		    i_bG.color = new Color(255, 255, 255); 
		    e = mB;
		    timer = 3f;
		    t_enemyName.text = s;
		    dead = false;
	    }
    }
}
