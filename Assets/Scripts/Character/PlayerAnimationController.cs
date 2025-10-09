using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : CharacterAnimationController
{
    [SerializeField] private CharacterBoost characterBoost;

    private const string BOOST_BOOLEAN = "Boost";

    protected override void Start()
    {
        base.Start();

        if (characterBoost == null)
        {
            characterBoost = GetComponent<CharacterBoost>();
        }
    }

    protected override void LateUpdate()
    {
        base.LateUpdate();
    }

    protected override void UpdateAnimation()
    {
        if (animator == null) return;

        animator.SetBool(BOOST_BOOLEAN, characterBoost.GetBoostValue() >= 20);
        animator.SetBool(RUN_BOOLEAN, characterMover.MoveSpeed > runThreshold);
    }

}
