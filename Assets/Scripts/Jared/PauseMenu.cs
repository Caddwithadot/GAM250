using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    private PauseManager pauseManager;
    private GameObject Player;
    private Camera Cam;
    private List<GameObject> Submenus = new List<GameObject>();

    public RectTransform PlayerMask;
    public GameObject ConfirmationPrompt;

    public int MainMenuBuildIndex = 0;


    void Start()
    {
        pauseManager = GameObject.FindObjectOfType<PauseManager>();

        Player = GameObject.FindWithTag("Player");
        Cam = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();

        MoveMaskToPlayer();
    }

    public void Resume()
    {
        pauseManager.Unpause();
    }

    public void Restart()
    {
        pauseManager.Unpause();

        //Confirmation prompt
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Settings()
    {
        //Instantiate settings menu
    }

    public void MainMenu()
    {
        pauseManager.Unpause();

        //Confirmation prompt
        SceneManager.LoadScene(MainMenuBuildIndex);
    }

    public void Quit()
    {
        //Confirmation prompt
        Application.Quit();
    }

    public void OnDestroy()
    {
        for (int i = 0; i < Submenus.Count; i++)
        {
            Destroy(Submenus[i]);
        }
    }

    public void MoveMaskToPlayer()
    {
        Transform Target = Player.transform;
        Vector3 ScreenPos = Cam.WorldToScreenPoint(Target.position);
        PlayerMask.position = ScreenPos;
    }
}
