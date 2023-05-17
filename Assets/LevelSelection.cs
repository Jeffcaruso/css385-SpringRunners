using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class LevelSelection : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //nothing to do
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("1") == true)
        {
            //tutorial (level 1)
            SceneManager.LoadScene("Tutorial-Scene");
        }
        else if (Input.GetKeyDown("2"))
        {
            //level 2 (Jeff)
            SceneManager.LoadScene("Jeffrey-Scene");
        }
        else if (Input.GetKeyDown("3"))
        {
            //level 3 (lawrence)
            SceneManager.LoadScene("Lawrence-Scene");
        }
        else if (Input.GetKeyDown("4"))
        {
            //level 4 (Malaki)
            SceneManager.LoadScene("Malachi-Scene");
        }
        else if (Input.GetKeyDown("5"))
        {
            //level 5 (Joseph)
            SceneManager.LoadScene("Joseph-Scene");
        }
        else
        {
            //do nothing
        }
    }
}
