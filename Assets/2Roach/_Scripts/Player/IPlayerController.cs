using UnityEngine;

public interface IPlayerController 
{
    public Vector3 Input { get; }
    public GameObject Model { get; }
    public Rigidbody Rb { get; }
    public bool IsMoving { get; }
    public bool IsCarrying { get; }

}