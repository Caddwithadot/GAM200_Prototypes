using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class ParticleTest : MonoBehaviour
{
    public PolygonCollider2D polygonCollider;
    public ParticleSystem particleSystem;

    public Transform player;
    public float offset = -0.28f;
    private float playerOffset;

    void Start()
    {
        if(player == null)
        {
            player = GameObject.FindWithTag("Player").transform;
        }

        if (polygonCollider == null || particleSystem == null)
        {
            polygonCollider = GetComponent<PolygonCollider2D>();
            particleSystem = GetComponent<ParticleSystem>();
        }
    }

    private void Update()
    {
        playerOffset = player.position.y + offset;

        EmitParticlesFromPolygon();
    }

    void EmitParticlesFromPolygon()
    {
        Vector2[] points = polygonCollider.GetPath(0); // Assuming only one path in the collider

        ParticleSystem.EmitParams emitParams = new ParticleSystem.EmitParams();

        foreach (Vector2 point in points)
        {
            if(point.y > playerOffset + 0.001f || point.y < playerOffset - 0.001f)
            {
                emitParams.position = new Vector3(point.x, point.y, 0f);
                particleSystem.Emit(emitParams, 1);
            }
        }
    }
}