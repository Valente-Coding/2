using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tubo : MonoBehaviour
{
    [SerializeField] private List<Ingredient> _ingredientsToSpawn;

    public static Tubo instance;

    void Awake()
    {

        if (instance != null && instance != this)
            Destroy(this.gameObject);
        else
        {
            instance = this;
            _stackQueue = new List<Stack>();
        }
    }


    private List<Stack> _stackQueue;

    public void PickupFirstStackInQueue(Stack grabber) {
        if (grabber == null) return;

        if (_stackQueue.Count != 0 )
            grabber.CombineStacks(_stackQueue[0]);
        else 
        {
            Stack newStack = new Stack();
            newStack.StackedIngredients.Add(GetRandomIngredient());
            grabber.CombineStacks(newStack);
        }


        _stackQueue.RemoveAt(0);
    }

    private Ingredient GetRandomIngredient()
    {
        return _ingredientsToSpawn[Random.Range(0,_ingredientsToSpawn.Count -1)];
    }

    public void PlaceStackInQueue(Stack placer) {
        if (placer == null) return;

        Stack newStack = new Stack();
        foreach (Ingredient ing in placer.StackedIngredients) {
            newStack.StackedIngredients.Add(ing);
        }

        _stackQueue.Add(newStack);

        placer.ResetStack();
    }


}
