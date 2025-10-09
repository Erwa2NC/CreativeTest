using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimationController : MonoBehaviour
{
    [Header("Animation Settings")]
    [SerializeField] protected Animator animator; // Reference to the Animator component
    [SerializeField] protected float runThreshold = 0.1f; // Speed threshold to trigger run animation
    
    [Header("References")]
    [SerializeField] protected CharacterMover characterMover; // Reference to the CharacterMover component

    protected const string RUN_BOOLEAN = "Run";

    protected virtual void Start()
    {
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
        
        if (characterMover == null)
        {
            characterMover = GetComponent<CharacterMover>();
        }
        
        if (animator != null)
        {
            animator.SetBool(RUN_BOOLEAN, false);
        }
    }

    protected virtual void LateUpdate()
    {
        UpdateAnimation();
    }


    protected virtual void UpdateAnimation()
    {
        if (animator == null) return;
        
        animator.SetBool(RUN_BOOLEAN, characterMover.MoveSpeed > runThreshold);
    }
  
}
