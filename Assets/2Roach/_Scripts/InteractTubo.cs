using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractTubo : MonoBehaviour
{
    [SerializeField] private PlayerController _playerControler;
    [SerializeField] private InputReader _input;
    [SerializeField] private InteractTip _tip;
    [SerializeField] private bool _isReceiver;

    private bool _canInteract = false;

    private void Start() {
        _input.InteractEvent += InteractWithHolder;
    }

    private void InteractWithHolder(bool newInput) {
        if (newInput == true && _canInteract == true && !_playerControler.CurrentStack.IsEmpty() && !_isReceiver) {
            Tubo.instance.PlaceStackInQueue(_playerControler.CurrentStack);
        }
        else if (newInput == true && _canInteract == true && _playerControler.CurrentStack.IsEmpty() && _isReceiver) 
        {
            
            Tubo.instance.PickupFirstStackInQueue(_playerControler.CurrentStack);
        }
    }


    private void OnTriggerEnter(Collider other) 
    {
        if (other.tag == "Player") 
        {
            if(!_isReceiver)
            {
                if(!_playerControler.CurrentStack.IsEmpty())
                {
                    _tip.EnableTip();
                    _canInteract = true;
                    return;
                }
            }
            else
            {
                _canInteract = true;
            }
        }
    }

    private void OnTriggerExit(Collider other) 
    {
        if (other.tag == "Player") 
        {
            _canInteract = false;
            if(!_isReceiver)
                _tip.DisableTip();
        }
    }
}
