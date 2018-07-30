using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NonCombatButton : MonoBehaviour {

    public int type;
    public void PressStart()
    {
   
        SceneManager.LoadScene("DemoMap");

    
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
	
}
