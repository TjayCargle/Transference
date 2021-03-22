using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
public class NonCombatButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{

    public int type;
    public NonCombatController controller;
    public GameObject loadingCanvas = null;
    private Vector3 startLocation;
    public StorySection specialNumber = StorySection.none;
    public NonCombatButtonAction myAction = NonCombatButtonAction.none;
    public StorySection requirements = StorySection.none;
    public float enterLocation = 1.2f;
    public float exitLocation = -1.2f;
    public int storyFollow = 4;
    public int chapterSelect = -1;

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
            LeanTween.moveLocalX(this.gameObject, enterLocation, 0.2f);
            // controller.selectedButton.GetComponentInChildren<Text>().color = Color.white;

            if (type < controller.targets.Length)
            {
                controller.buttonIndex = type;
                controller.currTarget = controller.targets[type];
            }

            if (type == 4 || type == 5)
            {
                if (controller.currNeoTarget)
                    controller.currNeoTarget.color = Common.blackened;
                controller.currNeoTarget = controller.neotargets[storyFollow - 4];
                controller.currNeoTarget.color = Color.white;

                Color brown;
                ColorUtility.TryParseHtmlString("#FF9500", out brown);//;

                controller.selectedButton.GetComponent<Image>().color = brown;
                controller.selectedButton = this;
                //controller.selectedButton.GetComponentInChildren<Text>().color = Color.yellow;
                controller.selectedButton.GetComponent<Image>().color = Color.yellow;
            }
            else
            {
                if (controller.currNeoTarget)
                    controller.currNeoTarget.color = Common.blackened;

                controller.selectedButton.GetComponent<Image>().color = Color.white;
                controller.selectedButton = this;
                //controller.selectedButton.GetComponentInChildren<Text>().color = Color.yellow;
                controller.selectedButton.GetComponent<Image>().color = Color.yellow;
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
                    switch (specialNumber)
                    {

                        case StorySection.JaxSaveSlot1:
                            controller.storyFollow = 4;
                            break;
                        case StorySection.JaxSaveSlotPrologue:
                            controller.storyFollow = 4;
                            break;

                        case StorySection.ZeffSaveSlot1:
                            controller.storyFollow = 5;
                            break;
                        case StorySection.ZeffSaveSlotPrologue:
                            controller.storyFollow = 5;
                            break;

                        case StorySection.PryinaSaveSlot1:
                            controller.storyFollow = 6;
                            break;
                        case StorySection.PyrinaPrologue:
                            controller.storyFollow = 6;
                            break;

                        case StorySection.FlaraSaveSlot1:
                            controller.storyFollow = 7;
                            break;
                        case StorySection.FlaraSaveSlotPrologue:
                            controller.storyFollow = 7;
                            break;

                        case StorySection.SapphireSaveSlot1:
                            controller.storyFollow = 8;
                            break;
                        case StorySection.SapphireSaveSlotPrologue:
                            controller.storyFollow = 8;
                            break;

                        case StorySection.LukonSaveSlot1:
                            controller.storyFollow = 9;
                            break;
                        case StorySection.LukonSaveSlotPrologue:
                            controller.storyFollow = 9;
                            break;
                    }
                    controller.OpenChapterSelect();

                    break;
                case NonCombatButtonAction.setChapter:
                    controller.chapterFollow = chapterSelect;
                    controller.OpenNewContinue();
                    break;
                case NonCombatButtonAction.newGame:
                    {
                        StorySection currentStory = controller.selectedCharacter;
                        if (controller.chapterFollow == 0)
                            currentStory += 5;
                        else if (controller.chapterFollow > 0)
                            currentStory += controller.chapterFollow - 1;
                        Common.currentStory = currentStory;
                        PlayNew();
                    }
                    break;
                case NonCombatButtonAction.continueGame:
                    {
                        StorySection currentStory = controller.selectedCharacter;
                        if (controller.chapterFollow == 0)
                            currentStory += 5;
                        else if (controller.chapterFollow > 0)
                            currentStory += controller.chapterFollow - 1;
                        Common.currentStory = currentStory; PlayContinue();
                    }
                    break;
            }
        }
    }

    public void PlayNew()
    {
        PlayerPrefs.SetInt("continue", -1);
        Debug.Log("newGame");
        PlayerPrefs.Save();
        int sceneEntry = Common.GetDefaultSceneEntry(controller.selectedCharacter);
        controller.loading = true;
        if(loadingCanvas != null)
        {
        loadingCanvas.SetActive(true);
        }
        PlayerPrefs.SetInt("defaultSceneEntry", sceneEntry);
        if(controller.cTT != null)
        {
            CTTDemoManager cTTD = controller.cTT;
            if (cTTD.nameText != null)
            {
                switch(controller.storyFollow)
                {
                    case 4:
                        cTTD.nameText.text = "Jax Drix";
                        cTTD.SetColors(Color.red, Common.pink);
                        break;

                    case 5:
                        cTTD.nameText.text = "Zeffron Drix";
                        break;

                    case 6:
                        cTTD.nameText.text = "Pyrina Phixie";
                        break;

                    case 7:
                        cTTD.nameText.text = "Flara Phixie";
                        break;


                    case 8:
                        cTTD.nameText.text = "Sapphire Witchcoven";
                        break;

                    case 9:
                        cTTD.nameText.text = "Lukon Hedgewolf";
                        break;

                   

                }
            }

            if (cTTD.chapterText != null)
            {
                string display = "Chapter ";
                if (controller.chapterFollow == 0)
                    display = "Prologue";
                else if (controller.chapterFollow > 0)
                    display += "" + controller.chapterFollow;

                cTTD.chapterText.text = display;
            }

            if (cTTD.beginEndText != null)
            {
                cTTD.beginEndText.text = "begin!";
            }
        }
        controller.TogglePanel(controller.animationCanvas);
        LeanTween.moveX(gameObject, 0, 250 * Time.deltaTime).setOnComplete(() =>
        {

            controller.TogglePanel(controller.animationCanvas);
            SceneManager.LoadSceneAsync("DemoMap5");
        });


    }

    public void PlayContinue()
    {
        PlayerPrefs.SetInt("continue", 1);
        Debug.Log("contune");
        PlayerPrefs.Save();
        int sceneEntry = Common.GetDefaultSceneEntry(controller.selectedCharacter);
        controller.loading = true;
        PlayerPrefs.SetInt("defaultSceneEntry", sceneEntry);
        SceneManager.LoadSceneAsync("DemoMap5");
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
        LeanTween.moveLocalX(this.gameObject, exitLocation, 0.2f);
    }

    public bool CheckRequirements()
    {
        bool turnOff = false;
        //        if(myAction == NonCombatButtonAction.setChapter && specialNumber != StorySection.none)

        if (myAction == NonCombatButtonAction.setChapter)
        {
            if (specialNumber != StorySection.JaxSaveSlot5)
            {
                requirements = (StorySection)((int)controller.selectedCharacter + (int)specialNumber);
            }

        }

        if (requirements != StorySection.none)
        {

            if (!PlayerPrefs.HasKey(requirements.ToString()))
            {
                turnOff = true;
            }
        }



        if (turnOff == true)
        {
            Image myImg = GetComponent<Image>();
            Color disabled;
            ColorUtility.TryParseHtmlString("#6C440B", out disabled);//;
            myImg.color = disabled;
            enabled = false;
        }
        else
        {
            if (type == 4)
            {
                Image myImg = GetComponent<Image>();
                Color enabledColor;
                ColorUtility.TryParseHtmlString("#FF9500", out enabledColor);//;
                myImg.color = enabledColor;
                enabled = true;
                if (GetComponentInChildren<TextMeshProUGUI>())
                {

                    GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
                }

                if (GetComponentInChildren<Text>())
                {

                    GetComponentInChildren<Text>().color = Color.white;
                }
            }

        }

        return turnOff;
    }
}
