using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour {

    public Transform HUDCanvas;
    private Slider healthSlider;
    private GameObject o_shieldFill;
    public Slider deadHealthSlider;
    private Text hpLabel;
    private Image damageImage;
    private GameObject fillImage;
    float floating = 1f;
    public GameObject[] o_blackHole;
    public GameObject player;
    public bool isShieldOn;
    public static int points_shield;
    private float currentShieldHP;

    public int bonusHP = 0;
    public int BonusHP
    {
        get
        {
            return bonusHP;
        }
        set
        {
            bonusHP = value;
            MaxHealth = baseMaxHPPowCoef * baseMaxHP * (1 + (float)value / 10f);

            try { string s = "" + currentHP.ToString("00"); if (points_shield > 0) s += " + " + (int)CurrentSP; s += " / " + maxHealth.ToString("#"); hpLabel.text = s; }
            catch { Debug.Log(gameObject.name + ": HP LABEL NOT FOUND"); }
        }
    }

    public void GetHP(float v)
    {
        CurrentHP += v;
    }

    public float bonusHPR = 0f;

    private float baseMaxHP;
    public float BaseMaxHP
    {
        get { return baseMaxHP; }
        set
        {
            baseMaxHP = value;
            MaxHealth = baseMaxHPPowCoef * baseMaxHP * (1 + (float)bonusHP / 10f);
        }
    }

    private float maxHealth;
    public float MaxHealth
    {
        get { return maxHealth; }
        set
        {
            maxHealth = value;
            healthSlider.maxValue = value;
            deadHealthSlider.maxValue = value;

            (healthSlider.GetComponent<RectTransform>()).sizeDelta = new Vector2(value * 2, 20);
            (deadHealthSlider.GetComponent<RectTransform>()).sizeDelta = new Vector2(value * 2, 20);
            (fillImage.GetComponent<RectTransform>()).sizeDelta = new Vector2(value * 2, 10);
            try { string s = "" + currentHP.ToString("00"); if (points_shield > 0) s += " + " + (int)CurrentSP; s += " / " + maxHealth.ToString("#"); hpLabel.text = s; }
            catch { Debug.Log(gameObject.name + ": HP LABEL NOT FOUND"); }
        }
    }
    private float currentHP;
    public float CurrentHP {
        get
        {
            return currentHP;
        }
        set
        {
            if (value < maxHealth)
                currentHP = value;
            else
                currentHP = maxHealth;


            if (currentHP < deadHealthSlider.value)
            {
                deadHealthSlider.value -= deadHealthSlider.value * 0.005f;
            }
            else
                deadHealthSlider.value = currentHP;

            try { healthSlider.value = value; } catch { Debug.Log(gameObject.name + ": HP SLIDER NOT FOUND. NO HPS VALUE UPDATE"); }
            try { string s = "" + currentHP.ToString("00"); if (points_shield > 0) s += " + " + (int)CurrentSP; s += " / " + maxHealth.ToString("#"); hpLabel.text = s; }
            catch { Debug.Log(gameObject.name + ": HP LABEL NOT FOUND"); }
        }
    }
    private float damageCooldown = 0;
    public float DamageCooldown
    {
        get
        {
            return damageCooldown;
        }
        set
        {
            damageCooldown = value;
            if (DamageCooldown > 0.1f)
            {
                canBeHit = true;
            }
            else
            {
                canBeHit = false;
            }

            if (!isShieldDisabled && points_shield > 0 && !isShieldOn && DamageCooldown > 5f)
            {
                isShieldOn = true;
                currentShieldHP = points_shield;
                GameObject.Find("shield_on").GetComponent<AudioSource>().Play();
            }
        }
    }

    public float CurrentSP
    {
        get
        {
            return currentShieldHP;
        }

        set
        {
            currentShieldHP = value;
            try { string s = "" + currentHP.ToString("00"); if (points_shield > 0) s += " + " + (int)CurrentSP; s += " / " + maxHealth.ToString("#");  hpLabel.text = s; }
            catch { Debug.Log(gameObject.name + ": HP LABEL NOT FOUND"); }
            (o_shieldFill.GetComponent<RectTransform>()).sizeDelta = new Vector2(100 * currentShieldHP / (10 * points_shield), 4 + 4 * points_shield);
        }
    }

    public static bool isDead;
    bool damaged;
    private bool canBeHit;
    private float distToBlackHole;
    public float healthRegenPowCoef = 1.0f;
    public float damagePowCoef = 1.0f;
    public bool isShieldDisabled;
    public float baseMaxHPPowCoef = 1.0f;

    void Awake()
    {

        try { fillImage = GameObject.Find("PlayerHUDHP"); }
        catch { Debug.Log("No game object called PlayerHUDHP found"); }

        try { hpLabel = GameObject.Find("HPlabel").GetComponent<Text>(); }
        catch { Debug.Log("No game object called HPlabel found"); }

        try { healthSlider = GameObject.Find("Health Slider").GetComponent<Slider>(); }
        catch { Debug.Log("No HealthSlider object found"); }

        try { o_shieldFill = GameObject.Find("ShieldFill"); }
        catch { Debug.Log("No HealthSlider object found"); }

        try { deadHealthSlider = GameObject.Find("DamageSlider").GetComponent<Slider>(); }
        catch { Debug.Log("No DamageSlider object found"); }

        try { damageImage = GameObject.Find("DamageImage").GetComponent<Image>(); }
        catch { Debug.Log("No game object found"); }

        BaseMaxHP = 40;
        CurrentHP = 40;
        DamageCooldown = 0;
        points_shield = 0;
        (o_shieldFill.GetComponent<RectTransform>()).sizeDelta = new Vector2(0, 0);

        o_blackHole = new GameObject[5];
        for (int i = 0; i < 5; i++)
            o_blackHole[i] = GameObject.Find("BH" + i);
    }

    void Update()
    {
        if (isDead)
        {
            if (Time.timeScale - 0.008f > 0)
                Time.timeScale -= 0.008f;
            else
                Time.timeScale = 0f; 

            return;
        }

        if (damaged)
        {
            damageImage.enabled = true;
            damaged = false;
        }
        else
            damageImage.enabled = false;

        for (int j = 0; j < 5; j++)
        {
            distToBlackHole = Vector3.Distance(o_blackHole[j].transform.position, player.transform.position);
            if (distToBlackHole < 20)
                takeDamage(33f);

            if (distToBlackHole < 100)
                takeDamage(500 / distToBlackHole * Time.deltaTime);
        }

        updateHP();
    }

    public void upgradeHP(float upgradeLife)
    {
        BaseMaxHP += upgradeLife;
        // CurrentHP += upgradeLife;
    }

    public void takeDamage(float amount)
    {

        if (isShieldDisabled)
        {
            currentShieldHP = 0;
            isShieldOn = false;
        }
        if (canBeHit)
        {
            DamageCooldown = 0f;
            
            if (isShieldOn)
            {
                if (CurrentSP > amount)
                {
                    CurrentSP -= amount;
                }
                else if (CurrentSP < amount)
                {
                    amount -= CurrentSP;
                    CurrentSP = 0.0f;
                    isShieldOn = false;
                    CurrentHP -= amount;
                    damaged = true;
                    GameObject.Find("shield_off").GetComponent<AudioSource>().Play();
                }
            }
            else
            {
                CurrentHP -= amount;
                damaged = true;
            }
        }

        if(damaged)
        {
            if (currentHP > 0f)
            {
                float r = Random.Range(0, 3);
                if (r == 0)
                    FindObjectOfType<PlayerController>().GetComponent<AudioSource>()
                        .PlayOneShot(Resources.Load<AudioClip>("Sounds/Hit1"));
                else if (r == 1)
                    FindObjectOfType<PlayerController>().GetComponent<AudioSource>()
                        .PlayOneShot(Resources.Load<AudioClip>("Sounds/Hit2"));
                else if (r == 2)
                    FindObjectOfType<PlayerController>().GetComponent<AudioSource>()
                        .PlayOneShot(Resources.Load<AudioClip>("Sounds/Hit3"));
            }

            if (CurrentHP <= 0f && !isDead)
                RIP();
        }

    }

    private void updateHP()
    {
        DamageCooldown += Time.deltaTime;
        regen();

        if (floating > currentHP / maxHealth)
            floating -= maxHealth / 5f * Time.deltaTime;
        else
            floating = currentHP / maxHealth;

        if (isShieldOn && CurrentSP < 10 * points_shield)
        {
            CurrentSP += Time.deltaTime * 5f;
        }
            
    }

    private void regen()
    {
        //CurrentHP += Time.deltaTime * BaseMaxHP * ( 1 + bonusHPRegen / 5) / 200f;
        if (CurrentHP < MaxHealth)
            CurrentHP += healthRegenPowCoef * Time.deltaTime * (0.25f * bonusHPR + 0.33f);
    }

    void RIP()
    {
        isDead = true;
        MusicControl.stopAll();
        FindObjectOfType<PlayerController>().GetComponent<AudioSource>().PlayOneShot(Resources.Load<AudioClip>("Sounds/citydepop"));
        CurrentHP = 0f;
		//TimeSystem.writer.Close();
    }
}
