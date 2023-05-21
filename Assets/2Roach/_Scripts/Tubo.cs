using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tubo : MonoBehaviour
{
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
        if (_stackQueue.Count <= 0 || grabber == null) return;

        grabber.CombineStacks(_stackQueue[0]);

        _stackQueue.RemoveAt(0);
        
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
