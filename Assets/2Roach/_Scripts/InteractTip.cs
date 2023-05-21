using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractTip : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _tipIcon;

    private void Start() {
        _tipIcon.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Player") {
            _tipIcon.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.tag == "Player") {
            _tipIcon.gameObject.SetActive(false);
        }
    }
}
