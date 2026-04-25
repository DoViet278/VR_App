using System;
using UnityEngine;

public class Map : MonoBehaviour
{
    public GameObject panel;
    public Boolean isMultiple;

    public void ShowModel()
    {
        // Chi co panel Da Nang co 2 panel cung luc
        panel.SetActive(true);
    }
}
