using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class LifeCube : MonoBehaviour {

    private PlayerHealth health;
	NavMeshAgent agent;
	static NavMeshHit hit;
    Animation anim;

	void Start() {
		agent = GetComponent<NavMeshAgent> ();
		NavMesh.SamplePosition(transform.position, out hit, 10000, 1);
		agent.Warp (hit.position);
        anim = GetComponent<Animation>();
        anim["Power"].speed = 0.1f;
        transform.Translate(Vector3.up * 1f);
		health = FindObjectOfType<PlayerHealth>();
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            health.GetHP(10);
            health.BaseMaxHP += 0.001f;
            Destroy(gameObject);
        }
    }
}   
