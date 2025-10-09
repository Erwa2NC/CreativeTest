using UnityEngine;

public class PlayerAnimationController : CharacterAnimationController
{
    [SerializeField] private CharacterBoost characterBoost;

    [Header("Shake Settings")]
    [SerializeField] private Transform body;
    [SerializeField] private float shakeMagnitude = 0.5f;
    [SerializeField] private float shakeAmplitude = 1f;
    [SerializeField] private Vector3 _originalPosition = Vector3.zero;
    private bool _isShaking = false;

    private const string BOOST_BOOLEAN = "Boost";

    protected override void Start()
    {
        base.Start();

        if (characterBoost == null)
        {
            characterBoost = GetComponent<CharacterBoost>();
        }
    }

    protected void Update()
    {
        ApplyShake();
    }

    private void ApplyShake()
    {
        if (characterBoost.IsMaxBoost && !_isShaking) //Start Shaking
        {
            _isShaking = true;
        }

        if (characterBoost.IsMaxBoost && _isShaking) //Shaking
        {
            Vector3 randomOffset = new Vector3(
                Random.Range(-shakeAmplitude, shakeAmplitude) * shakeMagnitude,
                Random.Range(-shakeAmplitude, shakeAmplitude) * shakeMagnitude,
                0f
                );

            body.localPosition = _originalPosition + randomOffset; 
        }

        else if (!characterBoost.IsMaxBoost && _isShaking) //Stop Shaking
        {
            _isShaking = false;
            body.localPosition = Vector3.Lerp(body.localPosition, _originalPosition, Time.deltaTime * 0.5f);
        }
    }

    protected override void LateUpdate()
    {
        base.LateUpdate();
    }

    protected override void UpdateAnimation()
    {
        if (animator == null) return;

        animator.SetBool(BOOST_BOOLEAN, characterBoost.IsMaxBoost);
        animator.SetBool(RUN_BOOLEAN, characterMover.MoveSpeed > runThreshold);
    }
}