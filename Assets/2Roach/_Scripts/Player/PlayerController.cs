using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, IPlayerController
{

    [Header("Components")]
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private GameObject _model;
    [SerializeField] private GameObject _hat;
    [SerializeField] private InputReader _input;
    public Vector3 MoveInput { get => _inputVec;  }
    public GameObject Model { get => _model; }
    public Rigidbody Rb { get => _rb;  }
    

    [Header("Stats Base")]
    [SerializeField]private float _baseTurnSpeed = 360;
    [SerializeField] private Transform _holdingTransform;

    private float _currentSpeed;

    private Stack _currentStack;

    public Stack CurrentStack 
    { 
        get => _currentStack;
        set => _currentStack = value;
            
    }

    
    private bool _isMoving = false;
    private bool _isCarrying = false;
    private bool _isInteracting = false;
    public bool IsMoving { get => _isMoving;  }
    public bool IsCarrying { get => _isCarrying;  }
    public bool IsInteracting { get => _isInteracting;  }
    private Vector3 _inputVec;


    public static PlayerController Instance { get; private set; }

    private void Awake() 
    { 
        // If there is an instance, and it's not me, delete myself.
        if (Instance != null && Instance != this) 
        { 
            Destroy(this); 
        } 
        else 
        { 
            Instance = this; 
        } 
    }

    private void Start()
    {
        _currentStack = new Stack();
        _currentStack.StackHolder = _holdingTransform;
    }

    private void OnEnable()
    {
        _input.MoveEvent += GatherMovInput;
        _input.InteractEvent += GatherInteractInput;
    }

    private void OnDisable()
    {
        _input.MoveEvent -= GatherMovInput;
        _input.InteractEvent -= GatherInteractInput;
    }


    private void FixedUpdate() 
    {
        _isCarrying = _currentStack?.StackedIngredients.Count > 0 ? true : false;  
        Move();
    }
    private void LateUpdate() 
    {
        Look();
    }

    private void GatherMovInput(Vector2 newInput) 
    {
        _inputVec = new Vector3(newInput.x, 0, newInput.y);
    }
    private void GatherInteractInput(bool newInput) 
    {
        _isInteracting = newInput;
    }


    private void Look() 
    {
        if (_inputVec != Vector3.zero) 
        {
            _isMoving = true;
            var lookRot = Quaternion.LookRotation(_inputVec.ToIso(), Vector3.up);
            _model.transform.rotation = Quaternion.RotateTowards(_model.transform.rotation, lookRot, _baseTurnSpeed * Time.deltaTime);
        }
        else
        {
            _isMoving =false;
        }
    }

    private void Move() 
    {
        _rb.MovePosition(transform.position + _model.transform.forward * _inputVec.normalized.magnitude * _currentStack.StackSpeedMultiplier * Time.deltaTime);
    }

    /// <summary>
    /// Sends the player to the opposite area
    /// </summary>
    public void Switch(RoomState destination)
    {
        if(destination == RoomState.Kitchen)
        {
            transform.position = new Vector3(0,transform.position.y + 100, 0);
            _hat.SetActive(true);
        }

        if(destination == RoomState.Saloon)
        {
            transform.position = new Vector3(0, transform.position.y - 100, 0);
            _hat.SetActive(false);
        }
       
    }
}

public static class Helpers 
{
    private static Matrix4x4 _isoMatrix = Matrix4x4.Rotate(Quaternion.Euler(0, 45, 0));
    public static Vector3 ToIso(this Vector3 input) => _isoMatrix.MultiplyPoint3x4(input);
    public static Vector3 FromIso(Vector3 isoPos) => _isoMatrix.inverse.MultiplyPoint3x4(isoPos);    
}