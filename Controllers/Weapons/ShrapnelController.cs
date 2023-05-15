using UnityEngine;

public class ShrapnelController: MonoBehaviour
{
    public static float bonusDamage = 0f;
    public static float speed = 40f;
    public static float bonusSpeed = 0f;
    private float life = 10f;

    public GameObject[] o_blackHole;
    private float distToBlackHole;

    void Start()
    {
        GetComponent<Rigidbody>().velocity = transform.forward * speed * (1 + bonusSpeed / 10);
        if (PlayerHealth.isDead)
            speed = Mathf.Lerp(speed, 0, 0.1f);

        o_blackHole = new GameObject[5];
        for (int i = 0; i < 5; i++)
            o_blackHole[i] = GameObject.Find("BH" + i);

        (GetComponent<AudioSource>()).Play();
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

}
