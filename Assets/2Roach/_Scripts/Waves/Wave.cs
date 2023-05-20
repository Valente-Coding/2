using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RoachID
{
    A1,
    A2,
    A3,
    B1,
    B2,
    B3,
    C1,
    C2,
    C3,
}
[CreateAssetMenu(fileName = "Wave", menuName = "Waves/Wave")]
public class Wave : DescriptionBasedSO
{
    [Header("Rest")]
    [SerializeField] float _restDurationBeforeWave = 10f;
    [SerializeField] float _extraDurationWindow = 2f;
    
    [Header("Config")]
    [SerializeField] List<RoachOrder> _orders;


    public float RestDurationBeforeWave { get => _restDurationBeforeWave;  }
    public List<RoachOrder> RoachOrders { get => _orders;  }

    public float Duration 
    { 
        get 
        {
            var minDuration = 0f;
            foreach (var order in _orders)
                minDuration += order.Order.Delay;
            return minDuration + _extraDurationWindow;
        }  
    }


}

    [System.Serializable]
    public class RoachOrder
    {
    [SerializeField] RoachID _roachId;
    [SerializeField] Order _order;

    public Order Order { get => _order;  }
    public RoachID RoachId { get => _roachId;  }
}

