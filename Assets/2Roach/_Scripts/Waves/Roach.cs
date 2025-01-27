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
    [SerializeField]private AlignIngredientsIcons _ingIconsList;
    [SerializeField]private bool _isPlaying;
    
    [Header("Shared Data:")]
    [SerializeField] private float _waitingToOrder_Time;
    [SerializeField] private float _dishBubbleDuration_Time = 4f;
    [SerializeField] private float _waitingForFood_Time;
    [SerializeField] private float _eating_Time;
    [SerializeField] private Sprite _readyToOrderSprite;
    [SerializeField] private Sprite _readyToEatSprite;
    [SerializeField] private Sprite _perfectCombinationSprite;
    [SerializeField] private Sprite _notAsExpectedSprite;
    [SerializeField] private Sprite _dontLikeItSprite;
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
    private bool _hasConditionChanged = false;

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

    private IEnumerator COR_DisplayIconInBubble(Sprite icon, float disableDelay = -1)
    {
        _icon.sprite = icon;
        _icon.gameObject.SetActive(true);
        _bubble_CUE?.Play();
        if (disableDelay != -1) {
            yield return new WaitForSeconds(disableDelay);
            HideIconBubble();
        }

        yield return null;
    }

    private void HideIconBubble() => _icon.gameObject.SetActive(false);


    public void TakeOrder()
    {
        if(_hasOrdered == false)
        {
            _hasOrdered = true;
            _hasConditionChanged = true;
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
        StartCoroutine(COR_DisplayIconInBubble(_readyToOrderSprite));
        yield return StartCoroutine(COR_WaitForTimeOrCondition(_waitingToOrder_Time));
        HideIconBubble();
        if(!_hasOrdered)
        {
            FailOrder();
            yield break;
        } 

        // Show Dish
        //DisplayIconInBubble(_currentOrder.Ingredients[0].Icon);//TODO show all ingredients not only the first one icon
        _ingIconsList.DisplayIngredientIcons(_currentOrder.Ingredients);
        yield return Yielders.Get(_dishBubbleDuration_Time);
        //HideIconBubble();
        _ingIconsList.ClearIconsList();

        State = RoachState.WaitingForFood;
        // UI.PopUp waiting for food.
        yield return Yielders.Get(2f);//Pop-upDelay
        StartCoroutine(COR_DisplayIconInBubble(_readyToEatSprite));
        yield return StartCoroutine(COR_WaitForTimeOrCondition(_waitingForFood_Time));
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
        _hasConditionChanged = true;

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
            StartCoroutine(COR_DisplayIconInBubble(_perfectCombinationSprite, 3f));
        }
        else
        {
            if(containsTheSame)
            {
                Debug.Log("Its FOOD I Guess");
                GameManager.instance.Score.Add(10f);//TODO Balance
                StartCoroutine(COR_DisplayIconInBubble(_notAsExpectedSprite, 3f));
            }
            else
            {
                Debug.Log("Not the same Ingredients");
                _badFood_CUE?.Play();
                GameManager.instance.Score.Add(1f);//TODO Balance
                StartCoroutine(COR_DisplayIconInBubble(_dontLikeItSprite, 3f));
            }
        }
    }

    private IEnumerator COR_WaitForTimeOrCondition(float waitTime)
    {
        float elapsedTime = 0f;

        while (!_hasConditionChanged && elapsedTime < waitTime)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        _hasConditionChanged = false;
    }


    private void FailOrder()
    {
        HideIconBubble();
        _mad_CUE?.Play();
        StopCoroutine(COR_Order());
        State = RoachState.Dormant;
        _isPlaying = false;
        _model.gameObject.SetActive(false);
    }

    private void CompletedOrder()
    {
        HideIconBubble();
        _happy_CUE?.Play();
        StopCoroutine(COR_Order());;
        State = RoachState.Dormant;
        _isPlaying = false;
        _model.gameObject.SetActive(false);
    }

}
