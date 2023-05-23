using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuSelections : MonoBehaviour
{
    public static bool isPaused = false;
    public static bool win = false;
    public static bool checkpoint = false;
    public static bool dead = false;

    public GameObject pauseMenuUI;
    public GameObject deathMenuUI;
    public GameObject checkpointDeathMenuUI;
    public GameObject victoryScreen;
    public GameObject quitScreen;

    private GameObject player;
    private GameObject bossTrigger;

    void Start()
    {
        player = GameObject.Find("Hero square");
        bossTrigger = GameObject.Find("BossTrigger");
    }

    void Update()
    {
        if (SceneManager.GetActiveScene().name == "Boss Level")
        {
            if (win)
            {
                player.GetComponent<HeroBehavior>().enabled = false;
                victoryScreen.SetActive(true);
            }
            else if (!dead)
            {
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
            }
            else if (checkpoint)
            {
                checkpointDeathMenuUI.SetActive(true);
            }
            else
            {
                deathMenuUI.SetActive(true);
            }
        }
        else
        {
            if (win)
            {
                player.GetComponent<HeroBehavior>().enabled = false;
                victoryScreen.SetActive(true);
            }
            else if (!dead)
            {
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
            }
            else
            {
                deathMenuUI.SetActive(true);
            }
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

    public void LevelSelect()
    {
        Time.timeScale = 1f;
        isPaused = false;
        dead = false;
        win = false;
        checkpoint = false;
        SceneManager.LoadScene("Level Select Scene");
    }

    public void Quit()
    {
        quitScreen.SetActive(true);
        Debug.Log("Quitting!");
        Application.Quit();
    }

    public void TryAgain()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        win = false;
        dead = false;
        deathMenuUI.SetActive(false);
        victoryScreen.SetActive(false);
    }

    public void TryAgainFromCheckpoint()
    {
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        //player.transform.position = new Vector3(bossTrigger.transform.position.x, bossTrigger.transform.position.y,player.transform.position.z);
        win = false;
        dead = false;
        checkpointDeathMenuUI.SetActive(false);
        victoryScreen.SetActive(false);
        SceneManager.sceneLoaded += OnSceneLoaded;
        StartCoroutine(LoadSceneAsync());
    }

    private IEnumerator LoadSceneAsync()
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync("Boss Level");
        asyncOperation.allowSceneActivation = false;

        while (!asyncOperation.isDone)
        {
            if (asyncOperation.progress >= 0.9f)
            {
                asyncOperation.allowSceneActivation = true;
            }

            yield return null;
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Additional code to be executed after the scene has fully loaded
        Debug.Log("Scene loaded!");

        // Your code to move the game object
        player = GameObject.Find("Hero square");
        bossTrigger = GameObject.Find("BossTrigger");
        player.transform.position = new Vector3(bossTrigger.transform.position.x, bossTrigger.transform.position.y, player.transform.position.z);
        GameObject.Find("Piston").GetComponent<PistonMovement>().pistonSpeed = 100f;
        // Unsubscribe from the event
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}