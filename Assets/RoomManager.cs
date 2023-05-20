using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RoomState
{
    Kitchen,
    Saloon
}

public class RoomManager : MonoBehaviour
{
    [SerializeField] PlayerController _player;
    [SerializeField] WaveManager _waveManager;
    [SerializeField] RoomState _state;
    [SerializeField] InputReader _input;
    [SerializeField] bool _hasBeenOnSaloon = false;

    public RoomState State { get => _state;  }

    private void OnEnable()
    {
        _input.SwitchEvent += SwitchRoom;
    }

    private void OnDisable()
    {
        _input.SwitchEvent -= SwitchRoom;
    }


    private void SwitchRoom()
    {
        switch (_state)
        {               
            case RoomState.Kitchen:
                TriggerFirstWave();
                _state = RoomState.Saloon;
                _player.Switch(RoomState.Saloon);
                OnStateChange();
                Debug.Log("Switching to Saloon");
            break;

            case  RoomState.Saloon:
                 _state = RoomState.Kitchen;
                 _player.Switch(RoomState.Kitchen);
                 OnStateChange();
                Debug.Log("Switching to Kitchen");
            break;
        }        

    }

    private void TriggerFirstWave()
    {
        if(_hasBeenOnSaloon) return;
        _hasBeenOnSaloon =true;
        _waveManager.StartFirstWave();
    }
    private void OnStateChange()
    {
        Debug.Log("Play SFX here!!!");
    }

}
