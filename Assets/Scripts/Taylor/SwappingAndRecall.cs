using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwappingAndRecall : MonoBehaviour
{
    public GameObject knightPrefab;
    public GameObject lightPrefab;
    public GameObject armorPrefab;
    public GameObject recallPrefab;

    [HideInInspector]
    public GameObject Knight;

    [HideInInspector]
    public GameObject Light;

    [HideInInspector]
    public GameObject Armor;

    private GameObject Recall;

    private CameraFollow cameraFollow;

    public bool amKnight = true;
    private bool amRecalling = false;

    public float lightHopForce = 10f;
    public float minRecallSpeed = 15f;
    public float maxRecallSpeed = 25f;

    //
    public bool launchUnlocked = false;

    // Start is called before the first frame update
    void Start()
    {
        cameraFollow = FindObjectOfType<CameraFollow>();

        Knight = Instantiate(knightPrefab, transform);

        cameraFollow.followTarget = Knight.transform;
    }

    // Update is called once per frame
    void Update()
    {
        //return to Knight
        if(Input.GetKeyDown(KeyCode.R) && !amKnight && !amRecalling)
        {
            amKnight = !amKnight;

            BecomeKnight();
        }

        //become Light
        if (Input.GetKeyDown(KeyCode.Space) && amKnight && !launchUnlocked)
        {
            amKnight = !amKnight;

            BecomeLight();
        }
    }

    public void BecomeLight()
    {
        //spawn armor on knight
        Armor = Instantiate(armorPrefab, Knight.transform.position, Quaternion.identity, transform);
        Destroy(Knight);

        //spawn Light on the Armoa
        Light = Instantiate(lightPrefab, new Vector2(Armor.transform.position.x, Armor.transform.position.y + 0.1f), Quaternion.identity, transform);
        Rigidbody2D lightRB = Light.GetComponent<Rigidbody2D>();

        lightRB.velocity = new Vector2(lightRB.velocity.x, lightHopForce);

        //set Light to the camera target
        cameraFollow.followTarget = Light.transform;
    }

    public void BecomeKnight()
    {
        // Spawn a new sprite next to the Light character
        Recall = Instantiate(recallPrefab, Light.transform.position, Quaternion.identity, transform);
        Destroy(Light);

        cameraFollow.followTarget = Recall.transform;

        // Lerping the new sprite's position to the Armor
        StartCoroutine(RecallToArmor(Recall));
    }

    //coroutine to recall to Armor
    private IEnumerator RecallToArmor(GameObject recallObject)
    {
        amKnight = false;

        float distanceToArmor = Vector3.Distance(Light.transform.position, Armor.transform.position);
        float currentLerpSpeed = (distanceToArmor <= 10f) ? minRecallSpeed : maxRecallSpeed;

        amRecalling = true;

        while (distanceToArmor > 0.1f)
        {
            recallObject.transform.position = Vector3.MoveTowards(recallObject.transform.position, Armor.transform.position, currentLerpSpeed * Time.deltaTime);

            distanceToArmor = Vector3.Distance(recallObject.transform.position, Armor.transform.position);

            yield return null;
        }

        Destroy(recallObject);

        // Spawn Knight at armor
        Knight = Instantiate(knightPrefab, Armor.transform.position, Quaternion.identity, transform);
        Destroy(Armor);

        //set Knight to the camera target
        cameraFollow.followTarget = Knight.transform;
        amKnight = true;
        amRecalling = false;
    }
}
