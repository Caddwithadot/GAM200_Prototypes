using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowWithoutParenting : MonoBehaviour
{
    public GameObject followingTarget;

    void Update()
    {
        transform.position = followingTarget.transform.position;
    }
}
