using System;
using UnityEngine;

public class StateModelManager : MonoBehaviour
{
    public static StateModelManager Instance { get; private set; }
    public int currentState = 0; // 0: gui3, 1: min, 2: mid, 3: max
    public int maxState = 2;
    public bool willFracture = false; // Cờ để xác định có nên fracture hay không
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
            if(currentState == maxState)
            {
                Debug.LogError("Max state reached!");
                willFracture = true; 
            }
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
