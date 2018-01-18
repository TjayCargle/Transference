using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum Element
{
    Water,
    Fire,
    Ice,
    Electric,

    Slash,
    Pierce,
    Blunt,

    Neutral

}
public enum EType
{
    physical,
    magical,
}
public class SkillScript : UsableScript
{

    [SerializeField]
    protected string description;
    [SerializeField]
    protected int accuraccy;
    [SerializeField]
    protected int dmg;
    [SerializeField]
    private Vector2[] affecttedTiles;
    [SerializeField]
    private Vector2 myAttackRange;
    [SerializeField]
    protected float critRate;
    [SerializeField]
    protected Element affinity;


  

    public string DESC
    {
        get { return description; }
        set { description = value; }
    }
    public int ACCURACY
    {
        get { return accuraccy; }
        set { accuraccy = value; }
    }
    public int DMG
    {
        get { return dmg; }
        set { dmg = value; }
    }
    public Vector2[] TILES
    {
        get { return affecttedTiles; }
        set { affecttedTiles = value; }
    }
    public Vector2 Range
    {
        get { return myAttackRange; }
        set { myAttackRange = value; }
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

}
