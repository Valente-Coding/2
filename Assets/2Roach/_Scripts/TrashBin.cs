using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashBin : MonoBehaviour
{
    [SerializeField] private InputReader _input;
    [SerializeField] private PlayerController _playerControler;

    private bool _canInteract = false;

    private void Start() {
        _input.InteractEvent += Interact;
    }
    
    private void Interact(bool newInput) {
        if (newInput == true && _canInteract == true && !_playerControler.CurrentStack.IsEmpty()) {
            _playerControler.CurrentStack.ResetStack();
        }
    } 

    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Player") {
            _canInteract = true;
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.tag == "Player") {
            _canInteract = false;
        }
    }
}
