using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PositionPulse : MonoBehaviour
{
    [SerializeField] private RectTransform rt;

    [Header("Move Settings")]
    [SerializeField] private float position;
    [SerializeField] private float position1;
    [SerializeField] private float duration;
    [SerializeField] private Ease ease = Ease.InCirc;

    // Start is called before the first frame update
    void Start()
    {
        MoveGo();
    }

    private void MoveGo()
    {
        rt.DOLocalMoveX(position, duration).SetEase(ease).OnComplete(MoveBack);
    }

    private void MoveBack()
    {
        rt.DOLocalMoveX(position1, duration).SetEase(ease).OnComplete(MoveGo);
    }
}
