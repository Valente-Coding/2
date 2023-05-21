using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientSpawner : MonoBehaviour
{
    [SerializeField] private Ingredient _ingredientToSpawn;
    [SerializeField] private SimpleAudioEvent _onSpawn_CUE;
    [SerializeField] private InputReader _input;
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private SpriteRenderer _icon;


    private bool _canInteract = false;

    private void Start() {
        _input.InteractEvent += InteractWithSpawner;
        //_icon.sprite = _ingredientToSpawn.Icon;
        _icon.gameObject.SetActive(false);
    }
    
    private void InteractWithSpawner(bool newInput) {
        if (newInput == true && _canInteract == true) {
            _playerController.CurrentStack.AddIngredientToCurrentStack(_ingredientToSpawn);
            _onSpawn_CUE?.Play();
        }
    } 

    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Player") {
            _canInteract = true;
            _icon.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.tag == "Player") {
            _canInteract = false;
            _icon.gameObject.SetActive(false);
        }
    }
}
