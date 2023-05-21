using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Order_", menuName = "Waves/Order")]
public class Order : DescriptionBasedSO
{
    [SerializeField] List<Ingredient> _ingredients;
    [SerializeField] float _delay;
    [SerializeField] float _eatTime;
    

    public List<Ingredient> Ingredients { get => _ingredients; }
    public float Delay { get => _delay; }
    public float EatTime { get => _eatTime; }
}

