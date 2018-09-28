using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectScript : MonoBehaviour
{
    [SerializeField]
    StatusEffect effect;

    public StatusEffect EFFECT
    {
        get { return effect; }
        set { effect = value; }
    }

    public void ApplyReaction(ManagerScript manager, LivingObject living)
    {
        int chance = Random.Range(-5, 5);
        Debug.Log("Status Effect Chance = " + chance);
        switch (effect)
        {
            case StatusEffect.paralyzed:
                Debug.Log(living.FullName + " is paralyzed");
                if (chance <= 0)
                {
                    //Debug.Log("Player is stunned");
                    int dmg = (int)(living.HEALTH * 0.1);
                    manager.DamageLivingObject(living, dmg);
                    manager.CreateDmgTextEvent(dmg.ToString(), Color.yellow, living);
                    manager.NextTurn("effectScript");
                    manager.CreateTextEvent(this, "" + living.FullName + " is stunned", "stun effect", manager.CheckText, manager.TextStart);

                    return;
                }
                manager.CreateTextEvent(this, "" + living.FullName + " is no longer paralyzed", "no longer paralyzed effect", manager.CheckText, manager.TextStart);
                Destroy(this);
                break;
            case StatusEffect.sleep:
                Debug.Log(living.FullName + " is sleep");
                if (chance <= 0)
                {
                    //   Debug.Log(living.FullName + " is sleeping");
                    int dmg = -(int)(living.HEALTH * 0.1);
                    manager.DamageLivingObject(living, dmg);
                    manager.CreateDmgTextEvent(dmg.ToString(), Color.blue, living);
                    manager.NextTurn("effectScript");
                    manager.CreateTextEvent(this, "" + living.FullName + " is sleeping", "sleep effect", manager.CheckText, manager.TextStart);
                    return;
                }
                manager.CreateTextEvent(this, "" + living.FullName + " woke up", "no longer sleep effect", manager.CheckText, manager.TextStart);
                Destroy(this);
                break;
            case StatusEffect.frozen:
                Debug.Log(living.FullName + " is frozen");
                if (chance <= 0)
                {
                    Debug.Log(living.FullName + " is frozen solid");
                    manager.CreateDmgTextEvent("Frozen", Color.cyan, living);
                    manager.NextTurn("effectScript");
                    manager.CreateTextEvent(this, "" + living.FullName + " is frozen", "frozen effect", manager.CheckText, manager.TextStart);
                    return;
                }
                manager.CreateTextEvent(this, "" + living.FullName + " thawed out", "no longer frozen effect", manager.CheckText, manager.TextStart);
                Destroy(this);
                break;
            case StatusEffect.burned:
                {

                    Debug.Log(living.FullName + " is burned");
                    int dmg = (int)(living.HEALTH * 0.2);
                    manager.DamageLivingObject(living, dmg);
                    manager.CreateDmgTextEvent(dmg.ToString(), Color.red, living);
                    manager.CreateTextEvent(this, "" + living.FullName + " took damage from their burn", "burned effect", manager.CheckText, manager.TextStart);

                    if (chance > 0)
                    {
                        manager.CreateTextEvent(this, "" + living.FullName + " is no longer burned", "no longer burned effect", manager.CheckText, manager.TextStart);
                        Destroy(this);
                    }
                }
                break;
            case StatusEffect.poisoned:
                Debug.Log(living.FullName + " is poisoned");
                manager.DamageLivingObject(living, (int)(living.HEALTH * 0.1));
                if (chance > 0)
                {
                    Debug.Log(living.FullName + " is no longer poisoned");
                    Destroy(this);
                }
                break;
            default:
                break;
        }
    }
}