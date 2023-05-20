using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] SceneLoader _sceneLoader;
    [SerializeField] RoomState _gameState;
    public RoomState GameState { get => _gameState;  }


    public static GameManager instance;

    void Awake()
    {

        if (instance != null && instance != this)
            Destroy(this.gameObject);
        else
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }
    }
}
