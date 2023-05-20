using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public float delay = 1.0f; //Adds a specified amount of time before the shake occurs

    public void CallShake()
    {
        StartCoroutine(Shake(0.4f,3.0f));
    }

    public IEnumerator Shake (float duration, float magnitude)
    {
        Vector3 originalPos = transform.position;
        
        float elapsed = 0.0f;
        while (elapsed < delay){
            elapsed += Time.deltaTime;

            yield return null;
        }


        while (elapsed < duration + delay)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            transform.position = new Vector3(originalPos.x + x, originalPos.y + y, originalPos.z);

            elapsed += Time.deltaTime;

            yield return null;
        }

        transform.position = originalPos;
        MenuSelections.dead = true;
        //GameObject.Find("MainCharacter").GetComponent<MC>().DeathMessage();
    }
}