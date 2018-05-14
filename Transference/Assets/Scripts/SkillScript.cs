using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillScript : UsableScript
{

    [SerializeField]
    protected int accuraccy;

    [SerializeField]
    protected DMG damage;

    [SerializeField]
    private List<Vector2> affecttedTiles;

    [SerializeField]
    protected RanngeType rType;

    [SerializeField]
    protected float critRate;

    [SerializeField]
    protected Element affinity;

    [SerializeField]
    protected EType eType;

    [SerializeField]
    protected int hitCount;

    [SerializeField]
    protected int next;

    [SerializeField]
    protected int nextCount;

    [SerializeField]
    private ModifiedStat modStat;
    [SerializeField]
    private List<Element> modElements;
    [SerializeField]
    private List<float> modValues;

    public ModifiedStat ModStat
    {
        get
        {
            return modStat;
        }

        set
        {
            modStat = value;
        }
    }

    public List<Element> ModElements
    {
        get
        {
            return modElements;
        }

        set
        {
            modElements = value;
        }
    }

    public List<float> ModValues
    {
        get
        {
            return modValues;
        }

        set
        {
            modValues = value;
        }
    }

    public int ACCURACY
    {
        get { return accuraccy; }
        set { accuraccy = value; }
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

    public int HITS
    {
        get { return hitCount; }
        set { hitCount = value; }
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
    public Element ELEMENT
    {
        get { return affinity; }
        set { affinity = value; }
    }
    public EType ETYPE
    {
        get { return eType; }
        set { eType = value; }
    }
    public RanngeType RTYPE
    {
        get { return rType; }
        set { rType = value; }
    }
    public static bool isWeak(Element myAttack, Element Opponent)
    {
        bool result = false;
        switch (myAttack)
        {
            case Element.Water:
                if (Opponent == Element.Fire)
                    result = true;
                else
                    result = false;
                break;
            case Element.Fire:
                if (Opponent == Element.Ice)
                    result = true;
                else
                    result = false;
                break;
            case Element.Ice:
                if (Opponent == Element.Electric)
                    result = true;
                else
                    result = false;
                break;
            case Element.Electric:
                if (Opponent == Element.Water)
                    result = true;
                else
                    result = false;
                break;
            case Element.Slash:
                if (Opponent == Element.Pierce)
                    result = true;
                else
                    result = false;
                break;
            case Element.Pierce:
                if (Opponent == Element.Blunt)
                    result = true;
                else
                    result = false;
                break;
            case Element.Blunt:
                if (Opponent == Element.Slash)
                    result = true;
                else
                    result = false;
                break;
            case Element.Neutral:
                result = false;
                break;

        }
        return result;
    }
    public static bool isResistant(Element myAttack, Element Opponent)
    {
        bool result = false;
        switch (myAttack)
        {
            case Element.Water:
                if (Opponent == Element.Fire)
                    result = true;
                else
                    result = false;
                break;
            case Element.Fire:
                if (Opponent == Element.Ice)
                    result = true;
                else
                    result = false;
                break;
            case Element.Ice:
                if (Opponent == Element.Electric)
                    result = true;
                else
                    result = false;
                break;
            case Element.Electric:
                if (Opponent == Element.Ice)
                    result = true;
                else
                    result = false;
                break;
            case Element.Slash:
                if (Opponent == Element.Pierce)
                    result = true;
                else
                    result = false;
                break;
            case Element.Pierce:
                if (Opponent == Element.Blunt)
                    result = true;
                else
                    result = false;
                break;
            case Element.Blunt:
                if (Opponent == Element.Slash)
                    result = true;
                else
                    result = false;
                break;
            case Element.Neutral:
                result = false;
                break;

        }
        return result;
    }
    public static Element getWeakness(Element invokingElement)
    {
        Element returnedElement = Element.Neutral;
        switch (invokingElement)
        {
            case Element.Water:
                returnedElement = Element.Electric;

                break;
            case Element.Fire:
                returnedElement = Element.Water;

                break;
            case Element.Ice:
                returnedElement = Element.Fire;
                break;
            case Element.Electric:
                returnedElement = Element.Ice;
                break;
            case Element.Slash:
                returnedElement = Element.Blunt;
                break;
            case Element.Pierce:
                returnedElement = Element.Slash;
                break;
            case Element.Blunt:
                returnedElement = Element.Pierce;
                break;


        }
        return returnedElement;
    }
    public void UseSkill(LivingObject user)
    {
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

                    }
                }
            }
        }

    }

}
