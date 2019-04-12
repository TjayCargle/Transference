
using UnityEngine;
using UnityEngine.UI;

public class IconImage : MonoBehaviour
{
    private Image myImage;

    public Image ICON
    {
        get { return myImage; }
        set { myImage = value; }
    }
    private void Start()
    {
        myImage = GetComponent<Image>();
    }
    public void LoadImage()
    {
        myImage = GetComponent<Image>();
    }
}
