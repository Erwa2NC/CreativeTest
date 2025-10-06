using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class TriggerChild : MonoBehaviour
{
    public event Action<Collider> OnEnter;

    [Header("Settings")]
    [Tooltip("The layers you want to check")]
    [SerializeField] protected LayerMask masksToCheck;

    private void OnTriggerEnter(Collider other)
    {
        OnEnter?.Invoke(other);
    }
}
