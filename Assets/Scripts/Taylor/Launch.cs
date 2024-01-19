using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Launch : MonoBehaviour
{
    private SwappingAndRecall swap;

    public GameObject arrowPrefab;
    private GameObject arrow;
    private Transform knight;

    public GameObject testObj;
    public float launchForce;

    private void Start()
    {
        swap = GetComponent<SwappingAndRecall>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && swap.amKnight)
        {
            knight = swap.Knight.transform;

            arrow = Instantiate(arrowPrefab, knight);
        }

        if(Input.GetKeyUp(KeyCode.Space))
        {
            Vector3 launchDirection = arrow.GetComponent<FaceMouse>().direction;

            GameObject launchedObj = Instantiate(testObj, knight);
            launchedObj.GetComponent<Rigidbody2D>().velocity = launchDirection.normalized * launchForce;

            Destroy(arrow);
        }
    }
}
