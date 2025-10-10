using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideAfterDelay : MonoBehaviour
{
    [SerializeField] private float delay = 2f;        // Délai avant désactivation

    private void OnEnable()
    {
        Invoke(nameof(DisableObject), delay);
    }

    private void DisableObject()
    {
        this.gameObject.SetActive(false);
    }
}
