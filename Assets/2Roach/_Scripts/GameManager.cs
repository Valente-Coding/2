using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] SceneLoader _sceneLoader;
    [SerializeField] RoomState _gameState;
    public RoomState GameState { get => _gameState;  }
   
   public Score Score { get => _score;  }
    private Score _score = new Score();

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

    public void Win()
    {
        UIManager.instance.DisplayScore();
    }
}

public class Score
{
    private float _totalScore;
    public float TotalScore { get => _totalScore; }

    public void Add(float amount)
    {
        _totalScore += amount;
    }

    public void Remove(float amount)
    {
        _totalScore -= amount;
    }
}
