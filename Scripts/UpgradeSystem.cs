using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeSystem : MonoBehaviour {

	public static int count;

	private DifficultySystem diffSys;

    private Card[] C;
    private Card[] chosen = new Card[3];
    public Sprite sprite1; // Drag your first sprite here
    public Sprite sprite2; // Drag your second sprite here
    private PlayerController player;

    private Text tutorialText;

    public BulletController[] bullets = new BulletController[6];
    public BulletController bullet;
    Behaviour[] halos;
    private PlayerHealth health;
    private Image img;
    public Image[] cardImages = new Image[3]; // The image cards

	private List<Color> colors = new List<Color> ();
	private static Color[] colList = { Color.cyan, Color.black, Color.green, Color.magenta, Color.gray, Color.red, Color.blue, Color.green, Color.white};

    private bool windowOpen = false;
    public bool WindowOpen
    {
        get
        {
            return windowOpen;
        }
        set
        {
            windowOpen = value;
            if(value)
            {
                for (int i = 0; i < cardImages.Length; ++i) cardImages[i].enabled = true;
                tutorialText.color = Color.clear;
            }
            else
            {
                for (int i = 0; i < cardImages.Length; ++i) cardImages[i].enabled = false;
            }
        }
    }

    int remainingLevels;
    private bool tutorialSw;
    private int bulletHalo;
    private int bulletSpeedLevel;

    void Start()
    {
		try { diffSys = FindObjectOfType<DifficultySystem>(); }
		catch { Debug.Log("No UpgradeSystem found"); }

        img = GameObject.Find("CrossImage").GetComponent<Image>();
        health = FindObjectOfType <PlayerHealth> ();

        for (int i = 0; i < cardImages.Length; ++i)
            cardImages[i].enabled = false;

        player = FindObjectOfType<PlayerController>();
        tutorialText = GameObject.Find("TextTutorialUpgrade").GetComponent<Text>();

        img.sprite = sprite1;

        C = SelectDeck();

        chosen = Card.PickFromDeck(C, 3);
        updateCards();

        bullet = bullets[bulletHalo];
        bullet.GetComponent<TrailRenderer>().enabled = false;
    }

    private Card[] SelectDeck()
    {
        Card[] e = new Card[82];
		Card[] Req;

        repeat(0, "LOS", e);
        repeat(5, "MSP", e);
        repeat(10, "HPR", e);
        repeat(15, "HPM", e);
        repeat(20, "BSP", e);
        repeat(25, "ATK", e);
		repeat(30, "REJ", e);
		repeat(35, "MNM", e, 01);
		repeat(40, "DSH", e, 06);
		repeat(45, "SHD", e, 11);
		repeat(50, "MND", e, 16);
		repeat(55, "ASP", e, 21);
		repeat(60, "MMP", e, 31);

        int i = 65;
        Req = new Card[3]; Req[0] = e[26];    Req[1] = e[02]; Req[2] = e[37]; e[i + 0] = new Card("WMG", 2, 4f, Req);
        Req = new Card[3]; Req[0] = e[i + 0]; Req[1] = e[04]; Req[2] = e[39]; e[i + 1] = new Card("WMG", 2, 1f, Req);

        i = 67;
        Req = new Card[2]; Req[0] = e[26];                    Req[1] = e[40]; e[i + 0] = new Card("WSH", 1, 16f, Req);
        Req = new Card[3]; Req[0] = e[i + 0]; Req[1] = e[07]; Req[2] = e[42]; e[i + 1] = new Card("WSH", 2, 4f, Req);
        Req = new Card[3]; Req[0] = e[i + 1]; Req[1] = e[09]; Req[2] = e[44]; e[i + 2] = new Card("WSH", 3, 1f, Req);

        i = 70;
        Req = new Card[2]; Req[0] = e[26];                    Req[1] = e[45]; e[i + 0] = new Card("WIM", 1, 16f, Req);
        Req = new Card[3]; Req[0] = e[i + 0]; Req[1] = e[13]; Req[2] = e[47]; e[i + 1] = new Card("WIM", 2, 4f, Req);
        Req = new Card[3]; Req[0] = e[i + 1]; Req[1] = e[14]; Req[2] = e[49]; e[i + 2] = new Card("WIM", 3, 1f, Req);

        i = 73;
        Req = new Card[2]; Req[0] = e[26];                    Req[1] = e[50]; e[i + 0] = new Card("WRO", 1, 16f, Req);
        Req = new Card[3]; Req[0] = e[i + 0]; Req[1] = e[18]; Req[2] = e[52]; e[i + 1] = new Card("WRO", 2, 4f, Req);
        Req = new Card[3]; Req[0] = e[i + 1]; Req[1] = e[19]; Req[2] = e[54]; e[i + 2] = new Card("WRO", 3, 1f, Req);

        i = 76;
        Req = new Card[2]; Req[0] = e[26];                    Req[1] = e[55]; e[i + 0] = new Card("WCO", 1, 16f, Req);
        Req = new Card[3]; Req[0] = e[i + 0]; Req[1] = e[23]; Req[2] = e[57]; e[i + 1] = new Card("WCO", 2, 4f, Req);
        Req = new Card[3]; Req[0] = e[i + 1]; Req[1] = e[24]; Req[2] = e[59]; e[i + 2] = new Card("WCO", 3, 1f, Req);

        i = 79;
        Req = new Card[2]; Req[0] = e[26];                    Req[1] = e[60]; e[i + 0] = new Card("WSO", 1, 16f, Req);
        Req = new Card[3]; Req[0] = e[i + 0]; Req[1] = e[33]; Req[2] = e[62]; e[i + 1] = new Card("WSO", 2, 4f, Req);
        Req = new Card[3]; Req[0] = e[i + 1]; Req[1] = e[34]; Req[2] = e[64]; e[i + 2] = new Card("WSO", 3, 1f, Req);

        
        

        return e;
    }

    private void repeat(int i, string s, Card[] e, int v3)
    {
        Card[] Req;
        Req = new Card[1]; Req[0] = e[v3]; e[i + 0] = new Card(s, 1, 256, Req);
        Req = new Card[1]; Req[0] = e[i + 0]; e[i + 1] = new Card(s, 2, 64, Req);
        Req = new Card[1]; Req[0] = e[i + 1]; e[i + 2] = new Card(s, 3, 16, Req);
        Req = new Card[1]; Req[0] = e[i + 2]; e[i + 3] = new Card(s, 4, 4, Req);
        Req = new Card[1]; Req[0] = e[i + 3]; e[i + 4] = new Card(s, 5, 1, Req);
    }

    private void repeat(int i, string s, Card[] e)
    {
        Card[] Req;
        e[i + 0] = new Card(s, 1, 1024, new Card[0]); Req = new Card[1]; Req[0] = e[i + 0];
        e[i + 1] = new Card(s, 2, 256, Req); Req = new Card[1]; Req[0] = e[i + 1];
        e[i + 2] = new Card(s, 3, 64, Req); Req = new Card[1]; Req[0] = e[i + 2];
        e[i + 3] = new Card(s, 4, 16, Req); Req = new Card[1]; Req[0] = e[i + 3];
        e[i + 4] = new Card(s, 5, 4, Req);
    }
    void Update()
    {
		// 	OnGUI ();
        if (PlayerHealth.isDead)
        {
            WindowOpen = false;
            return;
        }

        if (WindowOpen)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
                upgradePlayer(chosen[0]);
            if (Input.GetKeyDown(KeyCode.Alpha2))
                upgradePlayer(chosen[1]);
            if (Input.GetKeyDown(KeyCode.Alpha3))
                upgradePlayer(chosen[2]);

            if (remainingLevels == 0)
                WindowOpen = false;
        }   

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (WindowOpen)
                WindowOpen = false;
            else if (remainingLevels > 0)
                WindowOpen = true;
        }

        SwitchSprite();


    }


	void OnGUI() {
        return;

		int X1 = (int)(Screen.width * 0.75), X2 = X1 + 100, X = X1;
		int Y = 20; // 800

		for (int i = 0; i < colList.Length; i++) foreach (Color c in colors) if (colList[i] == c) {
			
			//drawText (new Vector2 (X - 1, Y - 1), "1");
			
			// DrawQuad (new Rect (X - 1, Y - 1, 22, 22), Color.white); //todo disabled quad
			// DrawQuad (new Rect (X, Y, 20, 20), colList[i]);
			X += 25;
			if (X > X2) {
				X = X1;
				Y += 25;
			}
		}
	}

    private void upgradePlayer(Card card)
    {
		int E = 1 + Convert.ToInt32(diffSys.doublePower);

		Boolean sw;
		do {
			sw = true;

            //player o_player = FindObjectOfType<player>();

            switch (card.type)
            {
                case "MSP": // movement speed
                    player.BonusSpeed += E;
                    break;
                case "BSP":
                    bulletSpeedLevel++;
                    setTrails(bulletHalo);
                    player.BonusBulletspeed += 4 * E;
                    break;
                case "HPM":
                    health.BonusHP += E;
                    break;
                case "HPR":
                    health.bonusHPR += 0.75f * E;
                    break;
                case "LOS":
                    count++;
                    if (E == 2)
                        RenderSettings.fogDensity /= 1.3f * 1.3f;
                    else
                        RenderSettings.fogDensity /= 1.3f;

                    RenderSettings.ambientIntensity += 0.1f * E;
                    break;
                case "ATK":
                    bulletHalo++;
                    bullet = bullets[bulletHalo];
                    player.PointsDamage += E;
                    break;
                case "REJ":
                    player.PointsManaregen ++;
                    break;
                case "MNM":
                    player.PointsMinimap ++;
                    break;
                case "DSH":
                    player.PointsDash ++;
                    break;
                case "SHD":
                    PlayerHealth.points_shield ++;
                    break;
                case "MND":
                    player.PointsMe ++;
                    break;
                case "ASP":
                    player.PointsAttackspeed ++;
                    break;
                case "MMP":
                    player.PointsManamax ++;
                    break;
                case "WMG":
                    player.PointsMg ++;
                    break;
                case "WSH":
                    player.PointsSh ++;
                    break;
                case "WIM":
                    player.PointsIm ++;
                    break;
                case "WRO":
                    player.PointsRo ++;
                    break;
                case "WSO":
                    player.PointsPl ++;
                    break;
                case "WCO":
                    player.PointsCo++;
                    break;

                default:
                    sw = false;
                    break;
            }
		} while(sw == false);

        card.acquired = true;
        C = Card.RemoveFromDeck(card, C);
        //Debug.Log("Remaining cards : " + C.Length);
        remainingLevels -= 1;
        chosen = Card.PickFromDeck(C, 3);
        updateCards();
        }

    protected void setTrails(int bulletSpeedLevel)
    {
        if (bulletSpeedLevel == 1)
            bullet.GetComponent<TrailRenderer>().enabled = true;
        bullet.GetComponent<TrailRenderer>().time = 2 / bulletSpeedLevel;
    }

    private void updateCards()
    {

        for (int i = 0; i < 3; ++i)
        {
            String name = "" + chosen[i].type + chosen[i].tier;
            try
            		{
                cardImages[i].sprite = Resources.Load(name, typeof(Sprite)) as Sprite;
            }
            catch
            {
                Debug.Log("Error loading the texture :");
            }

            // Debug.Log("Info: Card #" + i + " with name " + name + ", chosen with RN[" + chosen[i].chosenRandomNo + "]/MXR[" + chosen[i].chosenMaxRange + "]\n");
            // Debug.Log(Card.errors);
			// Card.errors = "";
				

        }
    }

    void SwitchSprite()
    {
        if (windowOpen || remainingLevels > 0 && Time.time % 2 >= 1)
        {
            img.sprite = sprite2;
            img.color = Color.white;
        }
        else if (remainingLevels <= 0) // window closed or oscillating
        {
            img.sprite = sprite1;
            img.color = Color.clear;
        }
        else {
            img.sprite = sprite1;
        }
    }

    public void OneUP(int lvl)
    {
        remainingLevels++;
        if (lvl == 1)
            tutorialText.color = Color.white;
    }

	private void DrawQuad(Rect position, Color color) {
		Texture2D texture = new Texture2D(1, 1);
		texture.SetPixel(0,0,color);
		texture.Apply();
		GUI.skin.box.normal.background = texture;
		GUI.Box(position, GUIContent.none);
	}

}
