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
                _state = RoomState.Saloon;
                 StartCoroutine(COR_GoToSaloon());
                OnStateChange();
                Debug.Log("Switching to Saloon");
            break;

            case  RoomState.Saloon:
                _state = RoomState.Kitchen;
                 StartCoroutine(COR_GoToKitchen());
                 OnStateChange();
                Debug.Log("Switching to Kitchen");
            break;
        }        

    }

    private IEnumerator COR_GoToSaloon()
    {
        _input.DisableAllInput();
        //Camera Magic
        //Vignette Magic
        yield return Yielders.Get(1f);
        _player.Switch(RoomState.Saloon);
        _input.EnableGameplayInput();
        TriggerFirstWave();
    }

    private IEnumerator COR_GoToKitchen()
    {
        _input.DisableAllInput();
        //Camera Magic
        //Vignette Magic
        yield return Yielders.Get(1f);
        _player.Switch(RoomState.Kitchen);
        _input.EnableGameplayInput();
    }

    private void TriggerFirstWave()
    {
        if(_hasBeenOnSaloon) return;
        _hasBeenOnSaloon =true;
        _waveManager.StartFirstWave();
    }
    private void OnStateChange()
    {
        //Debug.Log("Play Common FX here!!!");
    }

}
