using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicControl : MonoBehaviour
{
    public static string[] names = { "Intro", "UCSNight2", "Boss01", "Follow", "bossDeath", "Boss02", "Pagorki", "Speakers", "Boss03", "ThingsIDo", "RockAndAwe", "FireStage"};
    public static AudioSource[] list = new AudioSource[names.Length];

    private void Awake()
    {
        for (int i = 0; i < names.Length; i++)
        {
            try { list[i] = GameObject.Find(names[i]).GetComponent<AudioSource>(); }
            catch { Debug.Log("" + names[i] + " not found\n"); }
        }
    }

public static void stopAll()
    {
        for (int i = 0; i < list.Length; ++i)
        {
            if (names[i] != "AmbientNight")
                list[i].Stop();
        }
    }

    public static void changeTo(string music)
    {
        stopAll();
        for (int i = 0; i < names.Length; i++) {
            if (music == names[i])
            {
                list[i].Play();
                break;
            }
        }
            
    }

    public static void PlayAmbient()
    {
        GameObject.Find("AmbientNight").GetComponent<AudioSource>().Play();
    }
}