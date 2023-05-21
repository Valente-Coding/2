using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientSpawner : MonoBehaviour
{
    [SerializeField] private Ingredient _ingredientToSpawn;
    [SerializeField] private SimpleAudioEvent _onSpawnCue;
    [SerializeField] private InputReader _input;
    [SerializeField] private PlayerController _playerControler;
    [SerializeField] private SpriteRenderer _icon;

    private bool _canInteract = false;

    private void Start() {
        _input.InteractEvent += InteractWithSpawner;
        _icon.sprite = _ingredientToSpawn.Icon;
        _icon.gameObject.SetActive(false);
    }
    
    private void InteractWithSpawner(bool newInput) {
        if (newInput == true && _canInteract == true) {
            _playerControler.CurrentStack.AddIngredientToCurrentStack(_ingredientToSpawn);
            _onSpawnCue?.Play();
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
