using UnityEngine;
using UnityEngine.AI;
public abstract class MobController : MonoBehaviour
{
    protected ScoringSystem ScoreSys;
    protected LevelingSystem LevelSys;
    protected DifficultySystem diffSys;
    protected PlayerHealth PlayerHealth;
    protected Transform playerTr;

    public int collCorrection = 3;
    public float orientation;
    public float goX;
    public float goY;

    public float currHealth;
    public float maxHealth;
    public bool Dead;

	protected NavMeshAgent agent;
	protected NavMeshHit hit;
	public PlayerController player;
	public static float regen;

	public void Initialize (PlayerController p, PlayerHealth h, ScoringSystem s, LevelingSystem l, DifficultySystem d)
	{
		player = p;
		playerTr = p.transform;
		PlayerHealth = h;
		ScoreSys = s;
		LevelSys = l;
		diffSys = d; 
	}

	public void regenHP()
	{
		if(regen > 0f)
			currHealth += regen * Time.deltaTime;
	}
	public void damage(float v)
	{
		v *= player.damageGlassCoef;
		currHealth -= v;
		if(player.lifeStealCoef >0)
			PlayerHealth.GetHP(v * player.lifeStealCoef );
        if (currHealth <= 0)
			RIP();
	}

	protected virtual void RIP(bool isKilledByPlayer = true)
    {
	    if (isKilledByPlayer)
	    {
		    EnemyHealthSystem.Put(this);
	    }
	    Dead = true;
		Destroy(gameObject);
    }
}