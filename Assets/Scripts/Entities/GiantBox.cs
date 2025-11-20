using UnityEngine;
using System.Collections;

public class GiantBox : MonoBehaviour
{
    [Header("Box Settings")]
    [SerializeField] private float health = 100f;

    [Header("Damage Particles")]
    public ParticleSystem damageParticles;   // Assign in Inspector

    [Header("Garbage Prefabs (4 types)")]
    [SerializeField] private GameObject garbage1;
    [SerializeField] private GameObject garbage2;
    [SerializeField] private GameObject garbage3;
    [SerializeField] private GameObject garbage4;

    [Header("Loot Prefab")]
    [SerializeField] private GameObject plugLoot;

    public void TakeDamage(float damage)
    {
        health -= damage;

        SpawnDamageParticles();

        if (health <= 0)
        {
            DestroyBox();
        }
    }

    private void SpawnDamageParticles()
    {
        if (damageParticles != null)
        {
            // Make particle face the camera
            Quaternion faceCamera = Quaternion.LookRotation(Vector3.forward, Vector3.up);

            ParticleSystem ps = Instantiate(
                damageParticles,
                transform.position + Vector3.forward * -0.1f,
                faceCamera // correct rotation for 2D
            );

            ps.Play();

            // Schedule automatic destruction of the particle system gameObject.
            // We compute a conservative lifetime using the particle system's duration
            // plus its max startLifetime so the particles have time to finish.
            var main = ps.main;
            float startLifetimeMax = Mathf.Max(main.startLifetime.constantMin, main.startLifetime.constantMax);
            // If startLifetime isn't set as min/max, constantMax might be 0 — fall back to constant
            if (startLifetimeMax <= 0f)
                startLifetimeMax = main.startLifetime.constant;

            float lifetime = main.duration + startLifetimeMax;
            Destroy(ps.gameObject, Mathf.Max(0.1f, lifetime));
        }
    }

    private IEnumerator DeleteParticle(ParticleSystem ps)
    {
        // kept only in case you want to use it elsewhere; not used for final particle deletion
        yield return new WaitForSeconds(0.4f);
        if (ps != null)
            Destroy(ps.gameObject);
    }

    private void DestroyBox()
    {
        if (damageParticles != null)
        {
            SpawnDamageParticles();
        }
        SpawnGarbage();
        SpawnLoot();
        Destroy(gameObject);
    }

    // -------------------------
    // NEW FUNCTIONS BELOW
    // -------------------------

    private void SpawnGarbage()
    {
        GameObject[] garbagePool = new GameObject[]
        {
            garbage1, garbage2, garbage3, garbage4
        };

        for (int i = 0; i < 7; i++)
        {
            GameObject prefab = garbagePool[Random.Range(0, garbagePool.Length)];

            Vector3 offset = new Vector3(
                Random.Range(-0.5f, 0.5f),
                Random.Range(-0.2f, 0.2f),
                0
            );

            Instantiate(prefab, transform.position + offset, Quaternion.identity);
        }
    }

    private void SpawnLoot()
    {
        if (plugLoot != null)
        {
            Instantiate(plugLoot, transform.position, Quaternion.identity);
        }
    }
}
