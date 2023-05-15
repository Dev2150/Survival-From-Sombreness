using UnityEngine;
using UnityEngine.AI;
public class EnergyCube : MonoBehaviour {
	NavMeshAgent agent;
	static NavMeshHit hit;
    Animation anim;

	void Start() {
		agent = GetComponent<NavMeshAgent> ();
		NavMesh.SamplePosition(transform.position, out hit, 1000, 1);
		agent.Warp (hit.position + Vector3.up * 2f);
        GetComponent<Animation>()["Power"].speed = 0.1f;

	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            FindObjectOfType<EnergySystem>().getEnergy(25);
            Destroy(gameObject);
        }
    }
}
