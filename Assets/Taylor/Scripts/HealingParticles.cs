using UnityEngine;

public class HealingParticles : MonoBehaviour
{
    public BoxCollider2D boxCollider;
    public ParticleSystem particleSystem;

    public int numberOfParticlesToEmit = 5;

    void Start()
    {
        if (boxCollider == null || particleSystem == null)
        {
            boxCollider = GetComponent<BoxCollider2D>();
            particleSystem = GetComponent<ParticleSystem>();
        }
    }

    private void Update()
    {
        if(Input.GetKey(KeyCode.P))
        {
            EmitParticlesFromBoxCollider();
        }
    }

    public void EmitParticlesFromBoxCollider()
    {
        // Get the bounds of the box collider
        Vector2 minBounds = boxCollider.bounds.min;
        Vector2 maxBounds = boxCollider.bounds.max;

        ParticleSystem.EmitParams emitParams = new ParticleSystem.EmitParams();

        for (int i = 0; i < numberOfParticlesToEmit; i++)
        {
            // Get a random point within the bounds of the box collider
            Vector2 randomPoint = GetRandomPointInBounds(minBounds, maxBounds);

            // Set the particle position
            emitParams.position = new Vector3(randomPoint.x, randomPoint.y, 0f);

            // Emit the particle with the specified lifetime
            particleSystem.Emit(emitParams, 1);
        }
    }

    Vector2 GetRandomPointInBounds(Vector2 minBounds, Vector2 maxBounds)
    {
        // Generate a random point within the bounds of the box collider
        float randomX = Random.Range(minBounds.x, maxBounds.x);
        float randomY = Random.Range(minBounds.y, maxBounds.y);

        return new Vector2(randomX, randomY);
    }
}