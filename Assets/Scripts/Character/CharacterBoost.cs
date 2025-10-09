using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CharacterBoost : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject particles;
    [SerializeField] private Image[] arrowArray = new Image[6];

    [Header("Color Setings")]
    [SerializeField] private Color inactiveColor;
    [SerializeField] private Gradient colorGradient = new Gradient();

    [Header("Debug")]
    [SerializeField, Range(0, 20f)] private float boostValue = 1;

    private void OnValidate()
    {
        UpdateArrow();
    }

    public void SetBoostValue(float value)
    {
        boostValue += value;
        UpdateArrow();
    }

    public float GetBoostValue()
    {
        return boostValue;
    }

    private void UpdateArrow()
    {
        if (arrowArray == null || arrowArray.Length <= 0) return;

        float normalizedValue = Mathf.Clamp01(boostValue / /*arrowArray.Length*/20);

        for (int i = 0; i < arrowArray.Length; i++)
        {
            float arrowPos = (i + 1f) / arrowArray.Length;

            if (arrowPos <= normalizedValue)
            {
                float t = arrowPos / 1f;
                arrowArray[i].color = colorGradient.Evaluate(t);
            }
            else
                arrowArray[i].color = inactiveColor;
        }

        if (boostValue >= 20)
            particles.SetActive(true);
    }
}
