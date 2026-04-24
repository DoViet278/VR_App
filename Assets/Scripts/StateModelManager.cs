using System;
using UnityEngine;

public class StateModelManager : MonoBehaviour
{
    public static StateModelManager Instance { get; private set; }
    public int currentState = 0; // 0: gui3, 1: min, 2: mid, 3: max
    public int maxState = 3;

    public event Action<int> OnStateChanged;

    private void Awake()
    {
        Instance = this;
    }

    public void IncreaseState()
    {
        if (currentState < maxState)
        {
            currentState++;
            OnStateChanged?.Invoke(currentState);
        }
    }

    public void DecreaseState()
    {
        if (currentState > 0)
        {
            currentState--;
            OnStateChanged?.Invoke(currentState);
        }
    }
}
