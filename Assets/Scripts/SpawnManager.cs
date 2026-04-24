using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] prefabs;   
    public Transform spawnPoint;  

    private int currentIndex = 0;

    public void SpawnNext()
    {
        if (prefabs.Length == 0) return;

        Instantiate(prefabs[currentIndex], spawnPoint.position, spawnPoint.rotation,spawnPoint);

        currentIndex++;

        if (currentIndex >= prefabs.Length)
            currentIndex = 0;
    }
}