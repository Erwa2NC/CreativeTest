using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShakeUI : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] private RectTransform[] targets = new RectTransform[0];
    [SerializeField] private CharacterBoost characterBoost;

    [Header("Shake Settings")]
    [SerializeField] private float maxAmplitude = 0.5f;
    [SerializeField] private float maxFrequency = 25f;
    [SerializeField] private AnimationCurve shakeCurve;

    private Vector2[] _originalAnchoredPositions;
    private float _timeOffset;
    
    private void Awake()
    {
        _originalAnchoredPositions = new Vector2[targets.Length];
        for (int i = 0; i < targets.Length; i++)
        {
            if (targets[i] != null)
                _originalAnchoredPositions[i] = targets[i].anchoredPosition;
        }

        _timeOffset = Random.value * 100f;
    }

    private void Update()
    {
        ApplyShake();
    }

    private void ApplyShake()
    {
        if (targets.Length == 0) return;

        float normalized = Mathf.InverseLerp(0f, 20f, characterBoost.GetBoostValue());
        float curvedValue = shakeCurve.Evaluate(normalized);

        float amplitude = Mathf.Lerp(0f, maxAmplitude, curvedValue);
        float frequency = Mathf.Lerp(0f, maxFrequency, curvedValue);

        if (normalized <= 0.01f)
        {
            for (int i = 0; i < targets.Length; i++)
            {
                if (targets[i] == null) continue;
                targets[i].anchoredPosition = Vector2.Lerp(targets[i].anchoredPosition, _originalAnchoredPositions[i], Time.deltaTime * 5f);
            }
            return;
        }

        for (int i = 0; i < targets.Length; i++)
        {
            if (targets[i] == null) continue;

            float seed = _timeOffset + i * 10f;
            Vector2 randomOffset = new Vector2(
                Mathf.PerlinNoise(Time.time * frequency + seed, 0f) - 0.5f,
                Mathf.PerlinNoise(0f, Time.time * frequency + seed) - 0.5f
            ) * 2f * amplitude;

            targets[i].anchoredPosition = _originalAnchoredPositions[i] + randomOffset;
        }
    }
}