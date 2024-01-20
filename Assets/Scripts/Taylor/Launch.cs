using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Launch : MonoBehaviour
{
    private SwappingAndRecall swap;
    private CameraFollow cameraFollow;

    public GameObject arrowPrefab;
    private GameObject Arrow;

    public GameObject armorPrefab;
    public GameObject lightPrefab;
    private GameObject Light;
    private GameObject Armor;
    private GameObject Knight;

    private bool holding = false;
    private float heldTime;
    public float heldDuration = 0.25f;
    public float launchForce;

    private void Start()
    {
        swap = GetComponent<SwappingAndRecall>();
        cameraFollow = FindObjectOfType<CameraFollow>();
    }

    // Update is called once per frame
    void Update()
    {
        //prepare launch trajectory
        if (Input.GetKeyDown(KeyCode.Space) && swap.amKnight)
        {
            holding = true;
        }

        if(Input.GetKeyUp(KeyCode.Space) && swap.amKnight)
        {
            holding = false;

            //launch out of armor
            if(heldTime >= heldDuration)
            {
                Vector3 launchDirection = Arrow.GetComponent<FaceMouse>().direction;

                //spawn armor on knight
                Armor = Instantiate(armorPrefab, Knight.transform.position, Quaternion.identity, transform);
                swap.Armor = Armor;
                Destroy(Knight);

                Light = Instantiate(lightPrefab, new Vector2(Armor.transform.position.x, Armor.transform.position.y + 0.1f), Quaternion.identity, transform);
                swap.Light = Light;

                Light.GetComponent<PlayerMovementNEW>().enabled = false;
                Light.AddComponent<LaunchedLight>();

                Rigidbody2D lightRB = Light.GetComponent<Rigidbody2D>();
                lightRB.velocity = launchDirection.normalized * launchForce;

                swap.amKnight = false;
                cameraFollow.followTarget = Light.transform;

                Destroy(Arrow);
            }
            else
            {
                //hop out of armor
                swap.amKnight = false;
                swap.BecomeLight();
            }

            heldTime = 0;
        }

        if (holding)
        {
            heldTime += Time.deltaTime;

            if(heldTime >= heldDuration)
            {
                Knight = swap.Knight;
                Arrow = Instantiate(arrowPrefab, Knight.transform);

                holding = false;
            }
        }
    }
}
