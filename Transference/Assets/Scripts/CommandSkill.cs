using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandSkill : SkillScript
{

    [SerializeField]
    protected int cost;

    [SerializeField]
    protected int accuraccy;

    [SerializeField]
    protected SideEffect effect;

    [SerializeField]
    protected DMG damage;

    [SerializeField]
    private List<Vector2> affecttedTiles;

    [SerializeField]
    protected RanngeType rType;

    [SerializeField]
    protected float critRate;

    [SerializeField]
    private BuffType buff;

    [SerializeField]
    private float modVal;

    [SerializeField]
    private ModifiedStat buffedStat;

    [SerializeField]
    protected int hitCount;

    [SerializeField]
    protected EType eType;

    [SerializeField]
    protected Reaction reaction;

    [SerializeField]
    protected int next;

    [SerializeField]
    protected int nextCount;

    public int COST
    {
        get { return cost; }
        set { cost = value; }
    }

    public int ACCURACY
    {
        get { return accuraccy; }
        set { accuraccy = value; }
    }

    public SideEffect EFFECT
    {
        get { return effect; }
        set { effect = value; }
    }

    public DMG DAMAGE
    {
        get { return damage; }
        set { damage = value; }
    }
    public List<Vector2> TILES
    {
        get { return affecttedTiles; }
        set { affecttedTiles = value; }
    }

    public float CRIT_RATE
    {
        get { return critRate; }
        set { critRate = value; }
    }

    public RanngeType RTYPE
    {
        get { return rType; }
        set { rType = value; }
    }
    public Reaction REACTION
    {
        get { return reaction; }
        set { reaction = value; }
    }
    public int HITS
    {
        get { return hitCount; }
        set { hitCount = value; }
    }

    public BuffType BUFF
    {
        get { return buff; }
        set { buff = value; }
    }

    public float BUFFVAL
    {
        get { return modVal; }
        set { modVal = value; }
    }

    public ModifiedStat BUFFEDSTAT
    {
        get { return buffedStat; }
        set { buffedStat = value; }
    }

    public EType ETYPE
    {
        get { return eType; }
        set { eType = value; }
    }


    public int NEXT
    {
        get { return next; }
        set { next = value; }
    }

    public int NEXTCOUNT
    {
        get { return nextCount; }
        set { nextCount = value; }
    }


    public bool UseSkill(LivingObject user, float modification = 1.0f)
    {

        if (ETYPE == EType.magical)
        {
            OWNER.STATS.MANA -= (int)(COST * modification);
        }
        else
        {
            OWNER.STATS.FATIGUE += (int)(COST * modification);
        }
        // OWNER.TakeAction();
        if (NEXT > 0)
        {
            if (NEXTCOUNT > 0)
            {
                NEXTCOUNT--;
                if (NEXTCOUNT <= 0)
                {

                    DatabaseManager database = GameObject.FindObjectOfType<DatabaseManager>();
                    if (database != null)
                    {
                        database.LearnSkill(NEXT, user);
                        return true; //to check if a new skill was learned

                    }
                }
            }
        }
        return false;

    }
    public bool CanUse(float modification = 1.0f)
    {
        bool can = false;
        int amt = 0;
        switch (ETYPE)
        {
            case EType.physical:
                amt = owner.FATIGUE + (int)(COST * modification);
                if (amt < owner.MAX_FATIGUE)
                {
                    if (amt >= 0)
                    {

                        can = true;
                    }
                }
                break;
            case EType.magical:
                amt = owner.MANA + (int)(COST * modification);

                if (amt >= 0)
                {
                    can = true;
                }
                break;
        }
        return can;
    }
}
