using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionalDisplay : MonoBehaviour
{

    public State[] displayStates;
    public bool requiresSelected = false;
    public bool isAffectDisplay = false;
    public bool requiresDoorTile = false;
    public bool requiresShopTile = false;
    public bool requriesSelectedLiving = false;
    public bool checksForEvent = false;
    public bool requiresSelectedorSpecialTile = false;
    ManagerScript manager;
    public bool debugging = false;
    public bool checksForGlyph = false;
    public bool checksForEnemy = false;
    public bool checkForHelpTile = false;

    public bool playerHasSkills = false;
    public bool playerHasSpells = false;
    public bool playerHasStrikes = false;
    public bool playerHasBarriers = false;
    public bool playerHasItems = false;
    // Use this for initialization
    void Start()
    {
        manager = GameObject.FindObjectOfType<ManagerScript>();
    }

    //public void UpdateDisplay()
    //{
    //    gameObject.SetActive(false);
    //    if (manager)
    //    {
    //        for (int i = 0; i < displayStates.Length; i++)
    //        {
    //            if (manager. GetState() == displayStates[i])
    //            {
    //                gameObject.SetActive(true);

    //                break;
    //            }
    //        }

    //    }

    //}

    public void UpdateDisplay(ManagerScript newmanager)
    {
        manager = newmanager;
        gameObject.SetActive(false);
        bool found = false;
        if (requiresSelected == true)
        {
            if (manager.myCamera.infoObject == null)
            {
                if(debugging)
                {
                    Debug.Log("no info obj");
                }
                gameObject.SetActive(false);
                return;
            }
            if (isAffectDisplay)
            {
                if (manager)
                {
                    LivingObject livvy = manager.myCamera.infoObject.GetComponent<LivingObject>();
                    if (livvy)
                    {
                        if (livvy.INVENTORY.DEBUFFS.Count > 0 || livvy.INVENTORY.BUFFS.Count > 0 || livvy.INVENTORY.EFFECTS.Count > 0|| livvy.GetComponent<SecondStatusScript>())
                        {
                            gameObject.SetActive(true);
                        }

                    }
                }
            }

        }



        if (requriesSelectedLiving == true)
        {
            if (manager.myCamera.infoObject == null)
            {

                gameObject.SetActive(false);
                return;
            }
            else if (!manager.myCamera.infoObject.GetComponent<LivingObject>())
            {

                gameObject.SetActive(false);
                return;
            }
        }


        if (manager)
        {
            if (manager.menuManager.isGameOverShowing() == true) 
            {
                gameObject.SetActive(false);
                return;
            }

            if(manager.player)
            {
                if(manager.player.current)
                {
                    if(playerHasSkills == true)
                    {
                        if(manager.player.current.PHYSICAL_SLOTS.SKILLS.Count == 0)
                        {
                            gameObject.SetActive(false);
                            return;
                        }
                    }
                    if (playerHasSpells == true)
                    {
                        if (manager.player.current.MAGICAL_SLOTS.SKILLS.Count == 0)
                        {
                            gameObject.SetActive(false);
                            return;
                        }
                    }
                    if (playerHasStrikes == true)
                    {
                        if (manager.player.current.INVENTORY.WEAPONS.Count == 0)
                        {
                            gameObject.SetActive(false);
                            return;
                        }
                    }
                    if (playerHasBarriers == true)
                    {
                        if (manager.player.current.INVENTORY.ARMOR.Count == 0)
                        {
                            gameObject.SetActive(false);
                            return;
                        }
                    }
                    if (playerHasItems == true)
                    {
                        if (manager.player.current.INVENTORY.ITEMS.Count == 0)
                        {
                            gameObject.SetActive(false);
                            return;
                        }
                    }
                }
            
            }

            if (requiresDoorTile == true)
            {
                if (manager.myCamera.currentTile)
                {

                    if (manager.myCamera.currentTile.TTYPE != TileType.door)
                    {
                        gameObject.SetActive(false);
                        return;
                    }
                }
            }
            if (requiresSelectedorSpecialTile == true)
            {
                if (manager.myCamera.selectedTile)
                {

                    if (manager.myCamera.infoObject == null && manager.myCamera.selectedTile.TTYPE == TileType.regular)
                    {

                        gameObject.SetActive(false);
                        return;
                    }
                }
               
            }
            if (checksForGlyph == true)
            {
                if (manager.myCamera.infoObject == null)
                {
                    gameObject.SetActive(false);
                    return;
                }
                if (manager.myCamera.infoObject != manager.player.current)
                {
                    gameObject.SetActive(false);
                    return;
                }
                if (!manager.CheckAdjecentTilesGlyphs(manager.player.current))
                {
                    gameObject.SetActive(false);
                    return;
                }
            }
            if(checkForHelpTile == true)
            {
                if(!manager.myCamera)
                {
                    gameObject.SetActive(false);
                    return;
                }
                if(!manager.myCamera.currentTile)
                {
                    gameObject.SetActive(false);
                    return;
                }
                if(manager.myCamera.currentTile.TTYPE != TileType.help)
                {
                    if(Common.isOverrideTile(manager.myCamera.currentTile) == false)
                    {
                
                            gameObject.SetActive(false);
                            return;                      
                
                    }
                }
            }
            if (checksForEnemy == true)
            {
                gameObject.SetActive(false);
                return;
                if (manager.myCamera.infoObject == null)
                {
                    gameObject.SetActive(false);
                    return;
                }
                if (manager.myCamera.infoObject != manager.player.current)
                {
                    gameObject.SetActive(false);
                    return;
                }
                if (!manager.CheckAdjecentTilesEnemy(manager.player.current))
                {
       
                }
            }
            if (checksForEvent == true)
            {
                if (manager.myCamera.infoObject == null)
                {
                    gameObject.SetActive(false);
                    return;
                }
                if (manager.myCamera.infoObject != manager.player.current)
                {
                    gameObject.SetActive(false);
                    return;
                }
                if (!manager.CheckAdjecentTilesEvent(manager.player.current))
                {
                    gameObject.SetActive(false);
                    return;
                }


            }

            if (requiresShopTile == true)
            {
                if (manager.myCamera)
                {
                    if (manager.myCamera.currentTile)
                    {
                        if (manager.myCamera.currentTile.TTYPE != TileType.shop)
                        {
                            gameObject.SetActive(false);
                            return;
                        }
                    }
                }
            }
            gameObject.SetActive(true);

            for (int i = 0; i < displayStates.Length; i++)
            {
                if (manager. GetState() == displayStates[i])
                {
                
                    found = true;
                    break;
                }
            }

            if (found == false)
            {
                gameObject.SetActive(false);
                if(debugging == true)
                {
                    Debug.Log("found false");
                }
            }
        }
        else
        {
            Debug.Log("no manager");
        }

    }
}
