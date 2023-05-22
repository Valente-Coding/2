using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Table : MonoBehaviour
{
      [SerializeField] InputReader _input;
    [SerializeField] private List<Roach> _tableRoaches;
    [SerializeField] private bool _playerInRange =false;

    private IPlayerController _player = null;

    [ContextMenu("Setup My Roaches")]
    // Start is called before the first frame update
    private void Setup()
    {
        _tableRoaches.Clear();
        _tableRoaches.AddRange(GetComponentsInChildren<Roach>());        
    }

    private void OnEnable() => _input.InteractStartEvent += InteractRequest;
    private void OnDisable() => _input.InteractStartEvent -= InteractRequest;

    private void InteractRequest()
    {
        if(_playerInRange)
        {
            Debug.Log("Interacting with table: "+ this.name);
            InteractWithTable(_player.CurrentStack);
        }
    }

    private void InteractWithTable(Stack stack)
    {
        if (!stack.IsEmpty())
            foreach (var roach in _tableRoaches) {
                if (stack.StackedIngredients.Count == roach.CurrentOrder.Ingredients.Count) {
                    int equalIng = 0;
                    for (int i = 0; i < stack.StackedIngredients.Count; i++) {
                        if (stack.StackedIngredients[i].name == roach.CurrentOrder.Ingredients[i].name) {
                            equalIng++;
                        }
                    }

                    if (equalIng == stack.StackedIngredients.Count) {
                        roach.ReceiveStackFood(stack);
                        return;
                    }
                }
            }

        foreach (var roach in _tableRoaches)
        {
            if(roach.IsPlaying)
            {
                roach.TakeOrder();
                
                if (!stack.IsEmpty()) {
                    roach.ReceiveStackFood(stack);
                }
            }
        }       
    }

    /// <summary>
    /// OnTriggerEnter is called when the Collider other enters the trigger.
    /// </summary>
    /// <param name="other">The other Collider involved in this collision.</param>
    private void OnTriggerEnter(Collider other)
    {
        _player = other.GetComponent<IPlayerController>();
        _playerInRange =_player != null;
        
    }

    /// <summary>
    /// OnTriggerEnter is called when the Collider other enters the trigger.
    /// </summary>
    /// <param name="other">The other Collider involved in this collision.</param>
    private void OnTriggerExit(Collider other)
    {
        if(_player != null)
        {
            if(other.GetComponent<IPlayerController>() !=null)
            {
                _playerInRange = false;
                _player = null;
            }
        }
    }
}
