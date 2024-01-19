using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceMouse : MonoBehaviour
{
    public Vector3 direction;

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePosition = Input.mousePosition;
        Vector3 worldMousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        direction = new Vector3(worldMousePosition.x, worldMousePosition.y, 0) - transform.position;
        transform.right = direction;
    }
}
