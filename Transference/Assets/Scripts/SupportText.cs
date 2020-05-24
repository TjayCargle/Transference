using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SupportText : MonoBehaviour
{

    [SerializeField]
    TextMeshProUGUI main;
    [SerializeField]
    TextMeshProUGUI shadow;
    [SerializeField]
    TextMeshProUGUI dark;
    public bool isSetup = false;
    [SerializeField]
   public FreeMovement freemove;

    public bool isVisible = false;
    public void Setup()
    {
        if (!isSetup)
        {
            TextMeshProUGUI[] meshes = GetComponentsInChildren<TextMeshProUGUI>();
            if (meshes.Length == 3)
            {
                main = meshes[2];
                shadow = meshes[1];
                dark = meshes[0];
                isSetup = true;
            }
            else
            {
                Debug.Log("uh oh");
            }
            if (GetComponent<FreeMovement>())
            {
                freemove = GetComponent<FreeMovement>();
            }
        }
    }

    private void Start()
    {
        Setup();
    }

    public void SetText(string newtext, Color someColor)
    {
        if (isSetup)
        {

            main.text = newtext;
            shadow.text = newtext;
            dark.text = newtext;
            main.color = someColor;
        }
    }

    public void SetTransparent()
    {
        if (isSetup)
        {
            main.color = new Color(main.color.r, main.color.g, main.color.b, 0.0f);
            shadow.color = new Color(shadow.color.r, shadow.color.g, shadow.color.b, 0.0f);
            dark.color = new Color(dark.color.r, dark.color.g, dark.color.b, 0.0f);
            isVisible = false;
        }
    }

    public void SetVisible()
    {
        if (isSetup)
        {

            main.color = new Color(main.color.r, main.color.g, main.color.b, 1.0f);
            shadow.color = new Color(shadow.color.r, shadow.color.g, shadow.color.b, 1.0f);
            dark.color = new Color(dark.color.r, dark.color.g, dark.color.b, 1.0f);
            isVisible = true;
        }
    }
}
