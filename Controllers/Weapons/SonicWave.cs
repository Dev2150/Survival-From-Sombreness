using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SonicWave : EnemyMissile
{
    private bool approached = false;
    
    public static float damage = 20f;
    public static float speed = 100f;
    AudioSource e;

	public PlayerController player;
	public PlayerHealth playerHealth;
    private float distToBlackHole;
    GameObject[] o_blackHole;

    void Start()
	{
		player = FindObjectOfType<PlayerController> ();
		e = GetComponent<AudioSource>();
		//transform.rotation *= qdr;
		if (PlayerHealth.isDead) 
			speed = Mathf.Lerp(speed, 0, 0.1f);

        o_blackHole = new GameObject[5];
        for (int i = 0; i < 5; i++)
            o_blackHole[i] = GameObject.Find("BH" + i);

    }


	protected void OnTriggerEnter (Collider col)
	{
		if (col.gameObject.CompareTag("Player"))
		{
			playerHealth.takeDamage(damage);
			// Destroy(gameObject);
		}
	}

    private void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime, Space.Self);


        
        for (int j = 0; j < 5; j++)
        {
            distToBlackHole = Vector3.Distance(o_blackHole[j].transform.position, transform.position);
            if (distToBlackHole < 250)
            {
                transform.Translate(1000f * (o_blackHole[j].transform.position - transform.position) / (distToBlackHole * distToBlackHole) * Time.deltaTime);
                if (distToBlackHole < 30)
                    Destroy(this);
            }
        }



        

        float i = Vector3.Distance(transform.position, player.transform.position);
        if (i < speed * 2.5f)
        {
            if (!approached)
            {
                approached = true;
                e.Play();
            }
            // else e.volume = -i / 400f + 1;
        }

       
    }
}
