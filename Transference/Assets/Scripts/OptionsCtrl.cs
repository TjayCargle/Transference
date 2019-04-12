using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class OptionsCtrl : MonoBehaviour
{
   public Slider myslider;
   public Toggle myToggle;
   public Image myImage;
   public void Select()
    {
        if(myImage)
        {
            myImage.color = Color.yellow;
        }
    }
    public void DeSelect()
    {
        if (myImage)
        {
            myImage.color = Color.white;
        }
    }
    public void PressToggle()
    {
        if(myToggle)
        {
            myToggle.isOn = !myToggle.isOn;
        }
    }
    public void MoveSliderLeft()
    {
        if(myslider)
        {
            myslider.value -= 0.1f;
        }
    }
    public void MoveSliderRight()
    {
        if (myslider)
        {
            myslider.value += 0.1f;
        }
    }
}
