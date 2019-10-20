using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HackingButton : MonoBehaviour, IPointerDownHandler
{

    public HackingCtrl control;
    public bool isSetup = false;
    [SerializeField]
    public int hackType;

    public void OnPointerDown(PointerEventData eventData)
    {
       if(control)
        {
            switch(hackType)
            {
                case 0:
                    {
                        control.useStrike(this);
                    }
                    break;
                case 1:
                    {
                        control.useSkill(this);
                    }
                    break;

                case 2:
                    {
                        control.useSpell(this);
                    }
                    break;
            }
        }
    }

    public void Setup()
    {
        if(!isSetup)
        {
            if(!control)
            {
                control = GameObject.FindObjectOfType<HackingCtrl>();
            }
            isSetup = true;
        }
    }
    private void OnEnable()
    {
        if(control)
        {
            HackingButton caller = this;
            if (caller.hackType == 0)
            {
                if (control.strikeCount > 0)
                    caller.GetComponent<Button>().interactable = true;
                else
                    caller.GetComponent<Button>().interactable = false;
            }
            else if (caller.hackType == 1)
            {
                if (control.skillCount > 0)
                    caller.GetComponent<Button>().interactable = true;
                else
                    caller.GetComponent<Button>().interactable = false;
            }
            else if (caller.hackType == 2)
            {
                if (control.spellCount > 0)
                    caller.GetComponent<Button>().interactable = true;
                else
                    caller.GetComponent<Button>().interactable = false;
            }

        }
    }
    private void Start()
    {
        Setup();
    }
}
