//------------------------------------------------------------------------------
//
// File Name:    
// Author(s):    
// Project:        
// Course:        
//
// Copyright © 2023 DigiPen (USA) Corporation.
//
//------------------------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public GameObject PauseMenuPrefab;
    private GameObject PauseMenu;
    private float RecordedTimeScale;

    //Store more variables, find a way to record the light

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (PauseMenu == null)
            {
                Pause();
            }
            else
            {
                Unpause();
            }
        }
    }

    public void Pause()
    {
        PauseMenu = Instantiate(PauseMenuPrefab);
        RecordedTimeScale = Time.timeScale;
        Time.timeScale = 0f;
    }

    public void Unpause()
    {
        Destroy(PauseMenu);
        Time.timeScale = RecordedTimeScale;
    }
}
