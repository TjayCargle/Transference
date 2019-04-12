using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AugBtn : MonoBehaviour
{
    [SerializeField]
    Augment myAugment;

    public Augment AUGMENT
    {
        get { return myAugment; }
        set { myAugment = value; }
    }
}
