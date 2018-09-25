using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuStackManager : MonoBehaviour {
    ManagerScript manager;
    MenuManager menuManager;
    menuStackEntry cmd;
    menuStackEntry act;
    menuStackEntry skillsMain;
    menuStackEntry inventoryMain;
    menuStackEntry oppSelection;
    menuStackEntry oppOptions;
    menuStackEntry topEntry;
    menuStackEntry playerOptions;
    public bool isSetup = false;

    public void Setup()
    {
        if(!isSetup)
        {
            cmd = new menuStackEntry();
            skillsMain = new menuStackEntry();
            inventoryMain = new menuStackEntry();
            oppSelection = new menuStackEntry();
            oppOptions = new menuStackEntry();
            topEntry = new menuStackEntry();
            playerOptions = new menuStackEntry();
            act = new menuStackEntry();

            cmd.state = State.PlayerInput;
            cmd.menu = currentMenu.act;

            act.state = State.PlayerInput;
            act.menu = currentMenu.none;

            skillsMain.state = State.PlayerEquippingMenu;
            skillsMain.menu = currentMenu.invMain;

            inventoryMain.state = State.PlayerEquippingMenu;
            inventoryMain.menu = currentMenu.command;

            playerOptions.state = State.ChangeOptions;
            playerOptions.menu = currentMenu.command;

            oppOptions.state = State.PlayerOppOptions;
            oppOptions.menu = currentMenu.OppSelection;

            oppSelection.state = State.PlayerOppSelecting;
            oppSelection.menu = currentMenu.command;
         
            isSetup = true;
        }
    }
    private void Start()
    {
        Setup();
    }
    public menuStackEntry GetSkillStack()
    {
        return skillsMain;
    }

    public menuStackEntry GetInventoryStack()
    {
        return inventoryMain;
    }
    public menuStackEntry GetCmdStack()
    {
        return cmd;
    }

    public menuStackEntry GetActStack()
    {
        return act;
    }

    public menuStackEntry GetOptionsStack()
    {
        return playerOptions;
    }

    public menuStackEntry GetOppOptionsStack()
    {
        return oppOptions ;
    }

    public menuStackEntry GetOppSelectionStack()
    {
        return oppSelection;
    }

    public menuStackEntry GetTopStack()
    {
        return topEntry;
    }
}
