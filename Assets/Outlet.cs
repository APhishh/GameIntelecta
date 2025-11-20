using UnityEngine;

public class Outlet : MonoBehaviour
{
    public bool isOccupied;
    private Plug currentPlug;

    [Header("Power Actions")]
    [SerializeField] private GameObject invisibleWall;  // delete AFTER cable finishes
    [SerializeField] private CableTest cable;           // cable to extend
    [SerializeField] private float targetMaxLength = 6f;
    [SerializeField] private float growSpeed = 1f;

    [Header("Box Settings")]
    [SerializeField] private GameObject boxToFreeze;    // box whose rigidbody will be removed

    private bool alreadyPowered = false;

    private void OnTriggerExit2D(Collider2D collision)
    {
        Plug plug = collision.GetComponent<Plug>();
        if (plug != null && plug == currentPlug)
        {
            RemovePlug();
        }
    }

    public void OnPluggedIn(Plug plug)
    {
        if (isOccupied) return;
        if (plug.isBroken) return;

        isOccupied = true;
        currentPlug = plug;

        Debug.Log("Plug inserted into outlet!");

        ActivateOutlet();
    }

    private void ActivateOutlet()
    {
        if (alreadyPowered) return;
        alreadyPowered = true;

        Debug.Log("Outlet powered!");

        // Start cable growth first — wall removal and box freeze happen after
        if (cable != null)
        {
            StartCoroutine(GrowCableThenFinish());
        }
    }

    private System.Collections.IEnumerator GrowCableThenFinish()
    {
        // Extend the cable first
        while (cable.maxLength < targetMaxLength)
        {
            cable.maxLength += Time.deltaTime * growSpeed;
            yield return null;
        }

        // Ensure final value is exact
        cable.maxLength = targetMaxLength;

        // ---------------------------------------
        // AFTER cable finishes extending:
        // Delete invisible wall
        // Freeze box
        // ---------------------------------------

        if (invisibleWall != null)
            Destroy(invisibleWall);

        if (boxToFreeze != null)
        {
            Rigidbody2D rb = boxToFreeze.GetComponent<Rigidbody2D>();
            if (rb != null)
                Destroy(rb); // removes physics and makes the box completely stationary
        }

        Debug.Log("Cable finished extending — wall removed, box frozen.");
    }

    private void RemovePlug()
    {
        if (!isOccupied) return;

        Debug.Log("Plug removed from outlet.");

        isOccupied = false; 
        currentPlug = null;
    }
}
