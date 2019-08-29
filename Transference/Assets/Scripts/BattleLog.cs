using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BattleLog : MonoBehaviour
{

    public TextMeshProUGUI proText;
    RectTransform rect;
    private Vector2 trueSize = Vector2.up;
    private void Start()
    {
        if(proText)
        {

        rect = proText.GetComponent<RectTransform>();
        }
      
    }

    public void Log(string text)
    {
        if(proText)
        {
            proText.text += "\n" + text;

            if(rect)
            {
                //if(trueSize == Vector2.up)
                //{
                //    trueSize = rect.sizeDelta;
                //}

                rect.sizeDelta = rect.sizeDelta + new Vector2(0,proText.fontSize + text.Length);
            }
        }
    }

    public void Clear()
    {
        if(proText)
        {
            proText.text = "";
        }

        if (rect)
        {

            rect.sizeDelta = trueSize;
        }
    }
 

}
