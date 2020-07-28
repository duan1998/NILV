using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Zero.Dialogue;
using Zero.Inventory;
using Zero.Utils;

public class PickupItem : MonoBehaviour, ITalkable
{
    [Tooltip("道具数据")]
    public ScriptableItem itemData;
    
    private Talk _talk; // 拾取后对话

    private void Awake()
    {
        // 从当前游戏对象获得对话
        _talk = GetComponent<Talk>();
    }

    public void Pickup()
    {
        InventoryManager.Instance.AddItem(itemData.itemKey);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            other.GetComponent<PlayerCtrl>().AddPickupItem(this);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            other.GetComponent<PlayerCtrl>().RemovePickupItem(this);
    }

    public Talk GetTalk() => _talk;
}