using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BlackHoleController : MonoBehaviour {

    public float speed = 10f;
    public float floatTimer = 0f;
    AudioSource e;
    string formerTag;

    public PlayerController player;
    public PlayerHealth playerHealth;
    private float distToBlackHole;

    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        e = GetComponent<AudioSource>();
        //transform.rotation *= qdr;
        if (PlayerHealth.isDead)
            speed = Mathf.Lerp(speed, 0, 0.1f);
    }


    protected void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            playerHealth.takeDamage(100f);
        }
    }

    private void Update()
    {
        if (!player)
            return;
        if (PlayerHealth.isDead || player.Paused)
            return;

        transform.Translate(Vector3.forward * (speed * 0.1f * (10 + SpawnController.BlacksNo)) * Time.deltaTime, Space.Self);

        if (formerTag == "BH-FREE" && tag == "BH-BUSY")
        {
            floatTimer = 180f;
        }
        else if (floatTimer < 0)
        {
            transform.position = Vector3.down * 1000f;
            tag = "BH-FREE";
        }

        floatTimer -= Time.deltaTime;
        formerTag = tag;
    }
}
