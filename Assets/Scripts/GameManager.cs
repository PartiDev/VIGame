using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    public static GameManager Instance
    {
        get
        {
            if (_instance is null)
                Debug.LogError("GameManager is null");

            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {

        }
        else
        {
            DontDestroyOnLoad(gameObject);
            _instance = this;
        }

        Application.targetFrameRate = 60;
        Time.timeScale = 1f;
        StartGame();
    }
    private void StartGame()
    {
        Debug.Log("Starting game...");

    }
}
