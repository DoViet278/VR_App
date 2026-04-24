using System.Collections.Generic;
using UnityEngine;

public class TowerPart : MonoBehaviour
{
    private MeshRenderer rend;

    // Mỗi state = 1 list materials
    public List<MaterialSet> states;

    private void Start()
    {
        StateModelManager.Instance.OnStateChanged += UpdateMaterial;

        UpdateMaterial(StateModelManager.Instance.currentState);
    }

    void UpdateMaterial(int state)
    {
        if (state < states.Count)
        {
            rend.materials = states[state].materials.ToArray();
        }
    }

    private void OnDestroy()
    {
        if (StateModelManager.Instance != null)
            StateModelManager.Instance.OnStateChanged -= UpdateMaterial;
    }
}
