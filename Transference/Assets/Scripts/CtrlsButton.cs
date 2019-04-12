
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
public class CtrlsButton : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler
{

    public int type;
    public NonCombatController controller;
    public string titlestr;
    public string descstr;
    public Image myImage;
    public TextMeshProUGUI pro;
    private void Start()
    {
        myImage = GetComponent<Image>();
        pro = GetComponentInChildren<TextMeshProUGUI>();
        controller = GameObject.FindObjectOfType<NonCombatController>();
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        {
            //if(controller)
            //  {
            //      if(type == 0)
            //      {
            //          if (controller.controlIndex - 1 >= 0)
            //          {
            //              controller.controlIndex -= 1;
            //          }
            //          else
            //          {
            //              controller.controlIndex = controller.controls.Length -1;
            //          }

            //          controller.currControl.sprite = controller.controls[controller.controlIndex];
            //      }
            //      else
            //      {
            //          if (controller.controlIndex + 1 < controller.controls.Length)
            //          {
            //              controller.controlIndex += 1;
            //          }
            //          else
            //          {
            //              controller.controlIndex = 0;
            //          }

            //          controller.currControl.sprite = controller.controls[controller.controlIndex];
            //      }
            //  }

        }
        if (eventData.button == PointerEventData.InputButton.Left)
        {

            if (controller == null)
            {
                controller = GameObject.FindObjectOfType<NonCombatController>();
            }
            if (controller)
            {
                if (controller.selectedCtrlButton != this)
                {
                    if (controller.selectedCtrlButton.myImage)
                    {
                        controller.selectedCtrlButton.myImage.color = Color.white;
                    }
                    if (controller.selectedCtrlButton.pro)
                    {
                        controller.selectedCtrlButton.pro.color = Color.white;
                    }
                }
                controller.selectedCtrlButton = this;
            }
   
        }
    }
    public void ForceSelect()
    {
        if (controller == null)
        {
            controller = GameObject.FindObjectOfType<NonCombatController>();
        }
        if (controller)
        {

            if (controller.title)
            {
                TextMeshProUGUI titletext = controller.title.GetComponentInChildren<TextMeshProUGUI>();
                if (titletext)
                {
                    titletext.text = titlestr;
                }
            }

            if (controller.desc)
            {
                TextMeshProUGUI desctext = controller.desc.GetComponentInChildren<TextMeshProUGUI>();
                if (desctext)
                {
                    desctext.text = descstr;
                }
            }
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (myImage)
        {
            myImage.color = Color.yellow;
        }
        if (pro)
        {
            pro.color = Color.yellow;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (controller)
        {
            if (controller.selectedCtrlButton != this)
            {
                if (myImage)
                {
                    myImage.color = Color.white;
                }
                if (pro)
                {
                    pro.color = Color.white;
                }
            }
        }
        else
        {
            if (myImage)
            {
                myImage.color = Color.white;
            }
            if (pro)
            {
                pro.color = Color.white;
            }
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        ForceSelect();
    }
}
