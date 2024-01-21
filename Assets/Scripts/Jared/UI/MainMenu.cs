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
    private float AnimationTimer = 0f;
    private float AnimationDuration = 2f;

    public Animation StartAnimation;

    public ParticleSystem IdleParticleSystem;

    public Animator CameraAnimator;
    public Animator KnightSpriteAnimator;

    public GameObject KnightSprite;

    public GameObject ButtonsPanel;

    public GameObject QuitConfirmationPrompt;

    private void Start()
    {
        if (StartAnimation != null)
            AnimationDuration = StartAnimation.clip.length;
    }

    private void Update()
    {
        if (KeyPressed)
        {
            AnimationTimer += Time.deltaTime;

            AnyKeyPressed();

            if (AnimationTimer >= AnimationDuration)
            {
                ButtonFadeIn();

                //Add particle explosion anim
                IdleParticleSystemEffects();

            }


        }
        else if (Input.anyKeyDown)
        {
            KeyPressed = true;
        }
    }

    public void AnyKeyPressed()
    {
        IdleParticleSystem.Stop();

        KnightSprite.GetComponent<Animator>().SetTrigger("KnightSpriteAwake");
    }

    public void ButtonFadeIn()
    {
        ButtonsPanel.SetActive(true);

        for (int i = 0; i < ButtonsPanel.transform.childCount; i++)
        {

            if (ButtonsPanel.transform.GetChild(i).GetComponent<Animator>() != null)
            {
                ButtonsPanel.transform.GetChild(i).GetComponent<Animator>().SetTrigger("ButtonFadeIn");
            }
        }
    }

    public void ButtonFadeOut()
    {
        for (int i = 0; i < ButtonsPanel.transform.childCount; i++)
        {
            if (ButtonsPanel.transform.GetChild(i).GetComponent<Button>() != null)
            {
                ButtonsPanel.transform.GetChild(i).GetComponent<Button>().interactable = false;
            }

            if (ButtonsPanel.transform.GetChild(i).GetComponent<Animator>() != null)
            {
                ButtonsPanel.transform.GetChild(i).GetComponent<Animator>().SetTrigger("ButtonFadeOut");
            }
        }
    }

    public void IdleParticleSystemEffects()
    {
        ParticleSystem.MainModule main = IdleParticleSystem.main;
        main.startColor = Color.white;
        main.startSpeed = 3.5f;
        main.startSize = 0.5f;
    }

    public void StartButton()
    {
        CameraAnimator.SetTrigger("CameraToSaveFiles");
        KnightSpriteAnimator.SetTrigger("KnightSpriteDrop");

        ButtonFadeOut();
    }
    
    public void SettingsButton()
    {

        ButtonFadeOut();
    }

    public void CreditsButton()
    {


        ButtonFadeOut();
    }

    public void QuitButton()
    {
        if (QuitConfirmationPrompt != null)
        {
            Instantiate(QuitConfirmationPrompt);
        }
        else
        {
            Application.Quit();
        }
    }
}
