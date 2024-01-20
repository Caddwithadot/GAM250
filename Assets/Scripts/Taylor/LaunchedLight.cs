using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchedLight : MonoBehaviour
{
    private bool isGrounded = false;

    private float launchedDuration = 0.5f;
    private float timer = 0f;

    // Update is called once per frame
    void Update()
    {
        //timer += Time.deltaTime;

        if (isGrounded || timer >= launchedDuration)
        {
            GetComponent<PlayerMovementNEW>().enabled = true;
            Destroy(this);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Environment" || collision.tag == "JumpThrough")
        {
            isGrounded = true;
        }
    }
}
