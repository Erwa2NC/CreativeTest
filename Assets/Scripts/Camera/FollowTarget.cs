using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    [SerializeField] private Transform target;
    
    private void Update()
    {
        FollowUpdate();
    }

    private void FollowUpdate()
    {
        transform.position = target.position;
    }
}
