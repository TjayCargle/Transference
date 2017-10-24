using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmorScript : MonoBehaviour
{
    [SerializeField]
    private int myDefense;
    [SerializeField]
    private int mySpeed;
    [SerializeField]
    private string myName;
    private LivingObject owner;
    public LivingObject USER
    {
        get { return owner; }
        set { owner = value; }
    }
    public int DEFENSE
    {
        get { return myDefense; }
        set { myDefense = value; }
    }
    public int SPEED
    {
        get { return mySpeed; }
        set { mySpeed = value; }
    }
    public string NAME
    {
        get { return myName; }
        set { myName = value; }
    }
}
