using System;
using System.Collections;
using System.Collections.Generic;
using DungeonArchitect.UI.Widgets;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField] Image _gameOverImage;

    

    public static MenuManager Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    
        if (_gameOverImage != null)
        {
            _gameOverImage.gameObject.SetActive(false);
        }
    }

    public void DislayEndScreen()
    {
        if (_gameOverImage != null)
        {
            _gameOverImage.gameObject.SetActive(true);
        }

        TimeManager.Instance.SetTime(0);
    }
}