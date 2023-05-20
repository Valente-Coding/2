using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientSpawner : MonoBehaviour
{
    [SerializeField] private Ingredient _ingredientToSpawn;
    [SerializeField] private InputReader _input;
    [SerializeField] private PlayerController _playerControler;

    private bool _canPickUp = false;

    private void Start() {
        _input.InteractEvent += InteractWithSpawner;
    }
    
    private void InteractWithSpawner(bool newInput) {
        if (newInput == true && _canPickUp == true) {
            _playerControler.AddIngredientToCurrentStack(_ingredientToSpawn);
        }
    } 

    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Player") {
            _canPickUp = true;
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.tag == "Player") {
            _canPickUp = false;
        }
    }
}
