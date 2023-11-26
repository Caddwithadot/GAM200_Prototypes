using UnityEngine;

public class FollowParticles : MonoBehaviour
{
    public ParticleSystem particleSystem;
    private Transform followTarget;

    public float moveSpeed = 5f;

    private void Start()
    {
        followTarget = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        MoveParticles();
    }

    void MoveParticles()
    {
        if (particleSystem != null)
        {
            ParticleSystem.Particle[] particles = new ParticleSystem.Particle[particleSystem.main.maxParticles];
            int particleCount = particleSystem.GetParticles(particles);

            for (int i = 0; i < particleCount; i++)
            {
                particles[i].position = Vector3.MoveTowards(particles[i].position, followTarget.position, Time.deltaTime * moveSpeed);

                if (Vector3.Distance(particles[i].position, followTarget.position) < 0.1f)
                {
                    particles[i].remainingLifetime = 0f;
                }
            }

            particleSystem.SetParticles(particles, particleCount);
        }
    }
}