using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;
    public string[] sceneName;

    private void Awake()
    {
        instance = this;
    }

    public void LoadLevel(int index)
    {
        Debug.Log("Loading level " + index);
        SceneManager.LoadScene("BasicScene");
    }
}
