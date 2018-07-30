using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectScript : MonoBehaviour {
    [SerializeField]
    StatusEffect effect;

    public StatusEffect EFFECT
    {
        get { return effect; }
        set { effect = value; }
    }

    public void ApplyReaction(ManagerScript manager, LivingObject living)
    {
        int chance = Random.Range(0, 2);
        Debug.Log("Chance = " + chance);
        switch (effect)
        {
            case StatusEffect.paralyzed:
                Debug.Log(living.FullName + " is paralyzed");
                if(chance <= 0)
                {
                    Debug.Log("Player is stunned");
                    manager.DamageLivingObject(living, (int)(living.HEALTH * 0.1));

                    manager.NextTurn("effectScript");
                    return;
                }
                Debug.Log(living.FullName + " is no longer paralyzed");
                Destroy(this);
                break;
            case StatusEffect.sleep:
                Debug.Log(living.FullName + " is sleep");
                if (chance <= 0)
                {
                    Debug.Log(living.FullName + " is sleeping");
                    manager.DamageLivingObject(living, -(int)(living.HEALTH * 0.1));

                    manager.NextTurn("effectScript");
                    return;
                }
                Debug.Log(living.FullName + " woke up");
                Destroy(this);
                break;
            case StatusEffect.frozen:
                Debug.Log(living.FullName + " is frozen");
                if (chance <= 0)
                {
                    Debug.Log(living.FullName + " is frozen solid");
                    manager.NextTurn("effectScript");
                    return;
                }
                Debug.Log(living.FullName + " thawed out");
                Destroy(this);
                break;
            case StatusEffect.burned:
                Debug.Log(living.FullName + " is burned");
                manager.DamageLivingObject(living, (int)(living.HEALTH * 0.2));
                if (chance > 0)
                {
                    Debug.Log(living.FullName + " is no longer burned");
                    Destroy(this);
                }
                break;
            default:
                break;
        }
    }
}