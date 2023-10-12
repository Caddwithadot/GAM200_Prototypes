using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpThroughParent : MonoBehaviour
{
    void Start()
    {
        foreach (Transform Child in transform.GetComponentInChildren<Transform>())
        {
            Child.gameObject.AddComponent<JumpThroughPlatform>();
        }
    }
}
