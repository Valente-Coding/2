using UnityEngine;

[CreateAssetMenu(fileName = "PT_", menuName = "ScriptableObjects/ProcessTool")]
public class ProcessTool : ScriptableObject
{
    [SerializeField] int _toolSlots = 1;
}
