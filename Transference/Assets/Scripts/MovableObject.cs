using System;
using System.Collections.Generic;
using UnityEngine;

public class MovableObject : MonoBehaviour
{
    public float enterLocation = 1.2f;
    public float exitLocation = -1.2f;

    public void MoveEnter()
    {
        LeanTween.moveLocalX(gameObject, enterLocation, 0.5f);
    }

    public void MoveEnter(Func<object> onComplete)
    {
        LeanTween.moveLocalX(gameObject, enterLocation, 0.5f).setOnComplete(() =>
        {
            onComplete();
        });
    }

    public void MoveExit()
    {
        LeanTween.moveLocalX(gameObject, exitLocation, 0.5f);
    }

    public void MoveExit(Func<object> onComplete)
    {
        LeanTween.moveLocalX(gameObject, exitLocation, 0.5f).setOnComplete(() =>
        {
            onComplete();
        });
    }

}
