using UnityEngine;

public class CowardPoint : MonoBehaviour
{
    private Transform player;
    public Transform cowardPoint;
    public float distanceOffset = 2.0f; // Offset distance from the target.

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        // Calculate the distance between this object and the target object.
        float distance = Vector3.Distance(transform.position, player.position);

        // Set the position of the relative object based on the distance and offset.
        Vector3 relativePosition = player.position + (transform.position - player.position).normalized * (distance + distanceOffset);
        cowardPoint.position = relativePosition;
    }
}