using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stack  : MonoBehaviour
{
    List<Ingredient> _stackedIngredients = new List<Ingredient>();

    public List<Ingredient> StackedIngredients { get => _stackedIngredients; set => _stackedIngredients = value; }

    public void ResetStack() {
        _stackedIngredients = new List<Ingredient>();
    }
}
