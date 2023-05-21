using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProcessIngredient : MonoBehaviour
{
    [SerializeField] private PlayerController _playerControler;
    [SerializeField] private InputReader _input;
    [SerializeField] private Ingredient _currentIngredient;
    [SerializeField] private bool _isProcessing = false;
    [SerializeField] private SimpleAudioEvent _ding_CUE;
    [SerializeField] private SimpleAudioEvent _place_CUE;
    [SerializeField] private AudioSource _fryingSource;

    private bool _canInteract = false;

    public Ingredient IngredientToProcess { get => _currentIngredient; set => OnIngredientChange(value); }

    private void Start() {
        _input.InteractEvent += InteractWithTool;
    }

    private void InteractWithTool(bool newInput) {
        if (newInput == true && _canInteract == true && _isProcessing == false && _currentIngredient == null && _playerControler.CurrentStack.StackedIngredients.Count == 1) {
            IngredientToProcess = _playerControler.CurrentStack.StackedIngredients[0];
            _place_CUE.Play();
            _playerControler.CurrentStack.ResetStack();
        }
        else if (newInput == true && _canInteract == true && _currentIngredient != null) 
        {
            _fryingSource.mute = true;
            _playerControler.CurrentStack.AddIngredientToCurrentStack(_currentIngredient);
            _currentIngredient = null;
        }
    }

    private void OnIngredientChange(Ingredient value) {
        _currentIngredient = value;
        if (_currentIngredient?.AfterProcessIngredient != null)
            StartCoroutine(Process());
    }

    private IEnumerator Process() 
    {
        _fryingSource.mute = false;

        _isProcessing = true;
        yield return new WaitForSeconds(_currentIngredient.TimeToProcess);
        _isProcessing = false;
        

        if (_currentIngredient != null)
        {
            _fryingSource.mute = true;
            _ding_CUE?.Play();
            IngredientToProcess = _currentIngredient.AfterProcessIngredient;
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
