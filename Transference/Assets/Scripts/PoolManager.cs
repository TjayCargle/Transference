using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    private static PoolManager pool;

    private PoolManager()
    {
        
    }

    public static PoolManager GetManager()
    {
        if(pool == null)
        {
            pool = new GameObject().AddComponent<PoolManager>();
        }
        return pool;
    }

    [SerializeField]
    private int effectCount = 0;

    [SerializeField]
    private List<ScriptableContainer> effects = new List<ScriptableContainer>();

    public EffectScript GetEffect()
    {
        EffectScript effect = null;
        for (int i = 0; i < effects.Count; i++)
        {
            ScriptableContainer script = effects[i];
            if (script.inUse == false)
            {
                effect = script.scriptable as EffectScript;
                script.inUse = true;
                effectCount++;
                break;
            }
        }

        if(effect == null)
        {
            effect = ScriptableObject.CreateInstance<EffectScript>();
            ScriptableContainer container = new ScriptableContainer();
            container.scriptable = effect;
            container.inUse = true;
            effects.Add(container);
            effectCount++;
        }


        return effect;
    }

    public void ReleaseEffect(EffectScript effect)
    {
        for (int i = 0; i < effects.Count; i++)
        {
            ScriptableContainer script = effects[i];
            if (script.scriptable == effect)
            {
                script.inUse = false;
                effectCount--;
                break;
            }
        }
    }


}
