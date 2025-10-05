using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class CharacterMover : MonoBehaviour
{
    [SerializeField] 
    private bool isPlayer = false;
    
    [Header("Movement Settings")]
    [SerializeField] private Vector3 moveDirection = Vector3.forward; // Direction to move
    [SerializeField] private float rotationSpeed = 0.25f; // Speed of rotation
    [SerializeField] private float minMoveSpeed = 5f; // Speed of movement
    [SerializeField] private float maxMoveSpeed = 10f;
    
    [Header("Deceleration Settings")]
    [SerializeField] private AnimationCurve decelerationCurve = AnimationCurve.EaseInOut(0, 1, 1, 0);
    
    private float _moveSpeed;
    private float _originalSpeed;
    private bool _isDecelerating = false;
    private float _decelerationStartTime;
    private float _decelerationDuration = 2f;
    private Coroutine _decelerationCoroutine;
    private bool _isSliding = false;
    private Vector2 _lastInputPos;

    public float MoveSpeed => _moveSpeed;
    public bool IsDecelerating => _isDecelerating;
    
    public bool IsPlayer => isPlayer;
    
    private void Awake()
    {
        _moveSpeed = Random.Range(minMoveSpeed, maxMoveSpeed);
        _originalSpeed = _moveSpeed;
        _moveSpeed = 0;
    }

    private void OnEnable()
    {
        _moveSpeed = _originalSpeed;
    }

    private void Update()
    {
        RotationCharacter();
        MoveCharacter();
    }
    
    private void MoveCharacter()
    {
        // Calculate the movement vector
        Vector3 movement = (isPlayer) ? transform.forward * _moveSpeed * Time.deltaTime : moveDirection.normalized * _moveSpeed * Time.deltaTime;
        transform.position += movement;
    }

    private void RotationCharacter()
    {
        if (!isPlayer)
        {
            return;
            //Or make random movement for bot ?
        }

        //Touch
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    _isSliding = true;
                    _lastInputPos = touch.position;
                    break;

                case TouchPhase.Moved:
                    if (_isSliding)
                    {
                        float deltaX = touch.position.x - _lastInputPos.x;
                        transform.Rotate(Vector3.up, deltaX * rotationSpeed);
                        _lastInputPos = touch.position;
                    }
                    break;

                case TouchPhase.Ended:
                case TouchPhase.Canceled:
                    _isSliding = false;
                    break;
            }
        }

        //Mouse
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                _isSliding = true;
                _lastInputPos = Input.mousePosition;
            }
            else if (Input.GetMouseButton(0) && _isSliding)
            {
                Vector2 mousePos = Input.mousePosition;
                float deltaX = mousePos.x - _lastInputPos.x;
                transform.Rotate(Vector3.up, deltaX * rotationSpeed);
                _lastInputPos = mousePos;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                _isSliding = false;
            }
        }
    }

    // Public methods for speed control
    public void SetMoveSpeed(float newSpeed)
    {
        _moveSpeed = Mathf.Max(0f, newSpeed);
    }
    
    public float GetMoveSpeed()
    {
        return _moveSpeed;
    }
    
    // Deceleration methods
    public void StartDeceleration(float duration = 2f)
    {
        if (_isDecelerating) return;
        
        _decelerationDuration = Mathf.Max(0.1f, duration);
        _originalSpeed = _moveSpeed;
        _decelerationStartTime = Time.time;
        _isDecelerating = true;
        
        if (_decelerationCoroutine != null)
        {
            StopCoroutine(_decelerationCoroutine);
        }
        _decelerationCoroutine = StartCoroutine(DecelerationCoroutine());
    }
    
    public void StopDeceleration()
    {
        if (_decelerationCoroutine != null)
        {
            StopCoroutine(_decelerationCoroutine);
            _decelerationCoroutine = null;
        }
        _isDecelerating = false;
    }
    
    public void ResetSpeed()
    {
        StopDeceleration();
        _moveSpeed = _originalSpeed;
    }
    
    private IEnumerator DecelerationCoroutine()
    {
        float elapsedTime = 0f;
        
        while (elapsedTime < _decelerationDuration)
        {
            elapsedTime = Time.deltaTime - _decelerationStartTime;
            float normalizedTime = elapsedTime / _decelerationDuration;
            
            // Apply deceleration curve
            float speedMultiplier = decelerationCurve.Evaluate(normalizedTime);
            _moveSpeed = _originalSpeed * speedMultiplier;
            
            yield return null;
        }
        
        // Ensure final speed is 0
        _moveSpeed = 0f;
        _isDecelerating = false;
        _decelerationCoroutine = null;
    }
}
