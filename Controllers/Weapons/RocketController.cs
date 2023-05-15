using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketController : MonoBehaviour {

    public float bonusDamage = 0f;
    public float speed = 10f;
    public float bonusSpeed = 0f;
    private float _currentSpeed;
	public GameObject rocketBlast;

    private GameObject[] _oBlackHole;
    private float _distToBlackHole;
	private Vector3 _accumulatedSpeed;
	private PlayerController player;

	void Start ()
	{
		player = FindObjectOfType<PlayerController>();
        _oBlackHole = new GameObject[5];
        for (int i = 0; i < 5; i++)
            _oBlackHole[i] = GameObject.Find("BH" + i);

        _currentSpeed = speed;
		_accumulatedSpeed = Vector3.zero;

		GetComponent<Rigidbody>().velocity = transform.forward * speed * (1 + player.BonusBulletspeed / 10);
		GetComponent<Rigidbody> ().AddForce (2 * transform.forward * speed * (1 + player.BonusBulletspeed / 10), ForceMode.VelocityChange);
    }

    private void Update()
    {
        for (int i = 0; i < 5; i++)
        {
            _distToBlackHole = Vector3.Distance(_oBlackHole[i].transform.position, transform.position);
            if (_distToBlackHole < 250)
            {
                transform.Translate(3000f * (_oBlackHole[i].transform.position - transform.position) / (_distToBlackHole * _distToBlackHole) * Time.deltaTime);
                if (_distToBlackHole < 30)
                    Destroy(this);
            }
        }
    }
    
	void OnTriggerEnter (Collider collision) {
		if (collision.gameObject.CompareTag("Terrain") || collision.gameObject.CompareTag("Enemy")) {
			Instantiate (rocketBlast, transform.position, Quaternion.identity);
			Destroy (this);
		}
	}
}
