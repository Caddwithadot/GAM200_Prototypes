using System.Collections;
using UnityEngine;

public class OverheatParticles : MonoBehaviour
{
    public PolygonCollider2D polygonCollider;
    public ParticleSystem particleSystem;

    void Start()
    {
        if (polygonCollider == null || particleSystem == null)
        {
            polygonCollider = GetComponent<PolygonCollider2D>();
            particleSystem = GetComponent<ParticleSystem>();
        }
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.P))
        {
            EmitParticlesFromPolygon();
        }
    }

    void EmitParticlesFromPolygon()
    {
        Vector2[] points = polygonCollider.GetPath(0); // Assuming only one path in the collider

        ParticleSystem.EmitParams emitParams = new ParticleSystem.EmitParams();

        // Adjust the following values based on your requirements
        int numberOfParticlesToEmit = 5;
        float particleLifetime = 2f;

        for (int i = 0; i < numberOfParticlesToEmit; i++)
        {
            // Get a random point within the polygon collider
            Vector2 randomPoint = GetRandomPointInPolygon(points);

            // Set the particle position
            emitParams.position = new Vector3(randomPoint.x, randomPoint.y, 0f);

            // Emit the particle with the specified lifetime
            particleSystem.Emit(emitParams, 1);
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