using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class NonCombatButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{

    public int type;
    public NonCombatController controller;
    public GameObject loadingCanvas = null;
    private Vector3 startLocation;
    public int specialNumber = -1;
    public NonCombatButtonAction myAction = NonCombatButtonAction.none;
    public NonCombatButtonRequirements requirements = NonCombatButtonRequirements.none;
    private void Awake()
    {
        CheckRequirements();
    }
    private void Start()
    {
        controller = GameObject.FindObjectOfType<NonCombatController>();
        startLocation = transform.position;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (controller)
        {
            LeanTween.scale(this.gameObject, new Vector3(1.2f, 1.2f, 1.2f), 0.2f);//.setOnComplete(x => { });
            LeanTween.moveLocalX(this.gameObject, 1.2f, 0.2f);
            controller.selectedButton.GetComponentInChildren<Text>().color = Color.white;
            controller.selectedButton = this;
            controller.selectedButton.GetComponentInChildren<Text>().color = Color.yellow;
            if (type < controller.targets.Length)
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

    public void executeAction()
    {
        if (controller)
        {

            switch (myAction)
            {
                case NonCombatButtonAction.none:
                    break;
                case NonCombatButtonAction.openChapterSelect:
                    controller.selectedCharacter = specialNumber;
                    controller.OpenChapterSelect();
                    break;
                case NonCombatButtonAction.setChapter:
                    controller.selectedChapter = specialNumber;
                    controller.OpenNewContinue();
                    break;
                case NonCombatButtonAction.newGame:
                    {
                        PlayerPrefs.SetInt("continue", -1);
                        Debug.Log("newGame");
                        PlayerPrefs.Save();

                        controller.loading = true;
                        if (controller.selectedCharacter == 1)
                        {
                            playJax();
                        }
                        else
                        {
                            playZeffron();
                        }
                    }
                    break;
                case NonCombatButtonAction.continueGame:
                    {
                        PlayerPrefs.SetInt("continue", 1);
                        Debug.Log("contune");
                        PlayerPrefs.Save();
                        controller.loading = true;
                        if (controller.selectedCharacter == 1)
                        {
                            playJax();
                        }
                        else
                        {
                            playZeffron();
                        }
                    }
                    break;
            }
        }
    }
    public void playJax()
    {
        if (loadingCanvas != null)
        {
            loadingCanvas.SetActive(true);
        }
        PlayerPrefs.SetInt("defaultSceneEntry", 26);
        SceneManager.LoadSceneAsync("DemoMap5");
    }


    public void playZeffron()
    {
        PlayerPrefs.SetInt("defaultSceneEntry", 4);
        SceneManager.LoadSceneAsync("DemoMap5");
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        if (controller != null)
            controller.HitButton();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        LeanTween.scale(this.gameObject, new Vector3(1.0f, 1.0f, 1.0f), 0.2f);//.setOnComplete(x => { });
        LeanTween.moveLocalX(this.gameObject, -1.2f, 0.2f);
    }

    public bool CheckRequirements()
    {
        bool turnOff = false;
        switch (requirements)
        {
            case NonCombatButtonRequirements.none:
                break;
            case NonCombatButtonRequirements.jaxSave1:
                {
                    if(!PlayerPrefs.HasKey(Common.JaxSaveSlot1))
                    {
                        turnOff = true;
                    }
                }
                break;
            case NonCombatButtonRequirements.jaxSave2:
                break;
            case NonCombatButtonRequirements.jaxSave3:
                break;
            case NonCombatButtonRequirements.jaxSave4:
                break;
            case NonCombatButtonRequirements.jaxSave5:
                break;
            default:
                break;
        }
        if(turnOff == true)
        {
            Image myImg = GetComponent<Image>();
            Color disabled;
            ColorUtility.TryParseHtmlString("#6C440B", out disabled);//;
            myImg.color = disabled;
            enabled = false;
        }

        return turnOff;
    }
}
