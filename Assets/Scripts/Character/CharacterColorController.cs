using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterColorController : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] private CharacterBoost characterBoost;

    [Header("Color Settings")]
    [SerializeField] private CharacterColorData colorData;
    [SerializeField] private Renderer characterRenderer;
    [SerializeField] private string colorPropertyName = "_Color";
    [SerializeField] private bool useRandomColorOnStart = false;

    [Header("Gradient Settings")]
    [SerializeField] private Gradient colorGradient;
    [SerializeField] private float gradientDuration = 2f;
    [SerializeField] private bool loopGradient = true;

    [Header("Current Color")]
    [SerializeField] private int currentColorIndex = 0;
    
    private MaterialPropertyBlock propertyBlock;
    private int colorPropertyID;
    private Coroutine boostColorSwapCoroutine;
    private bool _isBoosting =  false;

    void Awake()
    {
        propertyBlock = new MaterialPropertyBlock();
        
        colorPropertyID = Shader.PropertyToID(colorPropertyName);
        
        if (characterRenderer == null)
        {
            characterRenderer = GetComponent<Renderer>();
        }
    }
    
    void Start()
    {
        if (colorData != null)
        {
            if (useRandomColorOnStart)
            {
                currentColorIndex = colorData.GetUniqueRandomColorIndex();
            }
            else
            {
                currentColorIndex = colorData.GetDefaultColorIndex();
            }
            ApplyColor(currentColorIndex);
        }
    }

    private void Update()
    {
        if(characterBoost !=  null)
            CheckIfBoost();
    }

    private void CheckIfBoost()
    {
        if(characterBoost.IsMaxBoost && !_isBoosting)
        {
            _isBoosting = true;
            StartBoostColorSwap();
        }
        else if(!characterBoost.IsMaxBoost && _isBoosting)
        {
            _isBoosting = false;
            StopBoostColorSwap();
        }
    }

    public void SetColor(int colorIndex)
    {
        if (colorData == null || characterRenderer == null) return;
        
        if (colorIndex >= 0 && colorIndex < colorData.ColorCount)
        {
            currentColorIndex = colorIndex;
            ApplyColor(colorIndex);
        }
        else
        {
            Debug.LogWarning($"Color index {colorIndex} is out of range. Available colors: {colorData.ColorCount}");
        }
    }
    
    public void SetColor(Color color)
    {
        if (characterRenderer == null) return;
        
        characterRenderer.GetPropertyBlock(propertyBlock, 0);
        propertyBlock.SetColor(colorPropertyID, color);
        characterRenderer.SetPropertyBlock(propertyBlock, 0);
    }
    
    public void NextColor()
    {
        if (colorData == null) return;
        
        int nextIndex = (currentColorIndex + 1) % colorData.ColorCount;
        SetColor(nextIndex);
    }
    
    public void PreviousColor()
    {
        if (colorData == null) return;
        
        int previousIndex = currentColorIndex - 1;
        if (previousIndex < 0)
        {
            previousIndex = colorData.ColorCount - 1;
        }
        SetColor(previousIndex);
    }
    
    
    private void ApplyColor(int colorIndex)
    {
        if (colorData == null || characterRenderer == null) return;
        
        Color color = colorData.GetColor(colorIndex);
        SetColor(color);
    }

    public void StartBoostColorSwap()
    {
        if (boostColorSwapCoroutine != null)
            StopCoroutine(boostColorSwapCoroutine);

        boostColorSwapCoroutine = StartCoroutine(BoostColorSwapCoroutine());
    }

    public void StopBoostColorSwap()
    {
        if (boostColorSwapCoroutine != null)
        {
            StopCoroutine(boostColorSwapCoroutine);
            boostColorSwapCoroutine = null;
        }
    }

    private IEnumerator BoostColorSwapCoroutine()
    {
        float time = 0f;

        while (true)
        {
            float t = Mathf.Clamp01(time / gradientDuration);
            Color gradientColor = colorGradient.Evaluate(t);
            SetColor(gradientColor);

            time += Time.deltaTime;

            if (time >= gradientDuration)
            {
                if (loopGradient)
                    time = 0f;
                else
                    break;
            }

            yield return null;
        }

        boostColorSwapCoroutine = null;
    }

}
