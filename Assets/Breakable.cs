using UnityEngine;

public class Breakable : MonoBehaviour
{
    [Header("Break Settings")]
    [SerializeField] private float breakVelocityThreshold = 8f;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip breakSound;
    [SerializeField] private Vector2 pitchRange = new Vector2(0.9f, 1.1f);

    [Header("Optional Effects")]
    [SerializeField] private GameObject breakParticles;

    private bool broken = false;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (broken) return;

        float impactForce = collision.relativeVelocity.magnitude;

        if (impactForce >= breakVelocityThreshold)
        {
            BreakObject();
        }
    }

    private void BreakObject()
    {
        broken = true;

        // Play sound with random pitch using a temporary AudioSource
        if (breakSound != null)
        {
            GameObject tempAudio = new GameObject("BreakSound");
            AudioSource tempSource = tempAudio.AddComponent<AudioSource>();
            tempSource.clip = breakSound;

            // Random pitch variation
            tempSource.pitch = Random.Range(pitchRange.x, pitchRange.y);
            tempSource.spatialBlend = 0f; // 2D sound
            tempSource.volume = audioSource.volume;

            tempSource.Play();
            Destroy(tempAudio, breakSound.length / tempSource.pitch);
        }

        // Spawn particles
        if (breakParticles != null)
        {
            Instantiate(breakParticles, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }
}
