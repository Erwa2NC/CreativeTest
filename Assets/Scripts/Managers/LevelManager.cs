using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    [Header("Environment")]
    [SerializeField] private Vector2 groundLimit;

    public Vector2 GroundLimit => groundLimit;
}
