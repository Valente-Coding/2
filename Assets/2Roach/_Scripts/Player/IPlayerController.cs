using UnityEngine;

public interface IPlayerController 
{
    public Vector3 MoveInput { get; }
    public GameObject Model { get; }
    public Rigidbody Rb { get; }
    public Stack CurrentStack { get; }
    public bool IsMoving { get; }
    public bool IsCarrying { get; }
    public void Switch(RoomState destination);

}