using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class NonCombatButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler
{

    public int type;
    public NonCombatController controller;
    private void Start()
    {
        controller = GameObject.FindObjectOfType<NonCombatController>();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        controller.selectedButton.GetComponentInChildren<Text>().color = Color.white;
        controller.selectedButton = this;
        controller.selectedButton.GetComponentInChildren<Text>().color = Color.yellow;
        if(type < 3)
        {
            controller.buttonIndex = type;
            controller.currTarget = controller.targets[type];
        }
    }
  //  public void OnPointerDown(PointerEventData eventData)
  //  {
   //     controller.HitButton();
   // }
   public void PressStart()
   {

       SceneManager.LoadSceneAsync("DemoMap4");


   }

   public void PressControls()
   {

   }

   public void PressQuit()
   {
       Application.Quit();
   }
   public void pressMain()
   {
       SceneManager.LoadScene("start");
   }

    public void OnPointerClick(PointerEventData eventData)
    {
        controller.HitButton();
    }
}
