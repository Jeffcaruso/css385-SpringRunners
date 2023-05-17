using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuSelections : MonoBehaviour
{
    public static bool isPaused = false;
    public static bool dead = false;

    public GameObject pauseMenuUI;
    public GameObject deathMenuUI;

    void Update()
    {
        if (!dead){
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (isPaused)
                {
                    Resume();
                }
                else
                {
                    Pause();
                }
            }
        } else {
            deathMenuUI.SetActive(true);
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
        GameObject.Find("Piston").GetComponent<PistonMovement>().enabled = true;
    }

    private void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
        GameObject.Find("Piston").GetComponent<PistonMovement>().enabled = false;
    }

    public void LevelSelect(){
        Time.timeScale = 1f;
        isPaused = false;
        dead = false;
        SceneManager.LoadScene("Level Select Scene");
    }

    public void Quit(){
        //Add quit screen?
        Debug.Log("Quitting!");
        Application.Quit();
    }

    public void TryAgain(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        dead = false;
        deathMenuUI.SetActive(false);
    }
}