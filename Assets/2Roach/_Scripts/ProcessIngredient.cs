using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProcessIngredient : MonoBehaviour
{
    [SerializeField] private PlayerController _playerControler;
    [SerializeField] private InputReader _input;
    [SerializeField] private Ingredient _currentIngredient;
    [SerializeField] private bool _isProcessing = false;

    private bool _canPlace = false;

    public Ingredient IngredientToProcess { get => _currentIngredient; set => OnIngredientChange(value); }

    private void Start() {
        _input.InteractEvent += InteractWithTool;
    }

    private void InteractWithTool(bool newInput) {
        if (newInput == true && _canPlace == true && _isProcessing == false && _currentIngredient == null && _playerControler.CurrentStack.StackedIngredients.Count == 1) {
            IngredientToProcess = _playerControler.CurrentStack.StackedIngredients[0];
            
            _playerControler.ResetCurrentStack();
        }
        else if (newInput == true && _canPlace == true && _playerControler.CurrentStack == null && _currentIngredient != null) {
            _playerControler.AddIngredientToCurrentStack(_currentIngredient);
            _currentIngredient = null;
        }
    }

    private void OnIngredientChange(Ingredient value) {
        _currentIngredient = value;
        if (_currentIngredient?.AfterProcessIngredient != null)
            StartCoroutine(Process());
    }

    private IEnumerator Process() {
        _isProcessing = true;
        yield return new WaitForSeconds(_currentIngredient.TimeToProcess);
        _isProcessing = false;
        if (_currentIngredient != null)
            IngredientToProcess = _currentIngredient.AfterProcessIngredient;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Player") {
            _canPlace = true;
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.tag == "Player") {
            _canPlace = false;
        }
    }
}
