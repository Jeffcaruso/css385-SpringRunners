using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        if (SceneManager.GetActiveScene().name == "Boss Level"){//Ensure that the string matches the name for the boss level scene
            this.transform.DetachChildren();
        }
        rb.velocity = new Vector2(2000.0f, 500.0f);
    }
}
