using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConcreteController : MonoBehaviour {

	public static float speed = 10f;
	public static float bonusSpeed = 0f;

	private GameObject[] o_blackHole;
	private float distToBlackHole;
	private Vector3 accumulatedSpeed;
	private PlayerController player;
	
	void Start()
	{
		player = FindObjectOfType<PlayerController>();
		o_blackHole = new GameObject[5];
		for (int i = 0; i < 5; i++)
			o_blackHole[i] = GameObject.Find("BH" + i);
		
		GetComponent<Rigidbody>().velocity = transform.forward * speed * (1 + player.BonusBulletspeed / 10);
	}

	private void Update()
	{
		for (int i = 0; i < 5; i++)
		{
			distToBlackHole = Vector3.Distance(o_blackHole[i].transform.position, transform.position);
			if (distToBlackHole < 250)
			{
				transform.Translate(50000f * (o_blackHole[i].transform.position - transform.position) / (distToBlackHole * distToBlackHole) * Time.deltaTime);
				if (distToBlackHole < 100) {
					o_blackHole[i].transform.position = Vector3.down * 1000f;
					o_blackHole[i].tag = "BH-FREE";
					Destroy(this);
				}
			}
		}
	}

	void OnTriggerEnter (Collider collision) {
		if (collision.gameObject.tag == "ESonicWave") {
			Destroy (collision.gameObject);
			Destroy (this);
		}
	}
}
