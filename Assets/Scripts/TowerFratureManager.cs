using System.Collections.Generic;
using UnityEngine;

public class TowerFractureManager : MonoBehaviour
{
    public static TowerFractureManager instance;
    private List<FracturablePart> parts = new List<FracturablePart>();

    private void Awake()
    {
        instance = this;
    }

    public void RegisterPart(FracturablePart part)
    {
        if (!parts.Contains(part))
        {
            parts.Add(part);
        }
    }

    public void FractureAll()
    {
        foreach (var part in parts)
        {
            Debug.LogError("Fracturing part: " + part.name);
            if (part != null)
            {
                part.Fracture();
            }
        }
    }

    public void FracturePart(int index)
    {
        if (index < 0 || index >= parts.Count) return;
        parts[index].Fracture();
    }
}