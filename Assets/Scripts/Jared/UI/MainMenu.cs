//------------------------------------------------------------------------------
//
// File Name:    MainMenu.cs
// Author(s):    Jared
// Project:      KnightLight
// Course:       GAM250
//
// Copyright © 2023 DigiPen (USA) Corporation.
//
//------------------------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public bool KeyPressed = false;
    public float AnimationTimer = 0f;
    public float AnimationDuration = 0f;

    public GameObject ButtonsPanel;

    private void Update()
    {
        if (KeyPressed)
        {
            AnimationTimer += Time.deltaTime;

            if (AnimationTimer >= AnimationDuration)
            {
                ButtonFadeInAnimation();
            }


        }
        else if (Input.anyKeyDown)
        {
            KeyPressed = true;
            AnyKeyPressed();
        }

        if(Input.GetKeyDown(KeyCode.W))
        {
            
        }
    }

    public void AnyKeyPressed()
    {

        //Play animation

        //Timer

        //Unlock button panel
        ButtonsPanel.SetActive(true);

        for (int i = 0; i < ButtonsPanel.transform.childCount; i++)
        {
            ButtonsPanel.transform.GetChild(i).GetComponent<Animator>().SetTrigger("ButtonFadeIn");
        }
    }

    public void ButtonFadeInAnimation()
    {
        for (int i = 0; i < ButtonsPanel.transform.childCount; i++)
        {
            if (ButtonsPanel.transform.GetChild(i).GetComponent<Animator>() != null)
            {
                ButtonsPanel.transform.GetChild(i).GetComponent<Animator>().SetTrigger("ButtonFadeIn");
            }
        }
    }

    public void Play()
    {

    }
}
