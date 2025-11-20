using UnityEngine;

public class Plug : MonoBehaviour
{
    [Header("Plug State")]
    public bool isPluggedIn;
    public bool isBroken = true;   // starts broken

    [Header("Sprites")]
    [SerializeField] private Sprite brokenSprite;
    [SerializeField] private Sprite fixedSprite;

    private SpriteRenderer sr;
    private FixedJoint2D joint;

    [SerializeField] private Vector2 offset;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = brokenSprite;   // ensure broken appearance
    }

    // -------------------------------------------------------
    // REPAIR LOGIC
    // -------------------------------------------------------
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isBroken)
        {
            TryRepair(collision.gameObject);
            return;
        }

        // Otherwise normal plugging
        if (isPluggedIn) return;

        // FIX: detect outlet through parent/child/itself
        Outlet outlet =
            collision.collider.GetComponent<Outlet>() ??
            collision.collider.GetComponentInParent<Outlet>() ??
            collision.collider.GetComponentInChildren<Outlet>();

        if (outlet != null)
        {
            PlugInto(outlet);
        }
    }

    private void TryRepair(GameObject other)
    {
        if (other.name == "Plug(Clone)")
        {
            RepairPlug(other);
        }
    }

    private void RepairPlug(GameObject plugObject)
    {
        GrabScript grabber = FindObjectOfType<GrabScript>();
        if (grabber != null)
        {
            grabber.ForceDropIfHolding(plugObject);
        }

        Destroy(plugObject);

        sr.sprite = fixedSprite;
        isBroken = false;

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.freezeRotation = false;
        }
    }

    // -------------------------------------------------------
    // NORMAL PLUG-IN LOGIC
    // -------------------------------------------------------
    public void PlugInto(Outlet outlet)
    {
        transform.position = outlet.transform.position + (Vector3)offset;

        // Stick plug to outlet
        joint = gameObject.AddComponent<FixedJoint2D>();
        joint.connectedBody = outlet.GetComponent<Rigidbody2D>();
        joint.autoConfigureConnectedAnchor = true;

        // Snap upright
        transform.rotation = Quaternion.identity;

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.angularVelocity = 0f;
        rb.freezeRotation = true;

        isPluggedIn = true;

        outlet.OnPluggedIn(this);
    }
}
    