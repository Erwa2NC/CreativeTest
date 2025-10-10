using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class CharacterMover : MonoBehaviour
{
    [SerializeField] 
    private bool isPlayer = false;

    [Header("Reference")]
    [SerializeField] private Transform body;
    [SerializeField] private CharacterBoost characterBoost;

    [Header("Movement Settings")]
    [SerializeField] private Vector3 moveDirection = Vector3.forward; // Direction to move
    [SerializeField] private float minMoveSpeed = 5f; // Speed of movement
    [SerializeField] private float maxMoveSpeed = 10f;
    [SerializeField] private float rotationSpeed = 0.25f; // Speed of rotation
    [SerializeField, Range(0.1f, 90f)] private float maxTurnRotation = 45f; //Rotation Limit
    [SerializeField] private float boostYPos = 0.5f; //Y pos when boost

    [Header("Deceleration Settings")]
    [SerializeField] private AnimationCurve decelerationCurve = AnimationCurve.EaseInOut(0, 1, 1, 0);
    
    private float _moveSpeed;
    private float _originalSpeed;
    private bool _isDecelerating = false;
    private bool _isBoosting = false;
    private float _decelerationStartTime;
    private float _decelerationDuration = 2f;
    private Coroutine _decelerationCoroutine;
    private bool _isSliding = false;
    private Vector2 _lastInputPos;
    private Vector2 _groundLimit;

    public float MoveSpeed => _moveSpeed;
    public bool IsDecelerating => _isDecelerating;
    
    public bool IsPlayer => isPlayer;
    
    private void Awake()
    {
        _moveSpeed = Random.Range(minMoveSpeed, maxMoveSpeed);
        _originalSpeed = _moveSpeed;
        _moveSpeed = 0;
        _groundLimit = LevelManager.Instance.GroundLimit;
    }

    private void OnEnable()
    {
        _moveSpeed = _originalSpeed;
    }

    private void Update()
    {
        if (isPlayer)
        {
            CheckBoost();
        }
        
        RotationCharacter();
        MoveCharacter();
    }
    
    private void MoveCharacter()
    {
        // Calculate the movement vector
        Vector3 movement = (isPlayer) ? body.forward * (_moveSpeed + characterBoost.GetBoostValue()) * Time.deltaTime : moveDirection.normalized * (_moveSpeed + characterBoost.GetBoostValue()) * Time.deltaTime;
        transform.position += movement;

        LimitMovement();
    }

    private void LimitMovement()
    {
        Vector3 playerPos = transform.position;

        playerPos.x = transform.position.x;
        playerPos.x = Mathf.Clamp(playerPos.x, _groundLimit.x, _groundLimit.y);

        transform.position = playerPos;
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
                        body.Rotate(Vector3.up, deltaX * rotationSpeed);
                        _lastInputPos = touch.position;
                    }
                    break;

                case TouchPhase.Ended:
                    _isSliding = false;
                    break;

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
                body.Rotate(Vector3.up, deltaX * rotationSpeed);
                _lastInputPos = mousePos;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                _isSliding = false;
            }
        }

        LimitRotation();
    }

    private void LimitRotation()
    {
        Vector3 playerRota = body.localRotation.eulerAngles;
        playerRota.y = (playerRota.y > 180) ? playerRota.y - 360 : playerRota.y;
        playerRota.y = Mathf.Clamp(playerRota.y, -maxTurnRotation, maxTurnRotation);

        body.localRotation = Quaternion.Euler(playerRota);
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
    
    public void CheckBoost()
    {
        bool previousState = _isBoosting;
        _isBoosting = characterBoost.IsMaxBoost;

        if (_isBoosting != previousState)
        {
            if(previousState) //Boost to Run
            {
                body.DOMoveY(0, 0.5f);

            }

            else //Run to boost
            {
                body.DOMoveY(0.5f, 0.5f);
            }
        }

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
