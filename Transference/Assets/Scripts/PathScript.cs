using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathScript : MonoBehaviour {

    [SerializeField]
    private int firePhysicalPath;
    [SerializeField]
    private int waterPhysicalPath;
    [SerializeField]
    private int icePhysicalPath;
    [SerializeField]
    private int elecPhysicalPath;

    [SerializeField]
    private int fireMagicalPath;
    [SerializeField]
    private int waterMagicalPath;
    [SerializeField]
    private int iceMagicalPath;
    [SerializeField]
    private int elecMagicalPath;

    [SerializeField]
    private int autoPath;
    [SerializeField]
    private int passivePath;
    [SerializeField]
    private int oppPath;

    public int PHYS_FIRE_PATH
    {
        get { return firePhysicalPath; }
        set { firePhysicalPath = value; }
    }

    public int PHYS_WATER_PATH
    {
        get { return waterPhysicalPath; }
        set { waterPhysicalPath = value; }
    }

    public int PHYS_ICE_PATH
    {
        get { return icePhysicalPath; }
        set { icePhysicalPath = value; }
    }

    public int PHYS_ELEC_PATH
    {
        get { return elecPhysicalPath; }
        set { elecPhysicalPath = value; }
    }



    public int MAG_FIRE_PATH
    {
        get { return fireMagicalPath; }
        set { fireMagicalPath = value; }
    }

    public int MAG_WATER_PATH
    {
        get { return waterMagicalPath; }
        set { waterMagicalPath = value; }
    }

    public int MAG_ICE_PATH
    {
        get { return iceMagicalPath; }
        set { iceMagicalPath = value; }
    }

    public int MAG_ELEC_PATH
    {
        get { return elecMagicalPath; }
        set { elecMagicalPath = value; }
    }



    public int AUTO_PATH
    {
        get { return autoPath; }
        set { autoPath = value; }
    }

    public int PASS_PATH
    {
        get { return passivePath; }
        set { passivePath = value; }
    }

    public int OPP_PATH
    {
        get { return oppPath; }
        set { oppPath = value; }
    }
}
