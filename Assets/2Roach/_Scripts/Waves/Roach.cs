using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RoachState
{
    Choosing,
    WaitingToOrder, 
    WaitingForFood, 
    IsEating, 
    Completed,
    Failed,
}

public class Roach : MonoBehaviour
{
    [SerializeField]private RoachID _id;
    [SerializeField]private bool _enabled;
    
    [Header("Shared Data:")]
    [SerializeField] private float _waitingToOrder_Time;
    [SerializeField] private float _waitingForFood_Time;
    [SerializeField] private float _eating_Time;

    [Header("Debug:")]
    [SerializeField]private RoachState _state;
    [SerializeField]private Order _currentOrder;

    private bool _hasOrdered;
    private bool _hasReceivedFood;

    public RoachID Id { get => _id; }
    public RoachState State { get => _state; }
    public Order CurrentOrder { get => _currentOrder;  }

    public void InitOrder(Order order)
    {
        _enabled= true;
        _hasOrdered = false;
        _hasReceivedFood = false;

        _currentOrder = order;
        StartCoroutine(COR_Order());
        Debug.Log(_id + "has the order" + order.name);
    }


    public void RequestFood()
    {
        _hasOrdered =true;
    }

    private IEnumerator COR_Order()
    {
        _state = RoachState.Choosing;
        yield return Yielders.Get(_currentOrder.Delay);

        _state = RoachState.WaitingToOrder;
        // UI.PopUp order is available
        yield return StartCoroutine(WaitForTimeOrCondition(_waitingToOrder_Time,_hasOrdered));
        if(!_hasOrdered) FailOrder();
        
        _state = RoachState.WaitingForFood;
        // UI.PopUp waiting for food.
        yield return StartCoroutine(WaitForTimeOrCondition(_waitingForFood_Time,_hasReceivedFood));
        if(!_hasReceivedFood) FailOrder();

        _state = RoachState.IsEating;
        yield return Yielders.Get(_currentOrder.EatTime);

        CompletedOrder();
    }



    public void ReceiveStackFood(Stack foodStack)
    {
        if(_state != RoachState.WaitingForFood)
        {
            Debug.Log("Not Waiting for food.");
            return;
        }

        
        _hasReceivedFood = true;

        var isTheSame = true;
        var containsTheSame = true;

        for (int i = 0; i < foodStack.StackedIngredients.Count; i++)
        {
            if(isTheSame)
                if(foodStack.StackedIngredients[i] != _currentOrder.Ingredients[i])
                {
                    isTheSame = false;
                }
                else
                    continue;
            
            if(!isTheSame)
            {
                if(_currentOrder.Ingredients.Contains(foodStack.StackedIngredients[i]) == false)
                    containsTheSame = false;
            }

            if(containsTheSame == false) break;
        }

        if(isTheSame)
        {
            Debug.Log("Stack if PERFECT");
            //GameManager.Score.Add(MaxPoints)
        }
        else
        {
            if(containsTheSame)
            {
            Debug.Log("Its FOOD I Guess");

                //GameManager.Score.Add(HalfPoints)
            }
            else
            {
            Debug.Log("Not the same Ingredients");

                //GameManager.Score.Add(Almost No Points)
            }
        }
    }

    private IEnumerator WaitForTimeOrCondition(float waitTime, bool condition)
    {
        float elapsedTime = 0f;

        while (!condition && elapsedTime < waitTime)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }


    private void FailOrder()
    {
        _state = RoachState.Failed;
        _enabled = false;
        this.gameObject.SetActive(false);
    }

    private void CompletedOrder()
    {
        _state = RoachState.Completed;
        _enabled = false;
        this.gameObject.SetActive(false);
    }

}
