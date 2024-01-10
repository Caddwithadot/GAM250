using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class RayLightStates : MonoBehaviour
{
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private MouseControls mouseControls;
    private RayLightNEW rayLight;
    public SuperRayLight superRayLight;

    public float startAngle = 45f;
    public float startDist = 4f;
    public float endAngle = 8f;
    public float endDist = 12f;

    private float currentAngle;
    private float currentDist;

    private float currentSuperAngle = 0f;

    private AudioSource audioSource;
    public AudioSource otherSource;
    public AudioSource superAudioSource;
    public AudioClip focusSound;
    public AudioClip revertSound;
    public AudioClip flicker;
    public AudioClip overheat;

    private bool isFocused = false;
    private bool isKilling = true;

    private bool finishedFocusing = false;
    private bool finishedUnfocusing = false;
    private bool finishedOverheating = false;

    private float focusCooldownTime = 0f;
    public float focusCooldown = 0.3f;
    private float unfocusCooldownTime = 0f;
    public float unfocusCooldown = 0.3f;

    public float focusSpeed = 300f;
    public float unfocusSpeed = 150f;
    public float overheatSpeed = 2f;
    public float revertSpeed = 18f;

    public Animator flickerAni;
    public MeshRenderer superMesh;

    private PlayerMovementNEW moveScript;
    private float defaultSpeed;

    public Material lightMaterial;

    private float returnTime = 0;
    public float returnCooldown = 1f;
    private Animator returnLight;

    private void Start()
    {
        rayLight = GetComponent<RayLightNEW>();
        audioSource = GetComponent<AudioSource>();

        currentAngle = startAngle;
        currentDist = startDist;

        superRayLight.SetFOV(currentSuperAngle);
        superRayLight.SetViewDistance(endDist);

        moveScript = FindObjectOfType<PlayerMovementNEW>();
        defaultSpeed = moveScript.moveSpeed;

        returnLight = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!mouseControls.focus && unfocusCooldownTime <= 0 || !finishedUnfocusing)
        {
            UnfocusLight();
        }
        else if (mouseControls.focus && focusCooldownTime <= 0 || !finishedFocusing)
        {
            FocusLight();
        }

        if (mouseControls.kill && unfocusCooldownTime <= 0 && !finishedOverheating)
        {
            OverheatLight();
        }
        else if (!mouseControls.kill || focusCooldownTime <= 0 || finishedOverheating)
        {
            RevertLight();
        }

        if (!isFocused && unfocusCooldownTime > 0)
        {
            unfocusCooldownTime -= Time.deltaTime;
        }

        if (isFocused && focusCooldownTime > 0)
        {
            focusCooldownTime -= Time.deltaTime;
        }

        if(returnTime > 0)
        {
            returnTime -= Time.deltaTime;

            if(returnTime <= 0)
            {
                GetComponent<PolygonCollider2D>().enabled = true;
                finishedOverheating = false;
            }
        }
    }

    public void UnfocusLight()
    {
        if (!isFocused)
        {
            focusCooldownTime = focusCooldown;

            currentSuperAngle = 0;
            superRayLight.SetFOV(currentSuperAngle);
            finishedOverheating = false;
            isKilling = true;

            //audioSource.PlayOneShot(unfocusSound);
            finishedUnfocusing = false;
            isFocused = true;
        }

        superAudioSource.enabled = false;
        otherSource.enabled = false;
        #region Unfocus lerp
        float targetAngle = startAngle;
        float targetDist = startDist;

        float journeyLengthAngle = Mathf.Abs(targetAngle - currentAngle);
        float journeyLengthDist = Mathf.Abs(targetDist - currentDist);

        float stepAngle = unfocusSpeed / journeyLengthAngle * Time.deltaTime;
        float stepDist = unfocusSpeed / journeyLengthDist * Time.deltaTime;

        currentAngle = Mathf.Lerp(currentAngle, targetAngle, stepAngle);
        rayLight.SetFOV(currentAngle);

        currentDist = Mathf.Lerp(currentDist, targetDist, stepDist);
        rayLight.SetViewDistance(currentDist);
        #endregion

        //fully unfocused
        if (currentDist == startDist && currentAngle == startAngle)
        {
            finishedUnfocusing = true;
            //lightMaterial.color = startColor;
        }
    }

    public void FocusLight()
    {
        if (isFocused)
        {
            unfocusCooldownTime = unfocusCooldown;

            audioSource.PlayOneShot(focusSound, 0.75f);
            finishedFocusing = false;
            isFocused = false;
        }

        #region Focus lerp
        float targetAngle = endAngle;
        float targetDist = endDist;

        float journeyLengthAngle = Mathf.Abs(targetAngle - currentAngle);
        float journeyLengthDist = Mathf.Abs(targetDist - currentDist);

        float stepAngle = focusSpeed / journeyLengthAngle * Time.deltaTime;
        float stepDist = focusSpeed / journeyLengthDist * Time.deltaTime;

        currentAngle = Mathf.Lerp(currentAngle, targetAngle, stepAngle);
        rayLight.SetFOV(currentAngle);

        currentDist = Mathf.Lerp(currentDist, targetDist, stepDist);
        rayLight.SetViewDistance(currentDist);
        #endregion

        //fully focused
        if (currentDist == endDist && currentAngle == endAngle)
        {
            finishedFocusing = true;
        }
    }

    public void OverheatLight()
    {
        if (isKilling)
        {
            finishedOverheating = false;
            isKilling = false;
        }

        //
        moveScript.moveSpeed = defaultSpeed / 3;

        superAudioSource.enabled = true;
        #region Overheat lerp
        float targetAngle = endAngle;
        float journeyLengthAngle = Mathf.Abs(targetAngle - currentSuperAngle);

        float stepAngle = overheatSpeed / journeyLengthAngle * Time.deltaTime;

        currentSuperAngle = Mathf.Lerp(currentSuperAngle, targetAngle, stepAngle);
        superRayLight.SetFOV(currentSuperAngle);
        #endregion

        //handle the flickering
        if (currentSuperAngle >= (endAngle / 4) && currentSuperAngle < (endAngle / 3))
        {
            flickerAni.enabled = true;
            otherSource.enabled = true;
        }
        else if (currentSuperAngle >= (endAngle / 2) && currentSuperAngle < (endAngle / 3) * 2)
        {
            flickerAni.enabled = true;
            otherSource.enabled = true;
        }
        else
        {
            flickerAni.enabled = false;
            superMesh.enabled = true;
            otherSource.enabled = false;
        }

        //fully overheat
        if (currentSuperAngle == endAngle)
        {
            if(playerHealth.health > 1)
            {
                playerHealth.TakeDamage(1, null);
            }
            else
            {
                playerHealth.TakeDamage(0, null);
            }

            currentSuperAngle = 0;
            superRayLight.SetFOV(currentSuperAngle);

            audioSource.PlayOneShot(overheat, 2f);

            GetComponent<OverheatParticles>().EmitParticlesFromPolygon();
            GetComponent<PolygonCollider2D>().enabled = false;
            returnLight.SetTrigger("Off");
            returnTime = returnCooldown;

            finishedOverheating = true;
        }
    }

    public void RevertLight()
    {
        if (!isKilling)
        {
            isKilling = true;
            audioSource.PlayOneShot(revertSound, 0.25f);
        }

        //
        moveScript.moveSpeed = defaultSpeed;

        superAudioSource.enabled = false;
        otherSource.enabled = false;
        #region Revert lerp
        float targetAngle = 0;
        float journeyLengthAngle = Mathf.Abs(targetAngle - currentSuperAngle);

        float stepAngle = revertSpeed / journeyLengthAngle * Time.deltaTime;

        currentSuperAngle = Mathf.Lerp(currentSuperAngle, targetAngle, stepAngle);
        superRayLight.SetFOV(currentSuperAngle);
        #endregion 

        flickerAni.enabled = false;
        superMesh.enabled = true;
    }
}