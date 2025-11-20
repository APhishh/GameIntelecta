using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class CableTest : MonoBehaviour
{
    [Header("Cable Setup")]
    public Transform startPoint;
    public Rigidbody2D endPointRb; // The object at the end of the cable
    public int segments = 20;
    public float baseSagAmount = 0.3f;
    public float sagFalloff = 0.25f;

    [Header("Spring Settings")]
    public float maxLength = 3f;        // Cable length before it starts pulling back
    public float springForce = 20f;     // How strong the pullback is
    public float damping = 5f;          // How much it resists oscillation
    public float cableSize = 0.05f;

    private LineRenderer line;
    private Vector2 velocity; // for damping motion

    void Awake()
    {
        line = GetComponent<LineRenderer>();
        line.positionCount = segments + 1;

        line.startWidth = cableSize;
        line.endWidth = cableSize;

        if (line.material == null)
        {
            line.material = new Material(Shader.Find("Sprites/Default"));
            line.material.color = Color.black;
        }
    }

    void FixedUpdate()
    {
         if(endPointRb == null)
            return;
        Vector2 dir = endPointRb.position - (Vector2)startPoint.position;
        float dist = dir.magnitude;

        if (dist > maxLength)
        {
            // Calculate stretch distance
            float stretch = dist - maxLength;

            // Normalize direction
            Vector2 pullDir = dir.normalized;

            // Apply spring-like force toward start point
            Vector2 spring = -pullDir * (stretch * springForce);

            // Damping: counteracts current velocity to prevent endless bouncing
            Vector2 dampingForce = -endPointRb.velocity * damping;

            endPointRb.AddForce(spring + dampingForce);
        }
    }

    void Update()
    {
        // Update cable sag visually
         if(endPointRb == null)
            return;
        Vector3 start = startPoint.position;
        Vector3 end = endPointRb.position;
        float distance = Vector3.Distance(start, end);
        float sagAmount = Mathf.Max(0f, baseSagAmount - distance * sagFalloff);

        for (int i = 0; i <= segments; i++)
        {
            float t = i / (float)segments;
            Vector3 pos = Vector3.Lerp(start, end, t);

            float sag = Mathf.Sin(t * Mathf.PI) * sagAmount;
            pos.y -= sag;

            line.SetPosition(i, pos);
        }
    }
}