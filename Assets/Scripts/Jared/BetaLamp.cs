using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BetaLamp : MonoBehaviour
{
    public List<GameObject> ObjectsToDisable = new List<GameObject>();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            for (int i = 0; i < ObjectsToDisable.Count; i++)
            {
                ObjectsToDisable[i].SetActive(false);
            }
        }
    }
}
