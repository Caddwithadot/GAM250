using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    public int BuildIndex = 0;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        SceneManager.LoadScene(BuildIndex);
    }
}
