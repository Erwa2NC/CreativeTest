using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] private TriggerChild triggerChild;

    private void Start()
    {
        triggerChild.OnEnter += Trigger_OnEnter;
    }

    private void Trigger_OnEnter(Collider collider)
    {
        Debug.Log("Character cross this gate");

        //if(collider.TryGetComponent(out Capacité))
    }
}
