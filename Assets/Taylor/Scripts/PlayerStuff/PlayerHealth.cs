using Mono.Cecil.Cil;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    private Animator animator;
    private SpriteRenderer sr;

    public RayAura rayAura;
    public Material auraMaterial;

    public int health = 4;
    private int maxHealth;

    private float invTimer = 0f;
    public float invFrameCooldown = 2f;

    private bool isHealing = false;
    private float healTimer = 0f;
    public float passHealDelay = 5f;
    public float lampHealDelay = 0.5f;

    public float maxAuraScale = 6.0f;

    private AudioSource audioSource;
    public AudioClip hitSound;
    public AudioClip healSound;

    public SceneHandler sceneHandler;

    private PlayerMovementNEW moveScript;

    private Rigidbody2D rb;

    private float knockBackTimer = 0f;
    public float knockTime = 0.5f;

    public ParticleSystem auraParticles;
    public ParticleSystem edgeParticles;
    public Animator auraAnim;
    public MeshRenderer auraMesh;

    private void Start()
    {
        maxHealth = health;
        animator = transform.parent.GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        moveScript = GetComponent<PlayerMovementNEW>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if(invTimer > 0)
        {
            invTimer -= Time.deltaTime;

            animator.enabled = true;
            auraAnim.enabled = true;
        }
        else
        {
            animator.enabled = false;
            auraAnim.enabled = false;
            sr.enabled = true;
            auraMesh.enabled = true;
        }

        // lamp healing
        if (isHealing && health < maxHealth)
        {
            healTimer += Time.deltaTime;

            if (healTimer >= lampHealDelay)
            {
                audioSource.PlayOneShot(healSound, 1f);
                health++;
                healTimer = 0f;
            }
        }

        float auraDifference = maxAuraScale / maxHealth;
        float auraScale = health * auraDifference;

        if (health != 1)
        {
            sr.color = new Color((auraScale - 0.15f) / maxAuraScale, (auraScale - 0.15f) / maxAuraScale, (auraScale - 0.15f) / maxAuraScale);
            auraMaterial.color = new Color(1, 1, 0.5f, health * 0.098f);
            rayAura.viewDistance = (auraScale + 1.5f) / 3;
            auraParticles.emissionRate = health * 35;
            edgeParticles.startSize = health * 0.025f;
        }
        else
        {
            sr.color = new Color(0.3f, 0.3f, 0.3f);
            auraMaterial.color = new Color(1, 1, 0.5f, 0f);
            rayAura.viewDistance = 1;
            auraParticles.emissionRate = health * 35;
            edgeParticles.startSize = health * 0.025f;
        }

        //knockback
        if(knockBackTimer > 0)
        {
            knockBackTimer -= Time.deltaTime;

            if(knockBackTimer <= 0)
            {
                moveScript.enabled = true;
            }
        }
    }

    public void TakeDamage(int lostHealth, Transform hazard)
    {
        invTimer = invFrameCooldown;
        health -= lostHealth;
        moveScript.enabled = false;
        knockBackTimer = knockTime;

        if (hazard != null)
        {
            rb.velocity = Vector3.zero;
            if (transform.position.x > hazard.position.x)
            {
                rb.AddForce(new Vector2(3, 7.5f), ForceMode2D.Impulse);
            }
            else
            {
                rb.AddForce(new Vector2(-3, 7.5f), ForceMode2D.Impulse);
            }
        }
        else
        {
            rb.velocity = Vector3.zero;
            if (transform.localScale.x < 0)
            {
                rb.AddForce(new Vector2(2.5f, 5), ForceMode2D.Impulse);
            }
            else
            {
                rb.AddForce(new Vector2(-2.5f, 5), ForceMode2D.Impulse);
            }
        }

        if (health <= 0)
        {
            sceneHandler.PlayerDeathReload();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Aura"))
        {
            isHealing = true;

            if(health != maxHealth)
            {
                collision.transform.parent.GetComponent<HealLamp>().HealParticlesPlay();
            }
            else
            {
                collision.transform.parent.GetComponent<HealLamp>().HealParticlesStop();
            }
        }

        if (collision.gameObject.tag == ("Enemy") && invTimer <= 0)
        {
            audioSource.PlayOneShot(hitSound, 3f);
            TakeDamage(1, collision.gameObject.transform);
            healTimer = 0;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == ("Enemy") && invTimer <= 0)
        {
            audioSource.PlayOneShot(hitSound, 3f);
            TakeDamage(1, collision.gameObject.transform);
            healTimer = 0;
        }

        if (collision.CompareTag("Aura") && health == maxHealth)
        {
            collision.transform.parent.GetComponent<HealLamp>().HealParticlesStop();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Aura"))
        {
            collision.transform.parent.GetComponent<HealLamp>().HealParticlesStop();
            isHealing = false;
            healTimer = 0f;
        }
    }
}