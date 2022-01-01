using System;
using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    void Awake()
    {
        Instance = this;
    }
    public void Begin()
    {
        StartCoroutine(BeginGame());
    }

    IEnumerator BeginGame()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(3, LoadSceneMode.Additive);
        while (operation.isDone == false)
            yield return null;
        
        Debug.Log("Game Beginning");
        PlayerManager.Instance.SpawnPlayerCharacters();
    }

  
}
