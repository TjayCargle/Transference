using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum Element
{
    Water,
    Fire,
    Ice,
    Electric,

    Phys,
    Neutral

}
public class SkillScript : MonoBehaviour
{
    public string name;
    public int accuraccy;
    public int dmg;
    public float critRate;
    public Element affinity;
    public string description;

    public bool isWeak(Element myAttack, Element Opponent)
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
            case Element.Phys:
                    result = false;
                break;
            case Element.Neutral:
                result = false;
                break;

        }
        return result;
    }

}
