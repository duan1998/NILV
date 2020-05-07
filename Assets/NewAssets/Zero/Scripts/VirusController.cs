using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using DG.Tweening.Plugins.Core.PathCore;
using UnityEngine;

public class VirusController : MonoBehaviour
{
    public float moveSpeed = 1f;
    public Transform virusPath;

    private void Start()
    {
        // 初始位置为路径根节点
        transform.position = virusPath.position;

        var query = from Transform point in virusPath
            select point.position;
        var points = query.ToArray();

        // 在路径点来回移动
        transform.DOPath(points, moveSpeed)
            .SetEase(Ease.Linear)
            .SetSpeedBased()
            .SetLoops(-1, LoopType.Yoyo);
        transform.DOScale(0.8f, 0.5f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            other.GetComponent<PlayerHealth>()?.TakeDamage();
    }
}