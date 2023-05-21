using System.Collections.Generic;
using UnityEngine;

public class Stack
{
    private const float BASE_SPEED = 10f;
    private const float MIN_SPEED = 6f;
    private List<Ingredient> _stackedIngredients = new List<Ingredient>();

    private Transform _stackHolder;

    private float _stackSpeedMultiplier = BASE_SPEED;

    public List<Ingredient> StackedIngredients { get => _stackedIngredients; }
    public Transform StackHolder { get => _stackHolder; set => _stackHolder = value; }
    public float StackSpeedMultiplier { get => _stackSpeedMultiplier; }

    public void ResetStack() {
        _stackedIngredients.Clear();
        
        foreach (Transform ingredientGO in _stackHolder) {
            Object.Destroy(ingredientGO.gameObject);
        }

        UpdateSpeed();
    }

    public bool IsEmpty() {
        return _stackedIngredients.Count == 0;
    }

    public bool CombineStacks(Stack stackToCombine) {
        foreach (Ingredient ing in stackToCombine.StackedIngredients) {
            AddIngredientToCurrentStack(ing);
        }

        return true;
    }

    public void AddIngredientToCurrentStack(Ingredient newIngredient) {
        if (newIngredient == null) return;
        
        _stackedIngredients.Add(newIngredient);


        GameObject newIngredientGO = GameObject.Instantiate(newIngredient.PrefModel, _stackHolder);
        
        if (_stackHolder.childCount > 1) {
            Transform lastChild = _stackHolder.GetChild(_stackHolder.childCount-2);
            newIngredientGO.transform.localPosition = new Vector3(0, lastChild.localPosition.y + .15f, 0);
            //newIngredientGO.transform.localPosition = new Vector3(0, lastChild.localPosition.y + lastChild.localScale.y + newIngredientGO.transform.localScale.y, 0);
        } else {
            newIngredientGO.transform.localPosition = new Vector3(0, 0, 0);
        }

        UpdateSpeed();
    }

    private void UpdateSpeed()
    {
        if( _stackedIngredients != null && !IsEmpty())
        {
            foreach (var ing in _stackedIngredients)
            {
                _stackSpeedMultiplier *= ing.CarrySpeedMultiplier;
            }   
        }
        else
        {
            _stackSpeedMultiplier = BASE_SPEED;
        } 

        if(_stackSpeedMultiplier < MIN_SPEED) _stackSpeedMultiplier = MIN_SPEED;
    }
}
