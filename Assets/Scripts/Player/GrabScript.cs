using System.Collections;
using UnityEngine;

public class GrabScript : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private float extraPullForce;
    [SerializeField] private float grabDistance;

    private bool grabbed;
    private GameObject grabbedObj;
    private Rigidbody2D grabbedRb;
    private SpringJoint2D grabJoint;
    private bool cooldown;

    void Update()
    {
        // If object was destroyed externally, clean up safely
        if (grabbed && grabbedObj == null)
        {
            SafeReset();
            return;
        }

        CheckGrab();
    }

    private void CheckGrab()
    {
        Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = -3;

        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector3.forward);

        if (Input.GetMouseButtonDown(0) && hit.collider != null && !grabbed)
        {
            float distanceToPlayer = Vector2.Distance(hit.collider.transform.position, transform.position);
            if (hit.collider.tag != "Grabbable" || distanceToPlayer > grabDistance)
                return;

            Vector2 grabPoint = hit.collider.transform.InverseTransformPoint(hit.point);
            Pickup(hit.collider.gameObject, grabPoint);
        }
        else if (grabbed)
        {
            HandleGrab();
        }

        if (Input.GetMouseButtonUp(0))
        {
            drop();
        }
    }

    private void HandleGrab()
    {
        // Safety: joint or object destroyed mid-grab
        if (grabbedObj == null || grabJoint == null || grabbedRb == null)
        {
            SafeReset();
            return;
        }

        float distanceToPlayer = Vector2.Distance(transform.position, grabbedObj.transform.position);
        if (distanceToPlayer > grabDistance)
        {
            drop();
            return;
        }

        Vector2 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        grabJoint.connectedAnchor = mousePos;
        grabJoint.distance = -1;

        Vector2 dir = mousePos - (Vector2)grabbedObj.transform.position;
        grabbedRb.AddForce(dir.normalized * extraPullForce);
    }

    public void Pickup(GameObject obj, Vector2 grabPoint)
    {
        grabbedObj = obj;
        grabbedRb = obj.GetComponent<Rigidbody2D>();
        grabbedRb.drag = 4;
        grabbedRb.angularDrag = 4;

        grabbed = true;

        grabJoint = obj.AddComponent<SpringJoint2D>();
        grabJoint.enableCollision = true;
        grabJoint.anchor = grabPoint;        // ✔ EXACT spot of click preserved
        grabJoint.frequency = 10;
        grabJoint.dampingRatio = 1;
        grabJoint.autoConfigureDistance = false;
    }

    public void drop()
    {
        if (!grabbed) return;

        // Safe even if object is breaking/destroying
        if (grabbedRb != null)
        {
            grabbedRb.drag = 0;
            grabbedRb.angularDrag = 0.05f;
        }

        if (grabJoint != null)
        {
            Destroy(grabJoint);
        }

        grabbedObj = null;
        grabbedRb = null;
        grabJoint = null;
        grabbed = false;
    }

    // Called by break scripts BEFORE destroying the grabbable object
    public void ForceDropIfHolding(GameObject obj)
    {
        if (grabbed && grabbedObj == obj)
        {
            drop();
        }
    }

    private void SafeReset()
    {
        grabbed = false;
        grabbedObj = null;
        grabbedRb = null;
        grabJoint = null;
    }

    private IEnumerator dropcooldown()
    {
        yield return new WaitForSeconds(0.1f);
        cooldown = true;
    }
}
