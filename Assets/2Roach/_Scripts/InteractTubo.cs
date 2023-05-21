using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractTubo : MonoBehaviour
{
    [SerializeField] private PlayerController _playerControler;
    [SerializeField] private InputReader _input;
    [SerializeField] private bool _grab;

    private bool _canInteract = false;

    private void Start() {
        _input.InteractEvent += InteractWithHolder;
    }

    private void InteractWithHolder(bool newInput) {
        if (newInput == true && _canInteract == true && !_playerControler.CurrentStack.IsEmpty() && !_grab) {
            Tubo.instance.PlaceStackInQueue(_playerControler.CurrentStack);
        }
        else if (newInput == true && _canInteract == true && _playerControler.CurrentStack.IsEmpty() && _grab) 
        {
            
            Tubo.instance.PickupFirstStackInQueue(_playerControler.CurrentStack);
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
