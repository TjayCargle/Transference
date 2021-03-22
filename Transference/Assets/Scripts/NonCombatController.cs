
using UnityEngine;
using UnityEngine.UI;

public class NonCombatController : MonoBehaviour
{

    public NonCombatButton[] buttons;
    public NonCombatButton selectedButton;
    public Canvas ctrlCnvs;

    public Image currControl;
    //   public Sprite[] silouetes;
    public GameObject[] targets;
    public Image[] neotargets;
    public GameObject currTarget;
    public Image currNeoTarget;
    // [SerializeField]
    // Image siloute;
    public int buttonIndex = 0;
    public int controlIndex = 0;

    public CtrlsTitle title;
    public CtrlsDesc desc;
    public CtrlsButton selectedCtrlButton;

    public Canvas patchCnvs;
    public Canvas mainCanvas;
    public Canvas playCanvas;
    public GameObject musicCanvas;
    public GameObject cameraCanvas;
    public Animator[] animators;
    private AudioSource source;
    public AudioClip[] clips;
    public bool loading = false;
    public GameObject chapterSelectPanel;
    public GameObject newContinuePanel;
    public StorySection selectedCharacter = StorySection.none;
    public int storyFollow = 4;
    public int chapterFollow = -1;
    public GameObject animationCanvas;
    public CTTDemoManager cTT;
    
    //    public int selectedChapter = 1;
    // Use this for initialization
    void Start()
    {
        source = GetComponent<AudioSource>();
        selectedButton = buttons[0];
        //selectedButton.GetComponentInChildren<Text>().color = Color.yellow;
        selectedButton.GetComponent<Image>().color = Color.yellow;
        if (targets != null)
        {
            if (targets.Length > 0)
            {

                currTarget = targets[0];
            }
        }
        // controls = Resources.LoadAll<Sprite>("Demo/");
        if (ctrlCnvs)
        {
            ctrlCnvs.gameObject.SetActive(false);
        }

        if (patchCnvs)
        {
            patchCnvs.gameObject.SetActive(false);
        }
    }
    public void TogglePanel(GameObject panel)
    {
        if(panel != null)
        {
            panel.gameObject.SetActive(!panel.activeInHierarchy);

        }
    }
    public void SetPlay()
    {
        if (source)
        {
            if (clips.Length > 1)
            {
                source.clip = clips[1];
                source.Play();
            }
        }
        if (playCanvas)
        {
            playCanvas.gameObject.SetActive(true);
        }
        for (int i = 0; i < animators.Length; i++)
        {
            Animator animator = animators[i];
            animator.SetBool("Pressed Start", true);
        }
        if (mainCanvas)
        {
            mainCanvas.gameObject.SetActive(false);
        }

        if (musicCanvas)
        {
            musicCanvas.gameObject.SetActive(false);
        }

        if (cameraCanvas)
        {
            cameraCanvas.gameObject.SetActive(false);
        }

        if (chapterSelectPanel)
        {
            chapterSelectPanel.SetActive(false);
        }

        if (newContinuePanel)
        {
            newContinuePanel.SetActive(false);

        }
    }

    public void SetUnPlay()
    {
        if (source)
        {
            if (clips.Length > 0)
            {
                source.clip = clips[0];
                source.Play();
            }
        }
        for (int i = 0; i < animators.Length; i++)
        {
            Animator animator = animators[i];
            animator.SetBool("Pressed Start", false);
        }
        if (playCanvas)
        {
            playCanvas.gameObject.SetActive(false);
        }
        if (mainCanvas)
        {
            mainCanvas.gameObject.SetActive(true);
        }

        if (musicCanvas)
        {
            musicCanvas.gameObject.SetActive(false);
        }

        if (cameraCanvas)
        {
            cameraCanvas.gameObject.SetActive(false);
        }

        if (chapterSelectPanel)
        {
            chapterSelectPanel.SetActive(false);
        }

        if (newContinuePanel)
        {
            newContinuePanel.SetActive(false);

        }
    }

    public void OpenChapterSelect()
    {
        if (chapterSelectPanel)
        {
            chapterSelectPanel.SetActive(true);
        }

        if (newContinuePanel)
        {
            newContinuePanel.SetActive(false);

        }
    }
    public void OpenNewContinue()
    {

        if (newContinuePanel)
        {
            newContinuePanel.SetActive(true);

        }
    }
    public void TurnOff(GameObject someObj)
    {
        someObj.gameObject.SetActive(false);
    }

    public void SetMusic()
    {

        if (musicCanvas)
        {
            musicCanvas.gameObject.SetActive(true);
        }

        if (cameraCanvas)
        {
            cameraCanvas.gameObject.SetActive(false);
        }

    }

    public void SetCamera()
    {

        if (musicCanvas)
        {
            musicCanvas.gameObject.SetActive(false);
        }

        if (cameraCanvas)
        {
            cameraCanvas.gameObject.SetActive(true);
        }

    }

    public void HitButton()
    {
        if (!loading)
        {

            if (selectedButton)
            {

                switch (selectedButton.type)
                {
                    case 0:
                        selectedButton.PressStart();

                        break;

                    case 7:
                        selectedButton.PressMusix();

                        break;

                    case 8:
                        selectedButton.PressCam();

                        break;

                    case 1:
                        // selectedButton.PressControls();
                        // controlIndex = 0;
                        //  currControl.sprite = controls[controlIndex];
                        ctrlCnvs.gameObject.SetActive(true);
                        if (selectedCtrlButton)
                        {

                            if (selectedCtrlButton.myImage)
                            {
                                selectedCtrlButton.myImage.color = Color.yellow;
                            }
                            if (selectedCtrlButton.pro)
                            {
                                selectedCtrlButton.pro.color = Color.yellow;
                            }

                            selectedCtrlButton.ForceSelect();
                        }
                        break;
                    case 2:
                        {
                            for (int i = 0; i < animators.Length; i++)
                            {
                                Animator animator = animators[i];
                                animator.SetBool("Patched", true);
                            }
                            patchCnvs.gameObject.SetActive(true);

                        }
                        break;
                    case 3:
                        selectedButton.PressQuit();
                        break;

                    //case 3:
                    //   selectedButton.pressMain();
                    //    break;



                    case 4:
                        {

                            //       loading = true;
                            // selectedButton.playJax();
                            selectedButton.executeAction();
                        }

                        break;

                    case 5:
                        {

                            //        loading = true;
                            //  selectedButton.playZeffron();
                        }
                        break;
                    case 6:
                        SetUnPlay();
                        break;

                    default:
                        {
                            selectedButton.executeAction();

                        }
                        break;
                }
            }

            NonCombatButton[] buttons = GameObject.FindObjectsOfType<NonCombatButton>();
            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i].enabled = true;
                buttons[i].CheckRequirements();
                if (buttons[i].type == 5)
                {
                    buttons[i].storyFollow = storyFollow;
                }
            }

        }
    }
    // Update is called once per frame
    void Update()
    {
        if (!loading)
        {

            Vector3 targetLocation = Vector3.zero;
            if (currTarget)
            {
                targetLocation = currTarget.transform.position;
            }
            targetLocation.z -= 90;
            targetLocation.x -= 15;
            targetLocation.y += 15;
            if (Vector3.Distance(transform.position, targetLocation) > 2f)
            {
                transform.Translate((targetLocation - transform.position) * 0.5f);
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                if (buttons.Length > 0)
                {
                    selectedButton.GetComponentInChildren<Text>().color = Color.white;
                    if (buttonIndex + 1 < buttons.Length)
                    {
                        buttonIndex += 1;
                    }
                    else
                    {
                        buttonIndex = 0;
                    }

                    currTarget = targets[buttonIndex];

                    selectedButton = buttons[buttonIndex];
                    selectedButton.GetComponentInChildren<Text>().color = Color.yellow;
                }
            }

            if (Input.GetKeyDown(KeyCode.W))
            {
                if (buttons.Length > 0)
                {
                    selectedButton.GetComponentInChildren<Text>().color = Color.white;
                    if (buttonIndex - 1 >= 0)
                    {
                        buttonIndex -= 1;
                    }
                    else
                    {
                        buttonIndex = buttons.Length - 1;
                    }
                    currTarget = targets[buttonIndex];

                    selectedButton = buttons[buttonIndex];
                    selectedButton.GetComponentInChildren<Text>().color = Color.yellow;
                }
            }

            if (Input.GetKeyUp(KeyCode.Return))
            {
                HitButton();
            }


            if (ctrlCnvs)
            {

                if (ctrlCnvs.gameObject.activeInHierarchy)
                {

                    if (Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButton(1))
                    {
                        ctrlCnvs.gameObject.SetActive(false);
                    }

                    //if (Input.GetKeyDown(KeyCode.D))
                    //{
                    //    if (controlIndex + 1 < controls.Length)
                    //    {
                    //        controlIndex += 1;
                    //    }
                    //    else
                    //    {
                    //        controlIndex = 0;
                    //    }

                    //    currControl.sprite = controls[controlIndex];
                    //}

                    //if (Input.GetKeyDown(KeyCode.A))
                    //{
                    //    if (controlIndex - 1 >= 0)
                    //    {
                    //        controlIndex -= 1;
                    //    }
                    //    else
                    //    {
                    //        controlIndex = controls.Length - 1;
                    //    }

                    //    currControl.sprite = controls[controlIndex];
                    //}
                }
            }
            if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.LeftAlt) && Input.GetKey(KeyCode.T))
            {
                ctrlCnvs.gameObject.SetActive(true);

            }
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                ctrlCnvs.gameObject.SetActive(false);

            }
            if (patchCnvs)
            {

                if (patchCnvs.gameObject.activeInHierarchy)
                {

                    if (Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButton(1))
                    {
                        for (int i = 0; i < animators.Length; i++)
                        {
                            Animator animator = animators[i];
                            animator.SetBool("Patched", false);
                        }
                        patchCnvs.gameObject.SetActive(false);
                    }


                }
            }
        }
    }

}
