using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackHolder : MonoBehaviour
{
    [SerializeField] private Stack _currentStack = new Stack();
    [SerializeField] private PlayerController _playerControler;
    [SerializeField] private InputReader _input;
    [SerializeField] private Transform _stackHolder;

    private bool _canInteract = false;

    private void Start() {
        _currentStack.StackHolder = _stackHolder;
        _input.InteractEvent += InteractWithHolder;
    }

    private void InteractWithHolder(bool newInput) {
        if (newInput == true && _canInteract == true && !_playerControler.CurrentStack.IsEmpty()) {
            _currentStack.CombineStacks(_playerControler.CurrentStack);
            _playerControler.CurrentStack.ResetStack();
        }
        else if (newInput == true && _canInteract == true && _playerControler.CurrentStack.IsEmpty()) {
            _playerControler.CurrentStack.CombineStacks(_currentStack);
            _currentStack.ResetStack();
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
