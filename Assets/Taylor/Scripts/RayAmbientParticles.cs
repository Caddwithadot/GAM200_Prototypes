using System.Collections;
using UnityEngine;

public class RayAmbientParticles : MonoBehaviour
{
    public PolygonCollider2D polygonCollider;
    public ParticleSystem ps;

    public int numberOfParticlesToEmit = 100;

    private Transform player;
    public float particleSpeed = 5f;

    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;

        if (polygonCollider == null || ps == null)
        {
            polygonCollider = GetComponent<PolygonCollider2D>();
            ps = GetComponent<ParticleSystem>();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            EmitParticlesFromPolygon();
        }

        EmitParticlesFromPolygon();
    }

    public void EmitParticlesFromPolygon()
    {
        Vector2[] points = polygonCollider.GetPath(0); // Assuming only one path in the collider

        ParticleSystem.EmitParams emitParams = new ParticleSystem.EmitParams();

        for (int i = 0; i < numberOfParticlesToEmit; i++)
        {
            // Get a random point within the polygon collider
            Vector2 randomPoint = GetRandomPointInPolygon(points);

            // Calculate the direction from the player to the random point
            Vector2 direction = (randomPoint - (Vector2)player.position).normalized;

            // Set the particle position
            emitParams.position = new Vector3(randomPoint.x, randomPoint.y, 0f);

            // Set the particle velocity to move in the opposite direction of the player
            emitParams.velocity = direction * particleSpeed;

            // Emit the particle with the specified lifetime
            ps.Emit(emitParams, 1);
        }
    }

    Vector2 GetRandomPointInPolygon(Vector2[] polygonPoints)
    {
        // Compute the bounds of the polygon
        float minX = float.MaxValue, minY = float.MaxValue;
        float maxX = float.MinValue, maxY = float.MinValue;

        foreach (Vector2 point in polygonPoints)
        {
            minX = Mathf.Min(minX, point.x);
            minY = Mathf.Min(minY, point.y);
            maxX = Mathf.Max(maxX, point.x);
            maxY = Mathf.Max(maxY, point.y);
        }

        // Generate a random point within the bounds of the polygon
        float randomX = Random.Range(minX, maxX);
        float randomY = Random.Range(minY, maxY);

        // Check if the point is within the polygon; if not, generate a new point
        while (!IsPointInPolygon(new Vector2(randomX, randomY), polygonPoints))
        {
            randomX = Random.Range(minX, maxX);
            randomY = Random.Range(minY, maxY);
        }

        return new Vector2(randomX, randomY);
    }

    bool IsPointInPolygon(Vector2 point, Vector2[] polygonPoints)
    {
        int j = polygonPoints.Length - 1;
        bool inside = false;

        for (int i = 0; i < polygonPoints.Length; j = i++)
        {
            if (((polygonPoints[i].y <= point.y && point.y < polygonPoints[j].y) ||
                 (polygonPoints[j].y <= point.y && point.y < polygonPoints[i].y)) &&
                (point.x < (polygonPoints[j].x - polygonPoints[i].x) * (point.y - polygonPoints[i].y) / (polygonPoints[j].y - polygonPoints[i].y) + polygonPoints[i].x))
            {
                inside = !inside;
            }
        }

        return inside;
    }
}