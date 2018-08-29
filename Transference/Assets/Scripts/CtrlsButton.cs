using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CtrlsButton : MonoBehaviour, IPointerDownHandler
{

    public int type;
    public NonCombatController controller;

    public void OnPointerDown(PointerEventData eventData)
    {
      if(controller)
        {
            if(type == 0)
            {
                if (controller.controlIndex - 1 >= 0)
                {
                    controller.controlIndex -= 1;
                }
                else
                {
                    controller.controlIndex = controller.controls.Length -1;
                }

                controller.currControl.sprite = controller.controls[controller.controlIndex];
            }
            else
            {
                if (controller.controlIndex + 1 < controller.controls.Length)
                {
                    controller.controlIndex += 1;
                }
                else
                {
                    controller.controlIndex = 0;
                }

                controller.currControl.sprite = controller.controls[controller.controlIndex];
            }
        }
    }
}
