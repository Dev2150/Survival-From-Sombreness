using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollideNapalmGround : MonoBehaviour {
    
    public ParticleSystem GroundFlame;
    public ParticleSystem Flamethrower;
    private List<ParticleCollisionEvent> collisionEvents;
    public GameObject g_fire;
    public GameObject g_smoke;
    public GameObject g_embers;

    private void Start()
    {
        collisionEvents = new List<ParticleCollisionEvent>();
    }

    private void OnParticleCollision(GameObject other)
    {
    ParticlePhysicsExtensions.GetCollisionEvents(Flamethrower, other, collisionEvents);
        for (var index = 0; index < collisionEvents.Count; index++)
        {
            var t = collisionEvents[index];
            EmitAtLocation(t);
        }
    }

    private void EmitAtLocation(ParticleCollisionEvent collisionEvent)
    {
        GroundFlame.transform.position = collisionEvent.intersection;
        // GroundFlame.Emit(30);
        GameObject e;
        Vector3 offset = -Vector3.up;
        e = Instantiate(g_fire, collisionEvent.intersection + offset, Quaternion.Euler(-90, 0, 0));
        Destroy(e, 10);
        e = Instantiate(g_smoke, collisionEvent.intersection + offset , Quaternion.Euler(-90, 0, 0));
        Destroy(e, 10);
        e = Instantiate(g_embers, collisionEvent.intersection + offset, Quaternion.Euler(-90, 0, 0));
        Destroy(e, 10);
    }
}
