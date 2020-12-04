using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TweenedObject : MonoBehaviour
{
    public void ZoomOut()
    {

    }
    private void OnMouseEnter()
    {
        LeanTween.scale(gameObject, Vector3.one, 0.5f);
        Debug.Log("Enter");
    }
    private void OnMouseExit()
    {
        LeanTween.scale(gameObject, new Vector3(0.5f, 0.5f, 0.5f), 0.5f);

        Debug.Log("Exit");

    }
}
