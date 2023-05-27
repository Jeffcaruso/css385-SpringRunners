using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{

    public float speed = 10f;
    public float grappleDist = 3f;
    public float springShot = 10f;
    public bool isGrappling = false;
    public bool canSpringshot = true;
    public GameObject player;
    Rigidbody playerRigidbody;

    void Start()
    {
        playerRigidbody = player.GetComponent<Rigidbody>();
    }

    void Update()
    {
        Vector3 pos = transform.position;
        isGrappling = GetComponent<Grappler>().localGrapple;

        if(isGrappling){
            if(canSpringshot){
                //add force here (happens on grapple start)

                ////////////////////////////////////////////////////
                
                
                //playerRigidbody.AddForce(Vector3.up, ForceMode.VelocityChange);
                //playerRigidbody.AddForce(Vector3.up * springShot * speed);

                //float grappleAngle = new Vector3.Angle(grappleBlock.transform.position, transform.position);

                //float xcomponent = Mathf.Cos(grappleAngle * Mathf.PI / 180) * springShot;
                //float ycomponent = Mathf.Sin(grappleAngle * Mathf.PI / 180) * springShot;
                //Vector3 forceApplied = new Vector3 AddForce(ycomponent, 0, xcomponent);

                ////////////////////////////////////////////////////
                speed = 0f;

                ////////////////////////////////////////////////////
                Debug.Log("grapple!");
            }
            canSpringshot = false;

        }else{
            canSpringshot = true;

            // "w" can be replaced with any key
            // this section moves the character up
            if (Input.GetKey("w"))
            {
                pos.y += speed * Time.deltaTime;
            }

            // "d" can be replaced with any key
            // this section moves the character right
            if (Input.GetKey("d"))
            {
                pos.x += speed * Time.deltaTime;
            }

            // "a" can be replaced with any key
            // this section moves the character left
            if (Input.GetKey("a"))
            {
                pos.x -= speed * Time.deltaTime;
            }

            if (Input.GetKey("r"))
            {
                pos.x = -5;
                pos.y = -1;
            }

            transform.position = pos;
        }
            speed = 10f;
    }
}
