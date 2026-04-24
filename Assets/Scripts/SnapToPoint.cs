using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

public class SnapToPoint : MonoBehaviour
{
    public Transform snapPoint;    
    public float snapDistance = 3f;
    public UnityEvent onSnapped = new UnityEvent();

    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grabInteractable;
    private Rigidbody rb;
    private GameObject foundedObject;

    private void Awake()
    {
        grabInteractable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
        rb = GetComponent<Rigidbody>();

        if (snapPoint == null)
        {
            string cleanName = gameObject.name.Replace("(Clone)", "");
            string targetName = "pos_" + cleanName;

            foundedObject = GameObject.Find(targetName);

            if (foundedObject != null)
            {
                snapPoint = foundedObject.transform;
                Debug.Log("Found snap point: " + targetName);
            }
            else
            {
                Debug.LogError("Snap point NOT found: " + targetName);
            }
        }

        grabInteractable.selectExited.AddListener(OnRelease);
    }

    private void OnRelease(SelectExitEventArgs args)
    {
        Debug.LogError("Object released, checking for snap...");
        float distance = Vector3.Distance(transform.position, snapPoint.position);

        if (distance <= snapDistance)
        {
            Snap();
        }
    }

    private void Snap()
    {
        // Tắt physics để tránh rung
        rb.isKinematic = true;

        // Snap position + rotation
        transform.position = snapPoint.position;
        //transform.rotation = snapPoint.rotation;
        foundedObject.GetComponent<MeshRenderer>().enabled = false; // Ẩn đối tượng snap point nếu có
        // (Optional) khóa không cho grab nữa
        grabInteractable.enabled = false;
        Debug.LogError("Object snapped to point!");

        onSnapped?.Invoke();
    }
}