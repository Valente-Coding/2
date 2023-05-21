using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashBin : MonoBehaviour
{
    [SerializeField] private InputReader _input;
    [SerializeField] private PlayerController _playerControler;
    [SerializeField] private InteractTip _interactTip;
    [SerializeField] private SimpleAudioEvent _dump_Cue;

    private bool _canInteract = false;

    private void Start() {
        _input.InteractEvent += Interact;
    }
    
    private void Interact(bool newInput) {
        if (newInput == true && _canInteract == true && !_playerControler.CurrentStack.IsEmpty()) 
        {
            _playerControler.CurrentStack.ResetStack();
            _dump_Cue.Play();
        }
    } 

    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Player") 
        {
            if(!_playerControler.CurrentStack.IsEmpty()) _interactTip.EnableTip();
            _canInteract = true;
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.tag == "Player") 
        {
            _interactTip.DisableTip();
            _canInteract = false;
        }
    }
}
