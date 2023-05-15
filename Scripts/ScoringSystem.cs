using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoringSystem : MonoBehaviour {

    private float score = 0;
	public static float scoreUpdateCDMax = 0.333f;
    private float Score {
        get
        {
            return score;
        }
        set
        {
            return;

            score = value;
            if (scoreUpdateCooldown < 0)
            {
                scoreLabel.text = score.ToString("00");
				scoreUpdateCooldown = scoreUpdateCDMax;
            }
            scoreUpdateCooldown -= Time.deltaTime;
        }
    }
    public int scoreOverTime;
    private Text scoreLabel;
    private float scoreUpdateCooldown;

    private void Awake()
    {
        // scoreLabel = GameObject.Find("Score").GetComponent<Text>();
    }

    void Update () {
        if (PlayerHealth.isDead)
            return; 
        Score += scoreOverTime * Time.deltaTime;
    }

    internal void getScore(float score)
    {
        Score += score;
    }
}
