using UnityEngine;

public class ObjectGrabbable : MonoBehaviour
{

    private Rigidbody rb;
    [HideInInspector] public Transform GrabPoint;
    private Transform OriginalParent;
    [HideInInspector] public bool IsGrabbed = false; 
    

    private void Awake() 
    {
        rb = GetComponent<Rigidbody>();
        OriginalParent = transform.parent;
    }

    public void Grab(Transform GrabPoint)
    {
        IsGrabbed = true;
        this.GrabPoint = GrabPoint;
        rb.useGravity = false;
        rb.isKinematic = false;
        rb.velocity = Vector3.zero;
        rb.constraints = RigidbodyConstraints.None;
        
        transform.parent = OriginalParent;
    }
    public void Drop()
    {
        IsGrabbed = false;
        GrabPoint = null;
        rb.useGravity = true;
        rb.velocity = Vector3.zero;
        rb.constraints = RigidbodyConstraints.None;
        
        //transform.parent = OriginalParent;
    }
    private void Update() 
    { 
        if (GrabPoint != null)
        {
            if (IsGrabbed)
            {
                transform.position = GrabPoint.position;
            }
        }
    }
}