using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractTip : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _tipIcon;
    [SerializeField] private bool _isAutomatic = true;

    private void Start() {
        _tipIcon.gameObject.SetActive(false);
    }

    public void EnableTip() => _tipIcon.gameObject.SetActive(true);
    public void DisableTip() => _tipIcon.gameObject.SetActive(false);
    private void OnTriggerEnter(Collider other) {
        if(!_isAutomatic) return;
        if (other.tag == "Player") {
            EnableTip();
        }
    }

    private void OnTriggerExit(Collider other) {
         if(!_isAutomatic) return;
        if (other.tag == "Player") {
            DisableTip();
        }
    }
}
