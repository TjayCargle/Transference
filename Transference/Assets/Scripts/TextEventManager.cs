using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextEventManager : MonoBehaviour
{
    [SerializeField]
    public int actiEvents;
    public List<TextEvent> textEvents;
    public TextEvent currentEvent;
    public bool isSetup = false;
    public FlavorTextImg flavor;
    public void Setup()
    {
        if (!isSetup)
        {
            // flavor = FindObjectOfType<FlavorTextImg>();
            textEvents = new List<TextEvent>();
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

        if(textEvents == null)
        {
            textEvents = new List<TextEvent>();
        }
        actiEvents = textEvents.Count;
        // Debug.Log("ActiEvents = " + actiEvents);
        if (currentEvent.caller == null)
        {

            // GridEvent eve = gridEvents[0];
            if (textEvents.Count > 0)
            {
              //  Debug.Log("GOT AN EVENT BOUS");
                //  Debug.Log(textEvents.Count);
                if (currentEvent.isRunning == false)
                {
                   //  Debug.Log("not running BOUS");
                    currentEvent = textEvents[0];//Dequeue();
                                                 //  Debug.Log(currentEvent.data);
                    textEvents.Remove(textEvents[0]);
                    currentEvent.isRunning = true;
                    if (currentEvent.START != null)
                    {
                        currentEvent.START();
                    }
                    if (flavor)
                    {

                        flavor.theText = currentEvent.data;
                        flavor.SetText();
                    }


                      //   Debug.Log("Starting event: " + currentEvent.name + " from " + currentEvent.caller);

                }
            }
        }
        else
        {
            //    Debug.Log("chedftye AN EVENT BOUS");
            if (currentEvent.RUNABLE(null) == true)
            {
                // completed.Add(eve.name);
                //   Debug.Log("Finished event: " + currentEvent.name + " from " + currentEvent.caller);
                // Debug.Log("FINISHED: " + currentEvent.data);
                currentEvent.isRunning = false;
                currentEvent.caller = null;
                currentEvent.isRunning = false;
                currentEvent.data = null;
            }
        }


        //if (textEvents.Count <= 0)
        //{
        //    if (flavor)
        //    {
        //        if (flavor.gameObject.activeInHierarchy)
        //        {
        //            flavor.gameObject.SetActive(false);
        //        }
        //    }
        //}
    }
}
