using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NonCombatController : MonoBehaviour
{

    public NonCombatButton[] buttons;
    public NonCombatButton selectedButton;
    public Canvas ctrlCnvs;
    public Sprite[] controls;
    public Image currControl;
    int buttonIndex = 0;
    int controlIndex = 0;
    // Use this for initialization
    void Start()
    {
        selectedButton = buttons[0];
        selectedButton.GetComponentInChildren<Text>().color = Color.yellow;
        // controls = Resources.LoadAll<Sprite>("Demo/");
        if (ctrlCnvs)
        {
            ctrlCnvs.gameObject.SetActive(false);
        }
    }

    public void HitButton()
    {
        if (selectedButton)
        {

            switch (selectedButton.type)
            {
                case 0:
                    selectedButton.PressStart();
                    break;

                case 1:
                    // selectedButton.PressControls();
                    controlIndex = 0;
                    currControl.sprite = controls[controlIndex];
                    ctrlCnvs.gameObject.SetActive(true);
                    break;

                case 2:
                    selectedButton.PressQuit();
                    break;

                case 3:
                    selectedButton.pressMain();
                    break;
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
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

                selectedButton = buttons[buttonIndex];
                selectedButton.GetComponentInChildren<Text>().color = Color.yellow;
            }
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            HitButton();
        }


        if (ctrlCnvs)
        {

            if (ctrlCnvs.gameObject.activeInHierarchy)
            {

                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    ctrlCnvs.gameObject.SetActive(false);
                }

                if (Input.GetKeyDown(KeyCode.D))
                {
                    if (controlIndex + 1 < controls.Length)
                    {
                        controlIndex += 1;
                    }
                    else
                    {
                        controlIndex = 0;
                    }

                    currControl.sprite = controls[controlIndex];
                }

                if (Input.GetKeyDown(KeyCode.A))
                {
                    if (controlIndex - 1 >= 0)
                    {
                        controlIndex -= 1;
                    }
                    else
                    {
                        controlIndex = controls.Length - 1;
                    }

                    currControl.sprite = controls[controlIndex];
                }
            }
        }
    }

}
