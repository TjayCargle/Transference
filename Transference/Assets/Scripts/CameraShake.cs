using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour {
    public Vector3 origin; 
  public Quaternion rotation; 
    private void Start()
    {
         origin = transform.position;
         rotation = transform.localRotation;
    }
    public IEnumerator Shake(float duration, float magnatude, float delay)
    {

        float elapsed = 0.0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            if(elapsed < delay)
            {
                continue;
            }
            float xpos = Random.Range(-0.2f,0.2f) *magnatude;
            float ypos = Random.Range(-0.2f, 0.2f) * magnatude;
            float xro = Random.Range(-0.2f, 0.2f) * magnatude;
            float yro = Random.Range(-0.2f, 0.2f) * magnatude;
            float zro = Random.Range(-0.2f, 0.2f) * magnatude;
            Vector3 newpos = origin + new Vector3(xpos, ypos);
            Quaternion quat = Quaternion.Euler(xro, yro, zro);
            transform.position = newpos;
            transform.localRotation = quat;
            yield return null;
        }
        transform.position = origin;
        transform.localRotation = rotation;
    }
    public void ToOrigin()
    {
        transform.position = origin;
        transform.localRotation = rotation;
    }
}
