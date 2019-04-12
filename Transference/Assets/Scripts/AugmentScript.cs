using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AugmentScript : ScriptableObject
{

   
    public List<bool>  augmentTriggers =  new List<bool>();

    public void Reset()
    {
        augmentTriggers.Clear();
        int count = (int)Augment.end;
        for (int i = 1; i < count; i++)
        {
            augmentTriggers.Add(false);
        }
    }
}
