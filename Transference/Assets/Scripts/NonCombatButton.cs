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
    public GameObject loadingCanvas = null;
    private void Start()
    {
        controller = GameObject.FindObjectOfType<NonCombatController>();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (controller)
        {

            controller.selectedButton.GetComponentInChildren<Text>().color = Color.white;
            controller.selectedButton = this;
            controller.selectedButton.GetComponentInChildren<Text>().color = Color.yellow;
            // if(type < 3)
            {
                controller.buttonIndex = type;
                controller.currTarget = controller.targets[type];
            }

            if (type == 4 || type == 5)
            {
                if (controller.currNeoTarget)
                    controller.currNeoTarget.color = Common.blackened;
                controller.currNeoTarget = controller.neotargets[type - 4];
                controller.currNeoTarget.color = Color.white;
            }
            else
            {
                if (controller.currNeoTarget)
                    controller.currNeoTarget.color = Common.blackened;
            }


        }
        //if (type > 4)
        //{
        //    controller.buttonIndex = type;
        //    controller.currTarget = controller.targets[type - 1];
        //}
    }
    //  public void OnPointerDown(PointerEventData eventData)
    //  {
    //     controller.HitButton();
    // }
    public void PressStart()
    {

        //SceneManager.LoadSceneAsync("DemoMap4");

        controller.SetPlay();
    }

    public void PressMusix()
    {

        controller.SetMusic();
    }


    public void PressCam()
    {

        controller.SetCamera();
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

    public void playJax()
    {
        PlayerPrefs.SetInt("defaultSceneEntry", 26);
        SceneManager.LoadSceneAsync("DemoMap5");
        if (loadingCanvas != null)
        {
            loadingCanvas.SetActive(true);
        }
    }


    public void playZeffron()
    {
        PlayerPrefs.SetInt("defaultSceneEntry", 4);
        SceneManager.LoadSceneAsync("DemoMap45");
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        if (controller != null)
            controller.HitButton();
    }
}
