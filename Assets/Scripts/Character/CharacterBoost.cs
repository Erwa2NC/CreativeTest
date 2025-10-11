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

    public bool IsMaxBoost = false;

    //#region Debug

    //private void OnValidate()
    //{
    //    UpdateArrow();
    //    IsMaxBoost = boostValue >= 20;
    //    ParticlesTrigger(IsMaxBoost);
    //}

    //#endregion

    private void Start()
    {
        UpdateArrow();
    }

    public void SetBoostValue(float value)
    {
        boostValue += value;

        IsMaxBoost = boostValue >= 20;
        ParticlesTrigger(IsMaxBoost);
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
    }

    private void ParticlesTrigger(bool state)
    {
        if(particles == null) return;

        particles.SetActive(state);
    }
}
