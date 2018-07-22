using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondStatusScript : MonoBehaviour {
    [SerializeField]
    SecondaryStatus status;

    [SerializeField]
    int countdown;

    public SecondaryStatus STATUS
    {
        get { return status; }
        set { status = value; }
    }

    public int COUNTDOWN
    {
        get { return countdown; }
        set { countdown = value; }
    }
    public void Activate()
    {
        if(status == SecondaryStatus.slow)
        {
            if(GetComponent<LivingObject>())
            {
                GetComponent<LivingObject>().STATS.MOVE_DIST -= Mathf.RoundToInt(GetComponent<LivingObject>().STATS.MOVE_DIST * 0.5f);
            }
        }
    }
    public void ReduceCount(ManagerScript manager, LivingObject living)
    {
        float chance = Random.Range(0, 2);
        Debug.Log("Chance = " + chance);
        switch (status)
        {
            case SecondaryStatus.slow:
                break;
            case SecondaryStatus.rage:
                break;
            case SecondaryStatus.charm:
                break;
            case SecondaryStatus.seal:
                break;
            case SecondaryStatus.poisoned:
                Debug.Log(living.FullName + " is poisoned");
                manager.DamageLivingObject(living, (int)(living.HEALTH * 0.1));
                if (chance > 0)
                {
                    Debug.Log(living.FullName + " is no longer poisoned");
                    Destroy(this);
                }
                break;
        }

        COUNTDOWN--;
        if(COUNTDOWN < 0)
        {
            if(GetComponent<LivingObject>())
            {
                if(status == SecondaryStatus.slow)
                {
                    GetComponent<LivingObject>().STATS.MOVE_DIST -= Mathf.RoundToInt(GetComponent<LivingObject>().STATS.MOVE_DIST * 0.5f);
                }

                GetComponent<LivingObject>().SSTATUS = SecondaryStatus.normal;
            }
            Destroy(this);
        }
    }
}
