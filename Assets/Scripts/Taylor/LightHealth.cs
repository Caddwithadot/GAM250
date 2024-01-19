using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LightHealth : MonoBehaviour
{
    private SwappingAndRecall swapping;
    private PlayerMovementNEW playerMovement;
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private ParticleSystem ps;

    public float startDelay = 0.5f;
    private float startTimer = 0;
    public float deathDelay = 1f;
    private float deathTimer = 0f;

    private void Start()
    {
        swapping = FindObjectOfType<SwappingAndRecall>();
        playerMovement = GetComponent<PlayerMovementNEW>();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        ps = GetComponent<ParticleSystem>();

        startTimer = startDelay;
    }

    // Update is called once per frame
    void Update()
    {
        if(startTimer > 0)
        {
            startTimer -= Time.deltaTime;

            if(startTimer <= 0)
            {
                sr.sortingOrder++;
            }
        }

        if(deathTimer > 0f)
        {
            deathTimer -= Time.deltaTime;

            if(deathTimer <= 0f)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Hazard"))
        {
            //disallow character swapping
            swapping.enabled = false;

            //remove movement and reset velocity
            playerMovement.enabled = false;
            rb.velocity = Vector3.zero;

            //play death particles and hide player
            ps.Play();
            sr.enabled = false;

            //start the death timer
            deathTimer = deathDelay;
        }
    }
}
