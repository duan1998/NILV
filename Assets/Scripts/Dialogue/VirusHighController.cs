using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

public class VirusHighController : MonoBehaviour
{
    public float startTime = 0f; // 启动时间
    public float duration = 1f; // 持续时间
    public float interval = 3f; // 出现的间隔

    private Collider2D _collider2D;

    public bool IsActive { get; private set; }

    private void Start()
    {
        _collider2D = GetComponent<Collider2D>();
        _collider2D.enabled = false;
        
        Invoke(nameof(StartLoop), startTime);
    }

    public void StartLoop()
    {
        var sequence = DOTween.Sequence();
        sequence.AppendCallback(() =>
        {
            IsActive = true;
            _collider2D.enabled = true;
        });
        sequence.AppendInterval(duration);
        sequence.AppendCallback(() =>
        {
            IsActive = false;
            _collider2D.enabled = false;
        });
        sequence.AppendInterval(interval);
        sequence.SetLoops(-1, LoopType.Restart);
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerHealth>()?.TakeDamage();
        }
    }
}