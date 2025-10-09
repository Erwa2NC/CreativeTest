using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Gate : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] private TriggerChild triggerChild;
    [SerializeField] private TextMeshProUGUI gateText;
    [SerializeField] private Animator animator;

    [Header("Settings")]
    [SerializeField, Range(-20, 20)] private int gateValue = 0;
    [SerializeField] private Color positivColor = Color.green;
    [SerializeField] private Color negativColor = Color.red;

    private const string CLOSED_TRIGGER = "Closed";

    private void OnValidate()
    {
        UpdateText();
    }

    private void Start()
    {
        triggerChild.OnEnter += Trigger_OnEnter;
        UpdateText();
    }

    private void Trigger_OnEnter(Collider collider)
    {
        if(collider.TryGetComponent(out CharacterBoost characterBoost))
        {
            characterBoost.SetBoostValue(gateValue);
            animator.SetTrigger(CLOSED_TRIGGER);
        }
    }

    private void UpdateText()
    {
        if (gateValue > 0) 
        {
            gateText.text = "+" + gateValue.ToString();
            gateText.color = positivColor;
        }

        else
        {
            gateText.text = gateValue.ToString();
            gateText.color = negativColor;
        }
            
    }
}
