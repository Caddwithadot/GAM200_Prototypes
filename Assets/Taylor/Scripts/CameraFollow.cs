using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform followTarget;
    public Vector3 offset;

    public float interpolant = 0.15f;

    // Update is called once per frame
    private void FixedUpdate()
    {
        Vector3 target = followTarget.position + offset;
        transform.position = Vector3.Lerp(transform.position, target, interpolant);
    }
}
