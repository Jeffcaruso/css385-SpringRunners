using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathSequence : MonoBehaviour
{

    BoxCollider2D bc;
    Rigidbody2D rb;
    GameObject piston;
    // Start is called before the first frame update
    void Start()
    {
        bc = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        piston = GameObject.Find("Piston");
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.CompareTag("Piston") || transform.position.x < piston.transform.position.x)
        {
            Debug.Log("TESTING Collided with piston, die now");
            DeathAnimation();
        }
    }

    private void DeathAnimation(){
        /*Turns off Grappling Code for animation to work while grappling*/
        GetComponent<Grappler>()._distanceJoint.enabled = false;
        GetComponent<Grappler>()._lineRenderer.enabled = false;
        GetComponent<Grappler>().localGrapple = false;
        /*--------------------------------------------------------------*/
        GetComponent<HeroBehavior>().enabled = false;
        GameObject.Find("Main Camera").GetComponent<CameraFollow>().following = false;
        bc.enabled = false;
        GameObject.Find("Piston").GetComponent<PistonMovement>().enabled = false;
        GameObject.Find("Main Camera").GetComponent<CameraShake>().CallShake();
        rb.velocity = new Vector2(1500.0f, 500.0f);
    }
}
