using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Security.Cryptography.X509Certificates;
using ParadoxNotion.Serialization.FullSerializer;
using UnityEngine.Playables;
using Object = UnityEngine.Object;

public class PlayerController : MonoBehaviour {
    private const int EnergyDash = 10;
    private EnergySystem _energySys;
	public Transform PauseCanvas;
	public float Speed = 15;
    public float BonusSpeed;
    public float BonusAcceleration;
	public GameObject OBulletPrefab, OShrapnelPrefab, OImpulsePrefab, ORocketPrefab, OPlasmaPrefab, OConcrete, OPlasmaSlowPrefab, OShockPrefab, OBigShockPrefab;
    public Transform GSel;
    public Image IWmg, IWsh, IWim, IWro, IWso, IWco;
    public Image IPow01, IPow02, IPow03, IPow04, IPow05; 
    public Transform TrAmmoSpawn;
    private int _pointsMinimap;
    public int PointsManaregen;
    public int PointsManamax;
    public int PointsDamage;
    public int BonusBulletspeed;
    public int PointsAttackspeed;
    public int PointsMe;
    private int _pointsMg = 1;
    private int _pointsSh;
    private int _pointsIm;
    private int _pointsRo;
    private int _pointsPl;
    private int _pointsCo;
    public float EnergyMa = 5, CooldownMa, MaxCdMa = 0.20f, DamageMa = 7f;
    public float EnergySh = 9, CooldownSh, MaxCdSh = 0.40f, DamageSh = 7f;
    public float EnergyIm = 20, CooldownIm, MaxCdIm = 0.50f, DamageIm = 29f;
    public float EnergyRo = 47, CooldownRo, MaxCdRo = 0.48f;
    public float EnergyPl = 16, CooldownPl, MaxCdPl = 1.00f, DamagePl = 3.5f;
    public Transform Rockets;
    //public float EnergySo = 16, CooldownSo, MaxCdSo = 0.48f;
    public float EnergyCo = 50, CooldownCo, MaxCdCo = 1.00f, DamageCo;
    public float EnergyMe = 25, CooldownMe, MaxCdMe = 300f;
    public float[] WeaponLevels;
    private int _currentWeapon = 1;
    private float _fTimescaleTimer = 0f;
    private float _blackHoleDilation;
    public Boolean Paused = false;
	public NavMeshAgent Agent;
	private Vector3 _movementDirection;
	Vector3 _v3Rotation, _v3Velocity;
	public int IToxicityLevel;
	PostEffectScript _pes;
    public Image ImgToxicity;
    private PlayerHealth health;

    //public Image ImgKeyQ, ImgKeyW, ImgKeyE, ImgKeyR, ImgKeyA, ImgKeyS, ImgKeyD, ImgKeyShift, ImgKeyH, ImgKey1, ImgKey2, ImgKey3;
    //private Sprite _imgKeyQ0, _imgKeyW0, _imgKeyE0, _imgKeyR0, _imgKeyA0, _imgKeyS0, _imgKeyD0, _imgKeyShift0, _imgKeyH0, _imgKey10, _imgKey20, _imgKey30;
    //private Sprite _imgKeyQ1, _imgKeyW1, _imgKeyE1, _imgKeyR1, _imgKeyA1, _imgKeyS1, _imgKeyD1, _imgKeyShift1, _imgKeyH1, _imgKey11, _imgKey21, _imgKey31;
    
    public GameObject Camera1, Camera2;

	public GameObject OPow2On, OPow2Off;

    public GameObject[] OBlackHole;
    private float _distToBlackHole;

    public int PowerCooldown, PowerManaRegen, PowerHealthRegen, PowerDamage, PowerArmor, PowerSpeed, PowerBulletSpeed;
    public Powers[]   PowerNames    = new Powers[5];
    public float[]    PowerTimes    = new float[5];
    public float[]    PowerDurations = new float[5];
    public Image[] CurrentPowerImages;
    public Image[] BackPowerImages;
    public Sprite[]   PowerSprites;

    public enum Powers
    {
        None,
        SpeedMore,
        SpeedLess,
        NoShift,
        DoubleShift,
        HealthMore,
        HealthLess,
        ManaMore,
        ManaLess,
        DamageMore,
        DamageLess,
        AttackSpeedMore,
        AttackSpeedLess,
        LifestealPlayer,
        LifestealEnemy,
        BulletGravity,
        Vampirism,
        ShieldBreak,
        BulletDoubleSpeed,
        Arbalest,
        EnemyRegen,
        Glass
    }
    private readonly Quaternion _qM08 = Quaternion.Euler(0, -8, 0), _qP08 = Quaternion.Euler(0, +8, 0), _qM04 = Quaternion.Euler(0, -4, 0), _qP04 = Quaternion.Euler(0, +4, 0) ;
    public GameObject OMinimap;
    private float _time1W;
    private bool _isTapW;
    private float _time2W;
    private float _time1S;
    private bool _isTapS;
    private float _time2S;
    private float _time1A;
    private bool _isTapA;
    private float _time2A;
    private float _buttonCoolerW, _buttonCoolerS, _buttonCoolerA, _buttonCoolerD;
    private float _time2D;
    private float _time1D;
    private bool _isTapD;
    private int _buttonCountW, _buttonCountS, _buttonCountA, _buttonCountD;
    private AudioSource _audioSource;
    public float cooldownPowCoef = 1.0f;
    public float speedPowCoef = 1.0f;
    public float afterburnerPowCoef = 1.0f;
    public float damagePowCoef = 1.0f;
    public float lifeStealCoef;
    public float vampirism;
    public float cooldownArbalest = 1f;
    public float damageGlassCoef = 1f;


    public PlayerController()
    {
        CooldownSh = 0f;
        _pointsCo = 0;
        _pointsPl = 0;
        _pointsRo = 0;
        _pointsIm = 0;
        CooldownMa = 0f;
        //CooldownSo = 0f;
        CooldownPl = 0f;
        CooldownCo = 0f;
        CooldownMe = 0f;
    }

    public int PointsMg
    {
        get
        {
            return _pointsMg;
        }

        set
        {
            _pointsMg = value;
            switch (value)
            {
                case 1:
                    IWmg.sprite = Resources.Load("slot_WMG1", typeof(Sprite)) as Sprite;
                    MaxCdMa = 0.20f;
                    EnergyMa = 5f;
                    break;
                case 2:
                    IWmg.sprite = Resources.Load("slot_WMG2", typeof(Sprite)) as Sprite;
                    MaxCdMa = 0.12f;
                    EnergyMa = 3.8f;
                    break;
                case 3:
                    IWmg.sprite = Resources.Load("slot_WMG3", typeof(Sprite)) as Sprite;
                    MaxCdMa = 0.08f;
                    EnergyMa = 1.5f;
                    break;
            }
        }
    }

    public int PointsSh
    {
        get
        {
            return _pointsSh;
        }

        set
        {
            _pointsSh = value;
            switch (value)
            {
                case 1:
                    IWsh.sprite = Resources.Load("slot_WSH1", typeof(Sprite)) as Sprite;
                    MaxCdSh = 0.40f;
                    EnergySh = 12.5f;
                    break;
                case 2:
                    IWsh.sprite = Resources.Load("slot_WSH2", typeof(Sprite)) as Sprite;
                    MaxCdSh = 0.48f;
                    EnergySh = 8.3f;
                    break;
                case 3:
                    IWsh.sprite = Resources.Load("slot_WSH3", typeof(Sprite)) as Sprite;
                    MaxCdSh = 0.18f;
                    EnergySh = 7.5f;
                    break;
            }
        }
    }

    public int PointsIm
    {
        get
        {
            return _pointsIm;
        }

        set
        {
            _pointsIm = value;
            switch (value)
            {
                case 1:
                    IWim.sprite = Resources.Load("slot_WIM1", typeof(Sprite)) as Sprite;
                    MaxCdIm = 0.50f;
                    EnergyIm = 15f;
                    break;
                case 2:
                    IWim.sprite = Resources.Load("slot_WIM2", typeof(Sprite)) as Sprite;
                    MaxCdIm = 0.64f;
                    EnergyIm = 21f;
                    break;
                case 3:
                    IWim.sprite = Resources.Load("slot_WIM3", typeof(Sprite)) as Sprite;
                    MaxCdIm = 0.16f;
                    EnergyIm = 5.8f;
                    break;
            }
        }
    }

    public int PointsRo
    {
        get
        {
            return _pointsRo;
        }

        set
        {
            _pointsRo = value;
            switch (value)
            {
                case 1:
                    IWro.sprite = Resources.Load("slot_WRO1", typeof(Sprite)) as Sprite;
                    MaxCdRo = 0.48f;
                    EnergyRo = 21.4f;
                    break;
                case 2:
                    IWro.sprite = Resources.Load("slot_WRO2", typeof(Sprite)) as Sprite;
                    MaxCdRo = 0.32f;
                    EnergyRo = 15f;
                    break;
                case 3:
                    IWro.sprite = Resources.Load("slot_WRO3", typeof(Sprite)) as Sprite;
                    MaxCdRo = 0.52f;
                    EnergyRo = 18.8f;
                    break;
            }
        }

    }

    public int PointsPl
    {
        get
        {
            return _pointsPl;
        }

        set
        {
            _pointsPl = value;
            switch (value)
            {
                case 1:
                    IWso.sprite = Resources.Load("slot_WSO1", typeof(Sprite)) as Sprite;
                    MaxCdPl = 0.40f;
                    EnergySh = 12.5f;
                    break;
                case 2:
                    IWso.sprite = Resources.Load("slot_WSO2", typeof(Sprite)) as Sprite;
                    MaxCdPl = 0.5f;
                    EnergySh = 6f;
                    break;
                case 3:
                    IWso.sprite = Resources.Load("slot_WSO3", typeof(Sprite)) as Sprite;
                    MaxCdPl = 0.176f;
                    EnergySh = 25f;
                    break;
            }
        }
    }

    public int PointsCo
    {
        get
        {
            return _pointsCo;
        }

        set
        {
            _pointsCo = value;
            switch (value)
            {
                case 1:
                    IWco.sprite = Resources.Load("slot_WCO1", typeof(Sprite)) as Sprite;
                    MaxCdCo = 2.0f;
                    break;
                case 2:
                    IWco.sprite = Resources.Load("slot_WCO2", typeof(Sprite)) as Sprite;
                    MaxCdCo = 1.5f;
                    break;
                case 3:
                    IWco.sprite = Resources.Load("slot_WCO3", typeof(Sprite)) as Sprite;
                    MaxCdCo = 1.0f;
                    break;
            }
        }
    }

    private int CurrentWeapon
    {
        get
        {
            return _currentWeapon;
        }

        set
        {
            _currentWeapon = value;
            GSel.transform.position = new Vector3(13 + -70 + _currentWeapon * 68, 5, 0);
        }
    }

    public int PointsMinimap
    {
        get
        {
            return _pointsMinimap;
        }

        set
        {
            _pointsMinimap = value;
            switch (value)
            {
                case 0:
                    OMinimap.SetActive(false);
                    break;
                default:
                    OMinimap.SetActive(true);
                    Camera2.GetComponent<Camera>().orthographicSize = 30 * value;
                    break;
            }
        }
    }

    public int PointsDash { get; set; }

    void Awake()
    {
		_pes = FindObjectOfType <PostEffectScript> ();
        health = FindObjectOfType<PlayerHealth>();
        while (IToxicityLevel > 0)
            RemoveToxicity();
		IToxicityLevel = 0;
		// pauseCanvas = GameObject.Find ("PauseUI").transform;
		Agent = GetComponent<NavMeshAgent> ();
        Cursor.lockState = CursorLockMode.Locked;
        _energySys = FindObjectOfType<EnergySystem>();
        // bulletPrefab = (GameObject)Resources.Load("Prefabs/Bullet", typeof(GameObject));

        OPow2On.SetActive (false);
		OPow2Off.SetActive (false);

        Camera2 = GameObject.Find("CAM_RTS");

        online_keyboard_HUD_awake();

        OBlackHole = new GameObject[5];
        for (int i = 0; i < 5; i++)
            OBlackHole[i] = GameObject.Find("BH" + i);

        IWmg = GameObject.Find("slot_WMG").GetComponent<Image>();
        IWsh = GameObject.Find("slot_WSH").GetComponent<Image>();
        IWim = GameObject.Find("slot_WIM").GetComponent<Image>();
        IWro = GameObject.Find("slot_WRO").GetComponent<Image>();
        IWso = GameObject.Find("slot_WSO").GetComponent<Image>();
        IWco = GameObject.Find("slot_WCO").GetComponent<Image>();

        CurrentWeapon = 1;
        PointsMinimap = 0;


        CurrentPowerImages  = new Image[5];
        BackPowerImages     = new Image[5];
        CurrentPowerImages[0] = GameObject.Find("Power01").GetComponent<Image>();
        CurrentPowerImages[1] = GameObject.Find("Power02").GetComponent<Image>();
        CurrentPowerImages[2] = GameObject.Find("Power03").GetComponent<Image>();
        CurrentPowerImages[3] = GameObject.Find("Power04").GetComponent<Image>();
        CurrentPowerImages[4] = GameObject.Find("Power05").GetComponent<Image>();
        BackPowerImages[0] = GameObject.Find("Power01Back").GetComponent<Image>();
        BackPowerImages[1] = GameObject.Find("Power02Back").GetComponent<Image>();
        BackPowerImages[2] = GameObject.Find("Power03Back").GetComponent<Image>();
        BackPowerImages[3] = GameObject.Find("Power04Back").GetComponent<Image>();
        BackPowerImages[4] = GameObject.Find("Power05Back").GetComponent<Image>();

//        PowerSprites = new Sprite[13];
//        PowerSprites[0] = Resources.Load<Sprite>("/Powers/power_null");
//        PowerSprites[1] = Resources.Load<Sprite>("/Powers/power_speed_up");
//        PowerSprites[2] = Resources.Load<Sprite>("/Powers/power_speed_down");
//        PowerSprites[3] = Resources.Load<Sprite>("/Powers/power_afterburner_up");
//        PowerSprites[4] = Resources.Load<Sprite>("/Powers/power_afterburner_down");
//        PowerSprites[5] = Resources.Load<Sprite>("/Powers/power_health_up");
//        PowerSprites[6] = Resources.Load<Sprite>("/Powers/power_health_down");
//        PowerSprites[7] = Resources.Load<Sprite>("/Powers/power_attspd_down");
//        PowerSprites[8] = Resources.Load<Sprite>("/Powers/power_attspd_up");
//        PowerSprites[7] = Resources.Load<Sprite>("/Powers/power_attspd_down");
//        PowerSprites[7] = Resources.Load<Sprite>("/Powers/power_attspd_down");
//        PowerSprites[8] = Resources.Load<Sprite>("/Powers/power_mana_up");
//        PowerSprites[9] = Resources.Load<Sprite>("/Powers/power_mana_down");
//        PowerSprites[10] = Resources.Load<Sprite>("/Powers/power_damage_up");
//        PowerSprites[11] = Resources.Load<Sprite>("/Powers/power_damage_down");
        for (int i = 0; i < CurrentPowerImages.Length; i++)
        {
            CurrentPowerImages[i].sprite = PowerSprites[0];
            BackPowerImages[i].sprite = PowerSprites[0];
        }
    }

    private void online_keyboard_HUD_awake()
    {
        /*
        _imgKeyQ0 = Resources.Load("KQ0", typeof(Sprite)) as Sprite;
        _imgKeyQ1 = Resources.Load("KQ1", typeof(Sprite)) as Sprite;
        _imgKeyW0 = Resources.Load("KW0", typeof(Sprite)) as Sprite;
        _imgKeyW1 = Resources.Load("KW1", typeof(Sprite)) as Sprite;
        _imgKeyE0 = Resources.Load("KE0", typeof(Sprite)) as Sprite;
        _imgKeyE1 = Resources.Load("KE1", typeof(Sprite)) as Sprite;
        _imgKeyR0 = Resources.Load("KR0", typeof(Sprite)) as Sprite;
        _imgKeyR1 = Resources.Load("KR1", typeof(Sprite)) as Sprite;
        _imgKeyA0 = Resources.Load("KA0", typeof(Sprite)) as Sprite;
        _imgKeyA1 = Resources.Load("KA1", typeof(Sprite)) as Sprite;
        _imgKeyS0 = Resources.Load("KS0", typeof(Sprite)) as Sprite;
        _imgKeyS1 = Resources.Load("KS1", typeof(Sprite)) as Sprite;
        _imgKeyD0 = Resources.Load("KD0", typeof(Sprite)) as Sprite;
        _imgKeyD1 = Resources.Load("KD1", typeof(Sprite)) as Sprite;
        _imgKeyShift0 = Resources.Load("KShift0", typeof(Sprite)) as Sprite;
        _imgKeyShift1 = Resources.Load("KShift1", typeof(Sprite)) as Sprite;

        _imgKeyH0 = Resources.Load("KH0", typeof(Sprite)) as Sprite;
        _imgKeyH1 = Resources.Load("KH1", typeof(Sprite)) as Sprite;
        _imgKey10 = Resources.Load("K10", typeof(Sprite)) as Sprite;
        _imgKey11 = Resources.Load("K11", typeof(Sprite)) as Sprite;
        _imgKey20 = Resources.Load("K20", typeof(Sprite)) as Sprite;
        _imgKey21 = Resources.Load("K21", typeof(Sprite)) as Sprite;
        _imgKey30 = Resources.Load("K30", typeof(Sprite)) as Sprite;
        _imgKey31 = Resources.Load("K31", typeof(Sprite)) as Sprite;
        */
    }

    void Update()
    {
        online_keyboard_HUD();

        if (PlayerHealth.isDead) {
			if (Input.GetKeyDown (KeyCode.Escape)) {
				Cursor.lockState = CursorLockMode.None;
				Cursor.visible = true;
				SceneManager.LoadScene ("Main Menu");
			}
			return;
		}

        if (Input.GetKeyDown (KeyCode.Escape)) {
            Paused = !Paused;
            if (Paused)
                Time.timeScale = 0f;
            else
                Time.timeScale = 1f;
        }

        BonusAcceleration = afterburnerPowCoef != 0 && Input.GetKey (KeyCode.LeftShift) && _energySys.isEnough (10 * Time.deltaTime) ? afterburnerPowCoef * 0.2f : 0.0f;

        FireWeapons();

        if (PointsMe > 0) {
            OPow2On.GetComponent<Image>().fillAmount = CooldownMe / (300 - 60 * PointsMe);
            OPow2Off.SetActive (true);
            OPow2On.SetActive(true);
        }
		


		_v3Velocity = Vector3.zero;
        if (Input.GetButton("Vertical")) //Move the Rigidbody forwards constantly at speed you define (the blue arrow axis in Scene view)
            _v3Velocity += Input.GetAxis("Vertical") * transform.forward;

        if (Input.GetButton("Horizontal"))
        {
            //Move the Rigidbody forwards constantly at speed you define (the blue arrow axis in Scene view)
            _v3Velocity += Input.GetAxis("Horizontal") * transform.right;
        }
        _v3Velocity = _v3Velocity.normalized;

        if (Input.GetKeyDown(KeyCode.W)) // when mouse is clicked
        {
            if (_isTapW)
            {
                _time1W = Time.time;
                _isTapW = false;

                
                if (_time1W - _time2W < 0.2f) // interval between two clicked
                {
                    if (PointsDash > 0 && _energySys.isEnough(EnergyDash)) { 
                        GameObject.Find("teleport").GetComponent<AudioSource>().Play();
                        _v3Velocity += 2 * Speed * (1 + 0.2f * PointsDash) * Mathf.Pow(2, PowerSpeed) * transform.forward;
                    }
                }
            }
        }
        else // first of all, enter here because the mouse is not clicked
        {
            if (!_isTapW)
            {
                _time2W = Time.time;
                _isTapW = true;
            }
        }
        if (_buttonCoolerW > 0)
            _buttonCoolerW -= 1 * Time.deltaTime;
        else
            _buttonCountW = 0;

        if (Input.GetKeyDown(KeyCode.S)) // when mouse is clicked
        {
            if (_isTapS)
            {
                _time1S = Time.time;
                _isTapS = false;

                if (_time1S - _time2S < 0.2f) // interval between two clicked
                {
                    if (PointsDash > 0 && _energySys.isEnough(EnergyDash))
                    {
                        GameObject.Find("teleport").GetComponent<AudioSource>().Play();
                        _v3Velocity -= 2 * Speed * (1 + 0.2f * PointsDash) * transform.forward;
                    }
                }
            }
        }
        else // first of all, enter here because the mouse is not clicked
        {
            if (_isTapS == false)
            {
                _time2S = Time.time;
                _isTapS = true;
            }
        }
        if (_buttonCoolerS > 0)
            _buttonCoolerS -= 1 * Time.deltaTime;
        else
            _buttonCountS = 0;


        if (Input.GetKeyDown(KeyCode.A)) // when mouse is clicked
        {
            if (_isTapA)
            {
                _time1A = Time.time;
                _isTapA = false;

                if (_time1A - _time2A < 0.2f) // interval between two clicked
                {
                    if (PointsDash > 0 && _energySys.isEnough(EnergyDash))
                    {
                        GameObject.Find("teleport").GetComponent<AudioSource>().Play();
                        _v3Velocity -= 2 * Speed * (1 + 0.2f * PointsDash) * transform.right;
                    }
                }
            }
        }
        else // first of all, enter here because the mouse is not clicked
        {
            if (_isTapA == false)
            {
                _time2A = Time.time;
                _isTapA = true;
            }
        }
        if (_buttonCoolerA > 0)
            _buttonCoolerA -= 1 * Time.deltaTime;
        else
            _buttonCountA = 0;

        if (Input.GetKeyDown(KeyCode.D)) // when mouse is clicked
        {
            if (_isTapD)
            {
                _time1D = Time.time;
                _isTapD = false;

                if (_time1D - _time2D < 0.2f) // interval between two clicked
                {
                    if (PointsDash > 0 && _energySys.isEnough(EnergyDash))
                    {
                        GameObject.Find("teleport").GetComponent<AudioSource>().Play();
                        _v3Velocity += 2 * Speed * (1 + 0.2f * PointsDash) * transform.right;
                    }
                }
            }
        }
        else // first of all, enter here because the mouse is not clicked
        {
            if (_isTapD == false)
            {
                _time2D = Time.time;
                _isTapD = true;
            }
        }
        if (_buttonCoolerD > 0)
            _buttonCoolerD -= 1 * Time.deltaTime;
        else
            _buttonCountD = 0;

        _v3Velocity *= speedPowCoef * Speed * (1 + BonusSpeed / 10f) * (1f + BonusAcceleration);

        for (int j = 0; j < 5; j++)
        {
            _distToBlackHole = Vector3.Distance(OBlackHole[j].transform.position, transform.position);
            if (_distToBlackHole < 1000)
            {
                Agent.Warp(transform.position + 1000f * (OBlackHole[j].transform.position - transform.position) / (_distToBlackHole * _distToBlackHole) * Time.deltaTime);
            }
        }

        try
        {
            if (_v3Velocity != Vector3.zero)
                Agent.Warp(transform.position + _v3Velocity * Time.deltaTime);
        }
        catch (Exception e)
        {
            Agent.Warp(transform.position);
        }

        if (_fTimescaleTimer < 0) {
			_fTimescaleTimer = UnityEngine.Random.Range (1,1);

            _blackHoleDilation = Math.Max(1, 50f / _distToBlackHole);

            Time.timeScale = 1; //blackHoleDilation;

            if (IToxicityLevel > 2)
                Time.timeScale *= 1 + UnityEngine.Random.Range(Math.Max(0, 0.2f - IToxicityLevel * 0.1f), Math.Max(0, 0.33f * IToxicityLevel - 0.66f));

		}

        handlePowers();
		handleCDnTimers();
    }

    private void handleCDnTimers()
    {
        CooldownMa += cooldownArbalest * cooldownPowCoef * (1.0f + 0.2f * PointsAttackspeed) * Time.deltaTime;
        CooldownSh += cooldownArbalest * cooldownPowCoef * (1.0f + 0.2f * PointsAttackspeed) * Time.deltaTime;
        CooldownIm += cooldownArbalest * cooldownPowCoef * (1.0f + 0.2f * PointsAttackspeed) * Time.deltaTime;
        CooldownRo += cooldownArbalest * cooldownPowCoef * (1.0f + 0.2f * PointsAttackspeed) * Time.deltaTime;
        CooldownPl += cooldownArbalest * cooldownPowCoef * (1.0f + 0.2f * PointsAttackspeed) * Time.deltaTime;
        //CooldownSo += (1.0f + 0.2f * PointsAttackspeed) * Time.deltaTime;
        CooldownCo += cooldownArbalest * cooldownPowCoef * (1.0f + 0.2f * PointsAttackspeed) * Time.deltaTime;
        CooldownMe += Time.deltaTime;
        _fTimescaleTimer -= Time.deltaTime;
    }

    private void handlePowers()
    {
        for (int i = CurrentPowerImages.Length - 1; i >= 0; i--)
        {
            if (PowerTimes[i] > 0)
            {
                PowerTimes[i] -= Time.deltaTime;
                CurrentPowerImages[i].fillAmount = PowerTimes[i] / PowerDurations[i];
            }
            else if (PowerTimes[i] < 0)
            {
                PowerTimes[i] = 0;
    
                switch (PowerNames[i])
                {
                    case Powers.SpeedMore:
                    case Powers.SpeedLess:
                        speedPowCoef = 1f;
                        break;
                    case Powers.DoubleShift:
                    case Powers.NoShift:
                        afterburnerPowCoef = 1;
                        break;
                    case Powers.HealthMore:
                    case Powers.HealthLess:
                        FindObjectOfType<PlayerHealth>().healthRegenPowCoef = 1f;
                        break;
                    case Powers.ManaMore:
                    case Powers.ManaLess:
                        FindObjectOfType<EnergySystem>().energyRegenCoef = 1f;
                        break;
                    case Powers.DamageMore:
                    case Powers.DamageLess:
                        damagePowCoef = 1f;
                        break;
                    case Powers.AttackSpeedMore:
                    case Powers.AttackSpeedLess:
                        cooldownPowCoef = 1f;
                        break;
                    case Powers.BulletGravity:
                        BulletController.gravity = 0f;
                        break;
                    case Powers.ShieldBreak:
                        FindObjectOfType<PlayerHealth>().isShieldDisabled = true;
                        break;
                    case Powers.Vampirism:
                        vampirism = 0f;
                        break;
                    case Powers.BulletDoubleSpeed:
                        BulletController.bulletSpeedCoef = 1f;
                        break;
                    case Powers.Arbalest:
                        FindObjectOfType<EnergySystem>().energyRegenArbalest = 1f;
                        cooldownArbalest = 1f;
                        break;
                    case Powers.Glass:
                        health.baseMaxHPPowCoef = 1f;
                        health.BaseMaxHP -= 0.1f;
                        health.CurrentHP *= 6f;
                        damageGlassCoef = 1f;
                        break;
                }

                CurrentPowerImages[i].sprite  = PowerSprites[0];
                BackPowerImages[i].sprite     = PowerSprites[0];
                PowerTimes[i] = 0;
                PowerNames[i] = Powers.None;
                PowerDurations[i] = 0;
                
                for (int j = i; j < CurrentPowerImages.Length - 1; j++)
                {
                    if (PowerTimes[j + 1] > 0)
                    {
                        CurrentPowerImages[j].sprite = CurrentPowerImages[j + 1].sprite;
                        BackPowerImages[j].sprite = BackPowerImages[j + 1].sprite;
                        PowerTimes[j] = PowerTimes[j + 1];
                        PowerNames[j] = PowerNames[j + 1];
                        PowerDurations[j] = PowerDurations[j + 1];
                    }
                    else
                    {
                        CurrentPowerImages[j].sprite  = PowerSprites[0];
                        BackPowerImages[j].sprite     = PowerSprites[0];
                        PowerNames[j] = Powers.None;
                        PowerDurations[j] = 0;
                        break;
                    }
                }
                CurrentPowerImages[4].sprite  = PowerSprites[0];
                BackPowerImages[4].sprite     = PowerSprites[0];
                PowerTimes[4] = 0;
                PowerNames[4] = Powers.None;
                PowerDurations[4] = 0;
            }
        }
    }

    private void FireWeapons()
    {


        if (Input.GetButton("Fire1"))
        {
            switch (CurrentWeapon)
            {
                case 1:
                    if (CooldownMa > MaxCdMa && _energySys.isEnough(EnergyMa))
                    {
                        FireMachineGun();
                        if (vampirism > 0f)
                            health.CurrentHP -= vampirism * EnergyMa;
                        CooldownMa = 0;
                    }
                    break;
                case 2:
                    if (_pointsSh == 0)
                        break;
                    if (CooldownSh > MaxCdSh && _energySys.isEnough(EnergySh))
                    {
                        FireShrapnel();
                        if (vampirism > 0f)
                            health.CurrentHP -= vampirism * EnergySh;
                        CooldownSh = 0;
                    }
                    break;
                case 3:
                    if (_pointsIm == 0)
                        break;
                    if (CooldownIm > MaxCdIm && _energySys.isEnough(EnergyIm))
                    {
                        FireImpulse();
                        CooldownIm = 0;
                        if (vampirism > 0f)
                            health.CurrentHP -= vampirism * EnergyIm;
                    }
                    break;
                case 4:
                    if (_pointsRo == 0) break;
                    if (CooldownRo > MaxCdRo && _energySys.isEnough(EnergyRo))
                    {
                        FireRocket();
                        CooldownRo = 0;
                        if (vampirism > 0f)
                            health.CurrentHP -= vampirism * EnergyRo;
                    }
                    break;
                case 5:
                    if (_pointsPl == 0) break;
                    if (CooldownPl > MaxCdPl && _energySys.isEnough(EnergyPl))
                    {
                        FirePlasma();
                        if (vampirism > 0f)
                            health.CurrentHP -= vampirism * EnergyPl;
                        CooldownPl = 0;
                    }
                    break;
                case 6:
                    if (_pointsCo == 0) break;
                    if (CooldownCo > MaxCdCo && _energySys.isEnough(EnergyCo))
                    {
                        FireConcrete();
                        CooldownCo = 0;
                    }
                    break;
            
            }
        }
        
        if (Input.GetKeyDown(KeyCode.Z))
            CurrentWeapon = 1;
        if (Input.GetKeyDown(KeyCode.X))
            CurrentWeapon = 2;
        if (Input.GetKeyDown(KeyCode.C))
            CurrentWeapon = 3;
        if (Input.GetKeyDown(KeyCode.V))
            CurrentWeapon = 4;
        if (Input.GetKeyDown(KeyCode.B))
            CurrentWeapon = 5;
        if (Input.GetKeyDown(KeyCode.N))
            CurrentWeapon = 6;



        if (Input.GetButton("Fire2"))
        {
            if (PointsMe > 0 && CooldownMe > 300 - 60 * PointsMe && IToxicityLevel > 0 && _energySys.isEnough(EnergyMe))
            {
                RemoveToxicity();
            }
        }
    }

    private void online_keyboard_HUD()
    {
        /*
        img_key_Q.sprite = img_key_Q_0;
        img_key_W.sprite = img_key_W_0;
        img_key_E.sprite = img_key_E_0;
        img_key_R.sprite = img_key_R_0;
        img_key_A.sprite = img_key_A_0;
        img_key_S.sprite = img_key_S_0;
        img_key_D.sprite = img_key_D_0;
        img_key_H.sprite = img_key_H_0;
        img_key_1.sprite = img_key_1_0;
        img_key_2.sprite = img_key_2_0;
        img_key_3.sprite = img_key_3_0;
        img_key_Shift.sprite = img_key_Shift_0;
        if (Input.GetKey(KeyCode.Q)) img_key_Q.sprite = img_key_Q_1;
        if (Input.GetKey(KeyCode.W)) img_key_W.sprite = img_key_W_1;
        if (Input.GetKey(KeyCode.E)) img_key_E.sprite = img_key_E_1;
        if (Input.GetKey(KeyCode.R)) img_key_R.sprite = img_key_R_1;
        if (Input.GetKey(KeyCode.A)) img_key_A.sprite = img_key_A_1;
        if (Input.GetKey(KeyCode.S)) img_key_S.sprite = img_key_S_1;
        if (Input.GetKey(KeyCode.D)) img_key_D.sprite = img_key_D_1;
        if (Input.GetKey(KeyCode.H)) img_key_H.sprite = img_key_H_1;
        if (Input.GetKey(KeyCode.Alpha1)) img_key_1.sprite = img_key_1_1;
        if (Input.GetKey(KeyCode.Alpha2)) img_key_2.sprite = img_key_2_1;
        if (Input.GetKey(KeyCode.Alpha3)) img_key_3.sprite = img_key_3_1;
        if (Input.GetKey(KeyCode.LeftShift)) img_key_Shift.sprite = img_key_Shift_1;
        */
    }

    private void FireMachineGun()
    {        
        GameObject shot = Instantiate(OBulletPrefab, TrAmmoSpawn.position, TrAmmoSpawn.rotation); // Destroy(bullet, 2.0f);
        //shot.Start();
        Destroy(shot, 10f);
    }

    private void FireConcrete()
    {
        //GetComponent<AudioSource>().PlayOneShot(Resources.Load<AudioClip>("Sounds/fire_plasma4"));
		GameObject.Find("shot_concrete").GetComponent<AudioSource>().Play();
		GameObject shot = Instantiate(OConcrete, TrAmmoSpawn.position, TrAmmoSpawn.rotation); // Destroy(bullet, 2.0f);
		Destroy(shot, 10f);
    }

    private void FirePlasma()
    {
        GetComponent<AudioSource>().PlayOneShot(Resources.Load<AudioClip>("Sounds/fire_bolt_long"));
        GameObject shot;
//        switch (_pointsPl)
//        {
/*            case 2: // TODO
                shot = Instantiate(OShockPrefab, TrAmmoSpawn.position, TrAmmoSpawn.rotation); // Destroy(bullet, 2.0f);
                Destroy(shot, 10f);
                break;
            case 3: //TODO
				shot = Instantiate(OBigShockPrefab, TrAmmoSpawn.position, TrAmmoSpawn.rotation); // Destroy(bullet, 2.0f);
                Destroy(shot, 10f);
                break;
            default:
				shot = Instantiate(OShockPrefab, TrAmmoSpawn.position, TrAmmoSpawn.rotation); // Destroy(bullet, 2.0f);
                Destroy(shot, 10f);
                break;*/
                
            GameObject.Find("shot_plasma").GetComponent<AudioSource>().Play();

            var bullet = Instantiate(OShrapnelPrefab, TrAmmoSpawn.position, TrAmmoSpawn.rotation);
            Destroy(bullet, 3);
            bullet = Instantiate(OShrapnelPrefab, TrAmmoSpawn.position, TrAmmoSpawn.rotation * _qM08);
            Destroy(bullet, 3);
            bullet = Instantiate(OShrapnelPrefab, TrAmmoSpawn.position, TrAmmoSpawn.rotation * _qP08);
            Destroy(bullet, 3);

            bullet = Instantiate(OShrapnelPrefab, TrAmmoSpawn.position, TrAmmoSpawn.rotation * _qM04);
            Destroy(bullet, 3);
            bullet = Instantiate(OShrapnelPrefab, TrAmmoSpawn.position, TrAmmoSpawn.rotation * _qP04);
            Destroy(bullet, 3);

            if (_pointsPl == 3)
            {
                bullet = Instantiate(OShrapnelPrefab, TrAmmoSpawn.position, TrAmmoSpawn.rotation * _qM04);
                Destroy(bullet, 3);
                bullet = Instantiate(OShrapnelPrefab, TrAmmoSpawn.position, TrAmmoSpawn.rotation * _qP04);
                Destroy(bullet, 3);

                bullet = Instantiate(OShrapnelPrefab, TrAmmoSpawn.position, TrAmmoSpawn.rotation * _qM08);
                Destroy(bullet, 3);
                bullet = Instantiate(OShrapnelPrefab, TrAmmoSpawn.position, TrAmmoSpawn.rotation * _qP08);
                Destroy(bullet, 3);
            }

    }

    private void FireRocket()
    {
        GameObject.Find("shot_rocket").GetComponent<AudioSource>().Play();
        GameObject shot;

        switch (_pointsRo)
        {
            case 2: // TODO
                shot = Instantiate(ORocketPrefab, TrAmmoSpawn.position, TrAmmoSpawn.rotation); // Destroy(bullet, 2.0f);
                Destroy(shot, 10f);
                break;
            case 3:
                shot = Instantiate(ORocketPrefab, TrAmmoSpawn.position, TrAmmoSpawn.rotation); // Destroy(bullet, 2.0f);
                shot.transform.Translate(shot.transform.forward * 10f + shot.transform.right * 30f, Space.World);
                shot = Instantiate(ORocketPrefab, TrAmmoSpawn.position, TrAmmoSpawn.rotation); // Destroy(bullet, 2.0f);
                shot.transform.Translate(shot.transform.forward * 10f - shot.transform.right * 30f, Space.World);
                shot = Instantiate(ORocketPrefab, TrAmmoSpawn.position, TrAmmoSpawn.rotation); // Destroy(bullet, 2.0f);
                shot.transform.Translate(shot.transform.forward * 35f, Space.Self);
                Destroy(shot, 10f);
                break;
            default:

			shot = Instantiate(ORocketPrefab, TrAmmoSpawn.position, TrAmmoSpawn.rotation); // Destroy(bullet, 2.0f);
            //    shot = Instantiate(o_rocketPrefab, tr_ammoSpawn.position + Vector3.forward * 2 + Vector3.up * 1f, Quaternion.identity, rockets);
                //shot.transform.localRotation = Quaternion.identity;
           // shot.transform.Rotate(tr_ammoSpawn.transform.position.x, tr_ammoSpawn.transform.position.y, tr_ammoSpawn.transform.position.z);
                Destroy(shot, 5f);
                break;
        }

    }

    private void FireImpulse()
    {
        GameObject.Find("shot_impulse").GetComponent<AudioSource>().Play();
        GameObject shot = Instantiate(OImpulsePrefab, TrAmmoSpawn.position, TrAmmoSpawn.rotation); // Destroy(bullet, 2.0f);
        Destroy(shot, 5f);
        if (_pointsIm == 2)
        {
            shot = Instantiate(OImpulsePrefab, TrAmmoSpawn.position, TrAmmoSpawn.rotation * _qM08); // Destroy(bullet, 2.0f);
            Destroy(shot, 5f);
            shot = Instantiate(OImpulsePrefab, TrAmmoSpawn.position, TrAmmoSpawn.rotation * _qP08); // Destroy(bullet, 2.0f);
            Destroy(shot, 5f);
        }
        
    }

    private void FireShrapnel()
    {
        GameObject.Find("shot_shrapnel").GetComponent<AudioSource>().Play();

        var bullet = Instantiate(OShrapnelPrefab, TrAmmoSpawn.position, TrAmmoSpawn.rotation);
        Destroy(bullet, 5);
        bullet = Instantiate(OShrapnelPrefab, TrAmmoSpawn.position, TrAmmoSpawn.rotation * _qM08);
        Destroy(bullet, 5f);
        bullet = Instantiate(OShrapnelPrefab, TrAmmoSpawn.position, TrAmmoSpawn.rotation * _qP08);
        Destroy(bullet, 5f);

        if (_pointsSh == 2)
        {
            bullet = Instantiate(OShrapnelPrefab, TrAmmoSpawn.position, TrAmmoSpawn.rotation * _qM04);
            Destroy(bullet, 5f);
            bullet = Instantiate(OShrapnelPrefab, TrAmmoSpawn.position, TrAmmoSpawn.rotation * _qP04);
            Destroy(bullet, 5f);
        }
    }

    public void AddToxicity() {

		IToxicityLevel++;

		if (IToxicityLevel > 6)
			IToxicityLevel = 6;
	
		ImgToxicity.sprite = Resources.Load ("T" + IToxicityLevel, typeof(Sprite)) as Sprite;
		_pes.changeTo (IToxicityLevel);
	}
	public void RemoveToxicity() {

        CooldownMe = 0;
	    IToxicityLevel--;

		ImgToxicity.sprite = Resources.Load ("T" + IToxicityLevel, typeof(Sprite)) as Sprite;
		_pes.changeTo (IToxicityLevel);
	}
}
