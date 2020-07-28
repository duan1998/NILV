using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Zero.Inventory;

public class PlayerHealth : MonoBehaviour
{
    private const float ImmuneTime = 3f; // 病毒免疫时间
    private const float ImmuneFadeTime = 0.25f; // 免疫时fade时间

    private float _lastDamageTime = -100f; // 最后受伤的时间

    private MeshRenderer _meshRenderer;

    // 是否已死亡
    public bool IsDeath { get; private set; }

    // 是否免疫中
    public bool IsInImmune => Time.time <= _lastDamageTime + ImmuneTime;

    private void Start()
    {
        _meshRenderer = GetComponentInChildren<MeshRenderer>();

        int maskNumLoaded = SaveManager.Instance.LoadMaskNum();
        InventoryManager.Instance.AddItem(ItemKey.Mask, maskNumLoaded);
    }

    public void TakeDamage()
    {
        // 玩家已经死亡
        if (IsDeath) return;

        // 是否无敌时间内?
        if (IsInImmune) return;

        // 有口罩则消耗口罩
        if (InventoryManager.Instance.HasItem(ItemKey.Mask))
        {
            InventoryManager.Instance.RemoveItem(ItemKey.Mask);

            // 无敌3秒...
            Debug.Log("无敌3秒...");
            _lastDamageTime = Time.time;

            StopAllCoroutines();
            StartCoroutine(nameof(ImmuneFade));
        }
        else
            Death();
    }

    public void TakeDeathDamage()
    {
        // 玩家已经死亡
        if (IsDeath) return;
        Death();
    }

    public void Death()
    {
        // 玩家已经死亡
        if (IsDeath) return;

        Debug.Log("玩家死亡...");

        int deathCount = SaveManager.Instance.LoadDeathCount();
        SaveManager.Instance.SaveDeathCount(deathCount + 1);
        //IsDeath = true;
        LevelManager.Instance.ReloadScene();
    }

    public void Win()
    {
        int maskNum = InventoryManager.Instance.GetItemCount(ItemKey.Mask);
        SaveManager.Instance.SaveMaskNum(maskNum);
    }

    private IEnumerator ImmuneFade()
    {
        while (IsInImmune)
        {
            _meshRenderer.enabled = !_meshRenderer.enabled;
            yield return new WaitForSeconds(ImmuneFadeTime);
        }

        _meshRenderer.enabled = true;
    }
}