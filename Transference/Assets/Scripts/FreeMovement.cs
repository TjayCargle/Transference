using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeMovement : MonoBehaviour
{
    int dir = -1;
    float timer = 0.3f;
    public float startTime = 0.5f;
    public bool running = false;
    public bool upDown = true;
    public Vector3 truePosition;
    Vector3 up = new Vector3(0.0f, 0.5f);
    Vector3 down = new Vector3(0.0f, -0.5f);
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (running == true)
        {

            if (timer <= 0)
            {
                dir = dir * -1;
                timer = startTime;
            }
            else
            {
                if (upDown == true)
                {

                    if (dir > 0)
                    {
                        transform.Translate(up, Space.Self);
                    }
                    else
                    {
                        transform.Translate(down, Space.Self);

                    }
                }
                else
                {
                    if (dir > 0)
                    {
                        transform.Translate(Vector3.left, Space.Self);
                    }
                    else
                    {
                        transform.Translate(Vector3.right, Space.Self);

                    }
                }

                timer -= Time.deltaTime;
            }
        }
    }
}
