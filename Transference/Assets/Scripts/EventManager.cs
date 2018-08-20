using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : EventRunner
{

    // public delegate bool RunEvent();
    public List<GridEvent> gridEvents;
    public bool ir = false;
    // float time = 1.0f;
    public int activeEvents = 0;
    public Object obj;
    public bool isSetup = false;
    public GridEvent currentEvent;
    // public List<string> completed;
    public void Setup()
    {
        if (!isSetup)
        {
            gridEvents = new List<GridEvent>();
            currentEvent.data = null;
            isSetup = true;
        }
    }
    public void Start()
    {
        Setup();
        // completed = new List<string>();
    }

    private void Update()
    {


        activeEvents = gridEvents.Count;
        if (currentEvent.caller == null)
        {
            obj = currentEvent.data;
            ir = currentEvent.isRunning;

            // GridEvent eve = gridEvents[0];
            if (gridEvents.Count > 0)
            {

                if (currentEvent.isRunning == false)
                {
                    currentEvent = gridEvents[0];//Dequeue();
                    gridEvents.Remove(gridEvents[0]);
                    currentEvent.isRunning = true;
                    if (currentEvent.START != null)
                        currentEvent.START();
                    if (currentEvent.STARTW != null)
                        currentEvent.STARTW(currentEvent.data);
            //        Debug.Log("Starting event: " + currentEvent.name + " from " + currentEvent.caller);

                }
            }
        }
        else
        {

            if (currentEvent.RUNABLE(currentEvent.data) == true)
            {
                // completed.Add(eve.name);
              //  Debug.Log("Finished event: " + currentEvent.name + " from " + currentEvent.caller);
                currentEvent.isRunning = false;
                currentEvent.caller = null;
                currentEvent.isRunning = false;
                currentEvent.data = null;
            }
        }


    }
}
