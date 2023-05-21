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
    [SerializeField]private RoachID _id;
    [SerializeField]private GameObject _model;
    [SerializeField]private SpriteRenderer _icon;
    [SerializeField]private bool _isPlaying;
    
    [Header("Shared Data:")]
    [SerializeField] private float _waitingToOrder_Time;
    [SerializeField] private float _dishBubbleDuration_Time = 4f;
    [SerializeField] private float _waitingForFood_Time;
    [SerializeField] private float _eating_Time;
    [SerializeField] private Sprite _readyToOrderSprite;
    [SerializeField] private Sprite _readyToEatSprite;
    [SerializeField] private SimpleAudioEvent _deliverFood_CUE;
    [SerializeField] private SimpleAudioEvent _badFood_CUE;
    [SerializeField] private SimpleAudioEvent _mad_CUE;
    [SerializeField] private SimpleAudioEvent _happy_CUE;
    [SerializeField] private SimpleAudioEvent _bubble_CUE;

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
        HideIconBubble();// start false.
        _hasOrdered = false;
        _hasReceivedFood = false;

        _isPlaying= true;
        _model.gameObject.SetActive(true);

        _currentOrder = order;
        StartCoroutine(COR_Order());
        //Debug.Log(_id + " will order " + order.name);
    }

    private void DisplayIconInBubble(Sprite icon)
    {
        _icon.sprite = icon;
        _icon.gameObject.SetActive(true);
        _bubble_CUE?.Play();
    }

    private void HideIconBubble() => _icon.gameObject.SetActive(false);


    public void TakeOrder()
    {
        if(_hasOrdered == false)
        {
            _hasOrdered = true;
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
        DisplayIconInBubble(_readyToOrderSprite);
        yield return StartCoroutine(COR_WaitForTimeOrCondition(_waitingToOrder_Time, _hasOrdered));
        HideIconBubble();
        if(!_hasOrdered)
        {
            FailOrder();
            yield break;
        } 

        // Show Dish
        DisplayIconInBubble(_currentOrder.Ingredients[0].Icon);//TODO show all ingredients not only the first one icon
        yield return Yielders.Get(_dishBubbleDuration_Time);
        HideIconBubble();

        State = RoachState.WaitingForFood;
        // UI.PopUp waiting for food.
        yield return Yielders.Get(2f);//Pop-upDelay
        DisplayIconInBubble(_readyToEatSprite);
        yield return StartCoroutine(COR_WaitForTimeOrCondition(_waitingForFood_Time, _hasReceivedFood));
        HideIconBubble();
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
        if(State != RoachState.WaitingForFood) return;
        if(State == RoachState.IsEating) 
        {
             Debug.Log("Already eating...");
             return;
        }

        if(foodStack.IsEmpty())
        {
            Debug.Log("No food here...");
            return;
        }

        //Debug.Log(foodStack.StackedIngredients[0] + "IS ZERO");
        _deliverFood_CUE?.Play();
        _hasReceivedFood = true;

        var isTheSame = true;
        var containsTheSame = true;

        int foodStackCount = foodStack.StackedIngredients.Count;
        int currentOrderCount = _currentOrder.Ingredients.Count;

        int smallerList = foodStack.StackedIngredients.Count >= _currentOrder.Ingredients.Count ? _currentOrder.Ingredients.Count : foodStack.StackedIngredients.Count;
        for (int i = 0; i < smallerList; i++)
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

        Debug.Log("foodStack LOg");
        foodStack.ResetStack();//Clear Stack

        if(isTheSame && currentOrderCount == foodStackCount)
        {
            Debug.Log("Stack is PERFECT");
            GameManager.instance.Score.Add(20f);//TODO Balance
        }
        else
        {
            if(containsTheSame)
            {
            Debug.Log("Its FOOD I Guess");
            GameManager.instance.Score.Add(10f);//TODO Balance
            }
            else
            {
            Debug.Log("Not the same Ingredients");
            _badFood_CUE?.Play();
            GameManager.instance.Score.Add(1f);//TODO Balance
            }
        }
    }

    private IEnumerator COR_WaitForTimeOrCondition(float waitTime, bool condition)
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
        _mad_CUE?.Play();
        StopCoroutine(COR_Order());
        State = RoachState.Dormant;
        _isPlaying = false;
        _model.gameObject.SetActive(false);
    }

    private void CompletedOrder()
    {
        _happy_CUE?.Play();
        StopCoroutine(COR_Order());;
        State = RoachState.Dormant;
        _isPlaying = false;
        _model.gameObject.SetActive(false);
    }

}
