using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float speed = 30f;
    private float life = 10f;

    public GameObject[] o_blackHole;
    private float distToBlackHole;
    private PlayerController player;
    public static float gravity;
    Rigidbody rb;
    public static float bulletSpeedCoef = 1f;
    public static bool vampirism;

    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        rb = GetComponent<Rigidbody>();
        rb.velocity = bulletSpeedCoef * transform.forward * speed * (1 + player.BonusBulletspeed / 10);

        o_blackHole = new GameObject[5];
        for (int i = 0; i < 5; i++)
            o_blackHole[i] = GameObject.Find("BH" + i);
    }

    private void Update()
    {
        if(gravity > 0)
            rb.velocity += 5 * Vector3.down * Time.deltaTime;
        
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

    public void setParent(RedMobController redMobController)
    {
        throw new System.NotImplementedException();
    }
}
