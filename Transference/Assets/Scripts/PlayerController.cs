﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private SkillScript currentSkill;

    [SerializeField]
    public LivingObject current;

    [SerializeField]
    protected ManagerScript myManager;
    //public bool canMove = false;

    // Use this for initialization
    void Start()
    {
        myManager = GameObject.FindObjectOfType<ManagerScript>();
        if (!myManager.isSetup)
        {
            myManager.Setup();
        }
        if (myManager.turnOrder.Count > 0)
        {
            current = myManager.turnOrder[0];
        }
        // base.Start();
        // MoveDist = 1;
        // MAX_ATK_DIST = 2;
        // SkillManager.CreateSkill(1);
    }

    // Update is called once per frame
    void Update()
    {
        //base.Update();
        if (current)
        {

            switch (myManager.currentState)
            {
                case State.PlayerInput:
                    if (Input.GetKeyDown(KeyCode.W))
                    {
                        myManager.currentMenuitem--;
                        if (myManager.currentMenuitem < 0)
                        {
                            myManager.currentMenuitem = 5;
                        }
                        myManager.updateCurrentMenuPosition(myManager.currentMenuitem);
                    }

                    if (Input.GetKeyDown(KeyCode.S))
                    {
                        myManager.currentMenuitem++;
                        if (myManager.currentMenuitem > 5)
                        {
                            myManager.currentMenuitem = 0;
                        }
                        myManager.updateCurrentMenuPosition(myManager.currentMenuitem);
                    }
                    if (Input.GetKeyDown(KeyCode.Escape))
                    {
                        myManager.CancelMenuAction(current);
                    }
                    if (Input.GetKeyDown(KeyCode.Return))
                    {
                        myManager.SelectMenuItem(current);
                    }
                    break;
                case State.PlayerMove:
                    if (Input.GetKeyDown(KeyCode.W))
                    {
                        myManager.MoveGridObject(current, new Vector3(0, 0, 1));
                    }
                    if (Input.GetKeyDown(KeyCode.A))
                    {
                        myManager.MoveGridObject(current, new Vector3(-1, 0, 0));
                    }
                    if (Input.GetKeyDown(KeyCode.S))
                    {
                        myManager.MoveGridObject(current, new Vector3(0, 0, -1));
                    }
                    if (Input.GetKeyDown(KeyCode.D))
                    {
                        myManager.MoveGridObject(current, new Vector3(1, 0, 0));
                    }
                    if (Input.GetKeyDown(KeyCode.Return))
                    {
                        myManager.ComfirmMenuAction(current);
                    }
                    if (Input.GetKeyDown(KeyCode.Escape))
                    {
                        myManager.CancelMenuAction(current);
                    }
                    break;
                case State.PlayerAttacking:

                    if (Input.GetKeyDown(KeyCode.Return))
                    {
                        bool check = false;
                        if (currentSkill != null)
                        {
                            if (currentSkill.CanUse())
                            {
                                check = myManager.AttackTargets(current, currentSkill);
                                if (currentSkill != null)
                                    currentSkill = null;
                            }
                        }
                        else
                        {
                            check = myManager.AttackTargets(current, current.WEAPON);
                        }



                        if (check == true)
                        {

                            if (myManager.GetComponent<MenuManager>())
                            {
                                myManager.GetComponent<MenuManager>().ShowCommandCanvas();
                            }
                            myManager.prevState = myManager.currentState;
                            myManager.currentState = State.PlayerInput;
                        }

                    }
                    if (Input.GetKeyDown(KeyCode.Escape))
                    {
                        myManager.CancelMenuAction(current);
                        currentSkill = null;
                    }
                    break;
                case State.PlayerEquippingMenu:
                    if (Input.GetKeyDown(KeyCode.W))
                    {
                        myManager.currentMenuitem--;
                        if (myManager.currentMenuitem < 6)
                        {
                            myManager.currentMenuitem = 8;
                        }
                        myManager.updateCurrentMenuPosition(myManager.currentMenuitem);
                    }

                    if (Input.GetKeyDown(KeyCode.S))
                    {
                        myManager.currentMenuitem++;
                        if (myManager.currentMenuitem > 8)
                        {
                            myManager.currentMenuitem = 6;
                        }
                        myManager.updateCurrentMenuPosition(myManager.currentMenuitem);
                    }
                    if (Input.GetKeyDown(KeyCode.Return))
                    {
                        myManager.SelectMenuItem(current);
                    }
                    if (Input.GetKeyDown(KeyCode.Escape))
                    {
                        myManager.CancelMenuAction(current);
                    }
                    break;
                case State.PlayerEquipping:
                    {
                        if (Input.GetKeyDown(KeyCode.Return))
                        {
                            InventoryMangager invm = Object.FindObjectOfType<InventoryMangager>();

                            switch (invm.selectedMenuItem.refItem.TYPE)
                            {
                                case 0:
                                    if (invm.selectedMenuItem.refItem.GetType() == typeof(WeaponScript))
                                    {
                                        WeaponScript newWeapon = (WeaponScript)invm.selectedMenuItem.refItem;
                                        current.WEAPON.Equip(newWeapon);
                                    }
                                    break;
                                case 1:
                                    if (invm.selectedMenuItem.refItem.GetType() == typeof(ArmorScript))
                                    {
                                        ArmorScript newArmor = (ArmorScript)invm.selectedMenuItem.refItem;
                                        current.ARMOR.Equip(newArmor);
                                    }
                                    break;
                                case 2:
                                    if (invm.selectedMenuItem.refItem.GetType() == typeof(AccessoryScript))
                                    {
                                        AccessoryScript newAcc = (AccessoryScript)invm.selectedMenuItem.refItem;
                                        current.ACCESSORY.Equip(newAcc);
                                    }
                                    break;
                                case 3:
                                    if (invm.selectedMenuItem.refItem.GetType() == current.ACCESSORY.GetType())
                                    {
                                        AccessoryScript newAcc = (AccessoryScript)invm.selectedMenuItem.refItem;
                                        current.ACCESSORY.Equip(newAcc);

                                    }
                                    break;
                                case 4:
                                    {

                                        SkillScript selectedSkil = (SkillScript)invm.selectedMenuItem.refItem;
                                        if (selectedSkil.CanUse() == false)
                                        {
                                            break;
                                        }
                                        currentSkill = selectedSkil;
                                        myManager.attackableTiles = myManager.GetSkillsAttackableTiles(current, selectedSkil);
                                        myManager.ShowWhite();
                                        if (myManager.attackableTiles.Count > 0)
                                        {
                                            for (int i = 0; i < myManager.attackableTiles.Count; i++) //list of lists
                                            {
                                                for (int j = 0; j < myManager.attackableTiles[i].Count; j++) //indivisual list
                                                {

                                                    myManager.attackableTiles[i][j].myColor = Color.red;
                                                }
                                            }
                                            myManager.currentAttackList = myManager.attackableTiles[0];

                                            for (int i = 0; i < myManager.currentAttackList.Count; i++)
                                            {
                                                myManager.currentAttackList[i].myColor = Color.green;
                                            }


                                        }

                                        else
                                        {
                                            currentSkill = null;
                                            myManager.attackableTiles.Clear();
                                        }
                                        myManager.tempObject.transform.position = myManager.currentObject.transform.position;
                                        myManager.tempObject.GetComponent<GridObject>().currentTile = myManager.currentObject.currentTile;
                                        MenuManager myMenuManager = GameObject.FindObjectOfType<MenuManager>();
                                        if (myMenuManager)
                                        {
                                            myMenuManager.ShowNone();
                                        }
                                        myManager.prevState = myManager.currentState;
                                        myManager.currentState = State.PlayerAttacking;
                                    }
                                    break;
                            }

                        }
                        if (Input.GetKeyDown(KeyCode.Escape))
                        {
                            myManager.CancelMenuAction(current);
                            currentSkill = null;
                        }
                    }
                    break;
                case State.PlayerWait:
                    if (Input.GetKeyDown(KeyCode.Escape))
                    {
                        myManager.CancelMenuAction(current);
                    }
                    break;
                case State.FreeCamera:
                    if (Input.GetKeyDown(KeyCode.Return))
                    {
                        myManager.ComfirmMenuAction(current);
                    }

                    if (Input.GetKeyDown(KeyCode.Escape))
                    {
                        myManager.CancelMenuAction(current);
                    }
                    break;
                case State.EnemyTurn:
                    break;
                default:
                    break;
            }


        }
    }
}
