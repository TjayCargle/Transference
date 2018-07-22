using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventRunner : MonoBehaviour {

    private RunableEvent runable;

    protected RunableEvent RUNABLE
    {
        get { return runable;}

        set { runable = value; }
    }
}
