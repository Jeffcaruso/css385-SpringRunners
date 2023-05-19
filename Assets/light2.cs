using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class light2 : MonoBehaviour
{
    GameObject circle;
    // Start is called before the first frame update
    void Start()
    {
        //make new circle gameobject
        
        // circle = new GameObject("circle");
        // circle.AddComponent<SpriteRenderer>();
        // SpriteRenderer sr = circle.GetComponent<SpriteRenderer>();
        // sr.sprite = Resources.Load<Sprite>("Circle");


        circle = new GameObject("circle");
        circle.AddComponent<SpriteRenderer>();
        SpriteRenderer sr = circle.GetComponent<SpriteRenderer>();
        CircleCollider2D cr = GetComponent<CircleCollider2D>();
        sr.sprite = Resources.Load<Sprite>("Circle");
        //set position and radius
        circle.transform.position = this.transform.position;
        circle.transform.localScale = new Vector3(19 * cr.radius, 19 * cr.radius, 1);
        circle.transform.SetParent(this.transform);
        
        Color tmp = GetComponent<SpriteRenderer>().color;
        tmp.r = 105;
        tmp.b = 105;
        tmp.g = 105;
        circle.GetComponent<SpriteRenderer>().color = tmp;

        
    }

    // Update is called once per frame
    void Update()
    {
        Color tmp = GetComponent<SpriteRenderer>().color;
        tmp.a = 0.1f;
        circle.GetComponent<SpriteRenderer>().color = tmp;
    }
}
