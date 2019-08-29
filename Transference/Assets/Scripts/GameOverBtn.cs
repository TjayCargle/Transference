using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameOverBtn : MonoBehaviour, IPointerEnterHandler
{
    public int type;
    public string desc;
    ManagerScript manager;
    TextMeshProUGUI gameoverText;
    Button mybutton;
    UsedText used;
    private void Start()
    {
        manager = GameObject.FindObjectOfType<ManagerScript>();
        GameOverText gmt = GameObject.FindObjectOfType<GameOverText>();
        mybutton = GetComponent<Button>();
        used = GetComponentInChildren<UsedText>();


        if (mybutton)
        {
            mybutton.onClick.AddListener(PressButton);
        }
        if (gmt)
        {
            gameoverText = gmt.GetComponent<TextMeshProUGUI>();
        }

        if (used)
        {
            used.gameObject.SetActive(false);
        }
    }
    public void PressButton()
    {
        switch (type)
        {
            case 0:
                {
                    //full revive
                    //Revive each character with full hp,mp, ft, but halve all other stats and lose most used skill, most used spell, and all unequipped Barriers and Strikes.
                    if (manager)
                    {
                        manager.ReviveFull();
                        manager.forceReloadRound(Faction.ally);
                        manager.menuManager.DontShowGameOver();
                        if (mybutton)
                        {
                            mybutton.interactable = false;
                            if (used)
                            {
                                used.gameObject.SetActive(true);
                            }
                        }
                    }
                }
                break;
            case 1:
                {
                    //helping hand
                    //Revive each character with half hp/mp/ft but lose all passsive, auto, and opportunity skills.
                    if (manager)
                    {
                        manager.ReviveMedium();
                        manager.forceReloadRound(Faction.ally);
                        manager.menuManager.DontShowGameOver();
                        if (mybutton)
                        {
                            mybutton.interactable = false;
                            if (used)
                            {
                                used.gameObject.SetActive(true);
                            }
                        }
                    }
                }
                break;
            case 2:
                {
                    //second chance
                    //Revive each character with 1 hp, but halves all stats and halves Max HP, MP, and FT.
                    if (manager)
                    {
                        manager.ReviveLow();
                        manager.forceReloadRound(Faction.ally);
                        manager.menuManager.DontShowGameOver();
                        if (mybutton)
                        {
                            mybutton.interactable = false;
                            if(used)
                            {
                                used.gameObject.SetActive(true);
                            }
                        }
                    }
                }
                break;
            case 3:
                {
                    //

                    SceneManager.LoadScene("start");
                }
                break;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (gameoverText)
        {
            gameoverText.text = desc;
        }
    }
}
