using UnityEngine;

public class ImpulseController: MonoBehaviour
{
    public float bonusDamage = 0f;
    public float speed = 100f;
    public float bonusSpeed = 0f;
    private float life = 47.5f;

    public GameObject[] o_blackHole;
    private float distToBlackHole;
    private PlayerController player;


    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        GetComponent<Rigidbody>().velocity = transform.forward * speed * (1 + bonusSpeed / 10);
        if (PlayerHealth.isDead)
            speed = Mathf.Lerp(speed, 0, 0.1f);

        o_blackHole = new GameObject[5];
        for (int i = 0; i < 5; i++)
            o_blackHole[i] = GameObject.Find("BH" + i);
    }

    private void Update()
    {
        for (int i = 0; i < 5; i++)
        {
            distToBlackHole = Vector3.Distance(o_blackHole[i].transform.position, transform.position);
            if (distToBlackHole < 250)
            {
                transform.Translate(3000f * (o_blackHole[i].transform.position - transform.position) / (distToBlackHole * distToBlackHole) * Time.deltaTime);
                if (distToBlackHole < 30)
                    Destroy(this);
            }
        }

        if (life < 0f) 
            Destroy(gameObject);
    }

    void OnTriggerEnter(Collider col)
    {
        RedMobController r;
        if (r = col.gameObject.GetComponent<RedMobController>())
        {
            float formerLife = life - r.currHealth;
            r.damage(life * (10 + player.PointsDamage) / 10);
            life = formerLife;
            EnemyHealthSystem.Put(r);
        }
    }
}
