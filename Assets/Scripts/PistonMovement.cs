using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PistonMovement : MonoBehaviour
{
    private GameObject finishLine = null;
    private GameObject wall = null;
    private float finishX;
    //Camera edge
    private Camera mainCamera;
    private float leftEdge;


    public float pistonSpeed = 10f; //Change this variable to speed or slow piston
    // Start is called before the first frame update
    void Start()
    {
        wall = GameObject.Find("Piston");
        finishLine = GameObject.FindGameObjectWithTag("Finish");
        finishX = finishLine.transform.position.x;

        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        CalculateLeftEdge();
        if (wall.transform.position.x < finishX)
        {
            if (wall.transform.position.x > leftEdge - .001)
            {
                wall.transform.position = new Vector3(wall.transform.position.x + (pistonSpeed / 500), wall.transform.position.y, 0);

            }
            else
            {
                wall.transform.position = new Vector3(leftEdge, wall.transform.position.y, 0);
            }
        }

    }

    private void CalculateLeftEdge()
    {
        float distanceFromCamera = Mathf.Abs(transform.position.z - mainCamera.transform.position.z);
        Vector3 leftViewportPoint = new Vector3(0f, 0.5f, distanceFromCamera);
        Vector3 leftWorldPoint = mainCamera.ViewportToWorldPoint(leftViewportPoint);
        leftEdge = leftWorldPoint.x;
    }
}
