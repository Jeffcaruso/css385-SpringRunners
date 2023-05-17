using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class light : MonoBehaviour
{
    public SpriteRenderer sprender;
    public GameObject grappleBlock = null;
    public GameObject thePlayer = null;
    public bool inRange = false;
   
    // Update is called once per frame
    void Start()
    {   
        if (thePlayer.GetComponent<Grappler>().withinRange){
            Color tmp = GetComponent<SpriteRenderer>().color;
            tmp.r = 0;
            tmp.g = 150;
            tmp.b = 0;
            GetComponent<SpriteRenderer>().color = tmp;
        }else{
            Color tmp = GetComponent<SpriteRenderer>().color;
            tmp.r = 150;
            tmp.g = 0;
            tmp.b = 0;
            GetComponent<SpriteRenderer>().color = tmp;
        }
    }
}
