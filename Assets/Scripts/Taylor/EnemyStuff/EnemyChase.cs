using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChase : MonoBehaviour
{
    private Rigidbody2D rb;
    private Transform player;
    private EnemyPatrolWithChase enemyPatrol;
    private EnemyFlip enemyFlip;

    public float jumpForce = 9f;
    public float jumpSpeedBoost = 10f;

    private float jumpTimer = 0f;
    public float jumpCooldown = 1f;

    public float playerOffset = -1f;

    public EnemyDetection groundDetector;
    public EnemyDetection voidDetector;
    public EnemyDetection wallformDetector;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        enemyPatrol = GetComponent<EnemyPatrolWithChase>();
        enemyFlip = GetComponent<EnemyFlip>();
    }

    // Update is called once per frame
    void Update()
    {
        if(jumpTimer <= 0)
        {
            if (enemyPatrol.isFollowingTarget && groundDetector.detected)
            {
                if (player.position.y + playerOffset > transform.position.y && wallformDetector.detected)
                {
                    Jump();
                }
            }

            if(enemyPatrol.isReturningToPatrol && groundDetector.detected)
            {
                if (wallformDetector.detected)
                {
                    Jump();
                }
            }

            if (voidDetector.detected == false && groundDetector.detected)
            {
                Jump();
            }
        }
        else
        {
            jumpTimer -= Time.deltaTime;
        }
    }

    public void Jump()
    {
        rb.AddForce(new Vector2(rb.velocity.x + (jumpSpeedBoost * enemyFlip.direction), jumpForce), ForceMode2D.Impulse);
        jumpTimer = jumpCooldown;
    }
}
