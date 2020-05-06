using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextObjectHandler : MonoBehaviour
{


    public static void UpdateText(TextObj obj, string newText)
    {
        if(obj)
        {
            obj.SetText(newText);
        }
    }
}
