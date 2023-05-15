using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollideNapalm : MonoBehaviour {

	private PlayerHealth health;
	private float timerLightChange, cdLightChange = 0.2f;
	private Light light;
	void Start ()
	{
		health = FindObjectOfType<PlayerHealth>();
		light = GetComponentInChildren<Light>();
	}

	/*private void Update()
	{
		if (timerLightChange > cdLightChange)
		{
			timerLightChange = 0f;
			light.range = Random.Range(0.4f, 0.6f);
		}

		timerLightChange += Time.deltaTime;
	}*/

	private void OnParticleCollision(GameObject other)
	{
		health.takeDamage(10);
	}
}
