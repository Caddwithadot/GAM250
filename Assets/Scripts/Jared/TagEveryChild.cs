using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TagEveryChild : MonoBehaviour
{
    public string TagToApply = "";

    void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).tag = TagToApply;
        }
    }
}
