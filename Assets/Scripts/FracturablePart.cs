using UnityEngine;

public class FracturablePart : MonoBehaviour
{
    private GameObject fracturedObject;
    private bool isBroken = false;
    private TowerFractureManager fractureManager;

    private void Awake()
    {
        string cleanName = gameObject.name.Replace("(Clone)", "");
        string fracturedName = cleanName + "_Fractured";
        fracturedObject = GameObject.Find(fracturedName);

        if (fracturedObject == null)
        {
            Debug.LogError("Không tìm thấy: " + fracturedName);
        }
        else
        {
            fracturedObject.transform.GetChild(0).gameObject.SetActive(false);
        }
    }

    private void Start()
    {
        fractureManager = GetComponentInParent<TowerFractureManager>();
        fractureManager?.RegisterPart(this);
    }

    public void Fracture()
    {
        if (isBroken || fracturedObject == null) return;

        //fracturedObject.transform.position = transform.position;
        //fracturedObject.transform.rotation = transform.rotation;
        //fracturedObject.transform.localScale = transform.localScale;
        Debug.LogError("Fracturing: " + fracturedObject.name);
        fracturedObject.transform.GetChild(0).gameObject.SetActive(true);

        Rigidbody[] rbs = fracturedObject.GetComponentsInChildren<Rigidbody>();
        foreach (Rigidbody rb in rbs)
        {
            rb.AddExplosionForce(200f, transform.position, 3f);
        }

        gameObject.SetActive(false);
        isBroken = true;
    }
}