using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrolNEW : MonoBehaviour
{
    public Transform startPos;
    public Transform endPos;

    public float speed = 2.0f; // Set your desired speed here
    private float startTime;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = startPos.position;
        startTime = Time.time;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        float journeyLength = Vector2.Distance(new Vector2(startPos.position.x, startPos.position.y), new Vector2(endPos.position.x, startPos.position.y));
        float journeyTime = Time.time - startTime;

        float percent = Mathf.Clamp01(journeyTime / journeyLength);

        // Calculate the new X position and preserve the Y position
        float newX = Mathf.Lerp(startPos.position.x, endPos.position.x, percent);
        float newY = transform.position.y;

        transform.position = new Vector3(newX, newY, transform.position.z);

        if (percent >= 1.0f)
        {
            // Swap start and end positions to move back and forth
            Transform temp = startPos;
            startPos = endPos;
            endPos = temp;
            startTime = Time.time;
        }
    }
}