using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RoachState
{
     Dormant,
    Choosing,
    WaitingToOrder, 
    WaitingForFood, 
    IsEating, 
}

public class Roach : MonoBehaviour
{
    [ContextMenu("GetModel")]
    private void GetModel() => _model = transform.GetChild(0).gameObject;

    [SerializeField]private RoachID _id;
    [SerializeField]private GameObject _model;
    [SerializeField]private bool _isPlaying;
    
    [Header("Shared Data:")]
    [SerializeField] private float _waitingToOrder_Time;
    [SerializeField] private float _waitingForFood_Time;
    [SerializeField] private float _eating_Time;

    [Header("Debug:")]
    [SerializeField]private RoachState _state;
    [SerializeField]private Order _currentOrder;

    private bool _hasOrdered = false;
    private bool _hasReceivedFood = false;

    public RoachID Id { get => _id; }
    public RoachState State 
    { 
        get => _state; 
    private set 
        {
            _state = value;
            Debug.Log(_id + " - State: " + value);
        }
    }
    public Order CurrentOrder { get => _currentOrder;  }
    public bool IsPlaying { get => _isPlaying; }

   
    private void Start() => _model.gameObject.SetActive(false);

    public void InitOrder(Order order)
    {
        _isPlaying= true;
        _model.gameObject.SetActive(true);
        _hasOrdered = false;
        _hasReceivedFood = false;

        _currentOrder = order;
        StartCoroutine(COR_Order());
        Debug.Log(_id + " will order " + order.name);
    }


    public void TakeOrder()
    {
        if(_hasOrdered ==false)
        {
            _hasOrdered = true;
        //UI pop-up with wanted FOOD
        }
        else
        {
            Debug.Log("already ordered");
        }
    }

    private IEnumerator COR_Order()
    {
        State = RoachState.Choosing;
    
        yield return Yielders.Get(_currentOrder.Delay);

        State = RoachState.WaitingToOrder;
        // UI.PopUp order is available
        yield return StartCoroutine(WaitForTimeOrCondition(_waitingToOrder_Time, _hasOrdered));
        if(!_hasOrdered)
        {
            FailOrder();
            yield break;
        } 
   
        State = RoachState.WaitingForFood;
        // UI.PopUp waiting for food.
        yield return StartCoroutine(WaitForTimeOrCondition(_waitingForFood_Time, _hasReceivedFood));
        if(!_hasReceivedFood)
        {
            FailOrder();
            yield break;
        }

        State = RoachState.IsEating;
        yield return Yielders.Get(_currentOrder.EatTime);

        CompletedOrder();
    }


    public void ReceiveStackFood(Stack foodStack)
    {
        if(State != RoachState.WaitingForFood)
        {
            Debug.Log("Not Waiting for food.");
            return;
        }

        if(foodStack.IsEmpty())
        {
            Debug.Log("No food here");
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
            Debug.Log("Stack is PERFECT");
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
        StopCoroutine(COR_Order());
        State = RoachState.Dormant;
        _isPlaying = false;
        _model.gameObject.SetActive(false);
    }

    private void CompletedOrder()
    {
        StopCoroutine(COR_Order());;
        State = RoachState.Dormant;
        _isPlaying = false;
        _model.gameObject.SetActive(false);
    }

}
