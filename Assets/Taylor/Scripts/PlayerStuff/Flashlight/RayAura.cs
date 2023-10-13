using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RayAura : MonoBehaviour
{
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private LayerMask layerMask;
    private Mesh mesh;
    private float fov;
    private float viewDistance;
    private Vector3 origin;
    private float startingAngle;

    private PolygonCollider2D polygonCollider;

    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        fov = 363.5f;
        viewDistance = 2.5f;
        origin = Vector3.zero;

        //collider
        polygonCollider = GetComponent<PolygonCollider2D>();
    }

    private void LateUpdate()
    {
        int rayCount = 100;
        float angle = startingAngle;
        float angleIncrease = fov / rayCount;

        Vector2[] colliderPoints = new Vector2[rayCount + 1];
        Vector3[] vertices = new Vector3[rayCount + 1 + 1];
        int[] triangles = new int[rayCount * 3];

        // Initialize the first point as the origin (Vector2)
        colliderPoints[0] = new Vector2(origin.x, origin.y);
        vertices[0] = origin;

        int vertexIndex = 1;
        int triangleIndex = 0;

        for (int i = 1; i <= rayCount; i++)
        {
            Vector2 endpoint2D;
            Vector3 vertex;

            // Use a Raycast to check for collisions
            Ray ray = new Ray(origin, GetVectorFromAngle(angle));
            RaycastHit2D raycastHit2D = Physics2D.Raycast(origin, GetVectorFromAngle(angle), viewDistance, layerMask);

            if (raycastHit2D.collider == null)
            {
                // If no collision, set the endpoint to the ray's full length
                endpoint2D = origin + GetVectorFromAngle(angle) * viewDistance;
                vertex = new Vector3(endpoint2D.x, endpoint2D.y, 0);
            }
            else
            {
                // If collision, set the endpoint to the hit point
                endpoint2D = raycastHit2D.point;
                vertex = raycastHit2D.point;
            }

            colliderPoints[i] = endpoint2D;
            vertices[vertexIndex] = vertex;

            if (i > 1)
            {
                triangles[triangleIndex + 0] = 0;
                triangles[triangleIndex + 1] = vertexIndex - 1;
                triangles[triangleIndex + 2] = vertexIndex;

                triangleIndex += 3;
            }
            vertexIndex++;
            angle -= angleIncrease;
        }

        // Set the collider points
        polygonCollider.SetPath(0, colliderPoints);

        // Create or update the mesh
        if (mesh == null)
        {
            mesh = new Mesh();
            GetComponent<MeshFilter>().mesh = mesh;
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
    }

    public static Vector3 GetVectorFromAngle(float angle)
    {
        float angleRad = angle * (Mathf.PI / 180f);
        return new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
    }

    public static float GetAngleFromVectorFloat(Vector3 dir)
    {
        dir = dir.normalized;
        float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (n < 0)
        {
            n += 360;
        }

        return n;
    }

    public void SetOrigin(Vector3 origin)
    {
        this.origin = origin;
    }

    public void SetAimDirection(Vector3 aimDirection)
    {
        startingAngle = GetAngleFromVectorFloat(aimDirection) + fov / 2f;
    }

    public void SetFOV(float fov)
    {
        this.fov = fov;
    }

    public void SetViewDistance(float viewDistance)
    {
        this.viewDistance = viewDistance;
    }
}
