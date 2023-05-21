using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomIngredientSpawner : MonoBehaviour
{
    [SerializeField] private List<Ingredient> _ingredientsToSpawn;
    [SerializeField] private SimpleAudioEvent _onSpawn_CUE;
    [SerializeField] private InputReader _input;
    [SerializeField] private PlayerController _playerController;


    private bool _canInteract = false;

    private void Start() {
        _input.InteractEvent += InteractWithSpawner;
        //_icon.sprite = _ingredientToSpawn.Icon;
    }
    
    private void InteractWithSpawner(bool newInput) {
        if (newInput == true && _canInteract == true) {
            _playerController.CurrentStack.AddIngredientToCurrentStack(GetRandomIngredient());
            _onSpawn_CUE?.Play();
        }
    } 
    private Ingredient GetRandomIngredient()
    {
        return _ingredientsToSpawn[Random.Range(0,_ingredientsToSpawn.Count)];
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
