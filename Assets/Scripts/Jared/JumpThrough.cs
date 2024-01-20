using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpThrough : MonoBehaviour
{
    private BoxCollider2D boxCollider2D;
    public GameObject currentPlayer;
    public float Offset;

    private void Start()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        currentPlayer = GameObject.FindObjectOfType<PlayerMovementNEW>().gameObject;

        if (currentPlayer.transform.position.y + Offset <= transform.position.y)
        {
            boxCollider2D.enabled = false;
        }
        else
        {
            boxCollider2D.enabled = true;
        }
    }
}
