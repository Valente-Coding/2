using UnityEngine;

public enum IngredientTypes {
    Cheese,
    Tomato,
    RawMeat,
    CookedMeat,
    BurntMeat,
    Lettuce,
}


[CreateAssetMenu(fileName = "IGT_", menuName = "ScriptableObjects/Ingredient")]
public class Ingredient : ScriptableObject
{
    [SerializeField] private IngredientTypes _myType;
    [SerializeField] private GameObject _prefModel;
    [SerializeField] private Sprite _icon;
    [SerializeField] private float _timeToProcess;
    [SerializeField] private Ingredient _afterProcessIngredient;
    [SerializeField] private float _carrySpeedMultiplier = 1f;

    public IngredientTypes MyType { get => _myType; set => _myType = value; }
    public GameObject PrefModel { get => _prefModel; set => _prefModel = value; }
    public float TimeToProcess { get => _timeToProcess; set => _timeToProcess = value; }
    public Ingredient AfterProcessIngredient { get => _afterProcessIngredient; set => _afterProcessIngredient = value; }
    public float CarrySpeedMultiplier { get => _carrySpeedMultiplier; set => _carrySpeedMultiplier = value; }
    public Sprite Icon { get => _icon; }
}
