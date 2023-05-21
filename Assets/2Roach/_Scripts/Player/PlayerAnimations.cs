using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{

    [SerializeField]  IPlayerController _player;      
    [SerializeField]  AudioSource _sourceMoving;      
    [SerializeField]  Animator _animator;      

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        _player = GetComponent<IPlayerController>();
    }

    private void LateUpdate()
    {
        //_sourceMoving.mute = !_player.IsMoving;
       _animator.SetBool("isMoving", _player.IsMoving);
       _animator.SetBool("isCarry", _player.IsCarrying);
       _animator.SetFloat("input", _player.MoveInput.magnitude);
    }

}
