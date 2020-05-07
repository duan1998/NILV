using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zero.Dialogue;

public partial class PlayerController : MonoBehaviour
{
    private List<NPC> _npcList = new List<NPC>(); // 可交互NPC列表
    private List<PickupItem> _pickupItems = new List<PickupItem>(); // 可拾取道具列表

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            if (!UIManager.Instance.HasDialogueMessages())
            {
                // 没有对话消息...

                // 开始初始化对话消息
                Talk talk;
                if (_pickupItems.Count > 0)
                {
                    // 先拾取道具
                    var pickupItem = _pickupItems[0];
                    talk = pickupItem.GetTalk();
                    pickupItem.Pickup();
                    _pickupItems.Remove(pickupItem); // 拾取后移除
                }
                else if (_npcList.Count > 0)
                {
                    // 跟npc对话
                    var npc = _npcList[0];
                    talk = npc.GetTalk();
                }
                else return;

                // 没有对话消息则返回
                if (talk == null) return;

                // 设置对话消息并显示对话
                UIManager.Instance.DialogueInit(talk.GetDialogueMessages());
                UIManager.Instance.DialogueInput();
            }
            else
            {
                UIManager.Instance.DialogueInput();
            }
        }
    }


    public void AddNPC(NPC npc)
    {
        _npcList.Add(npc);
    }

    public void RemoveNPC(NPC npc)
    {
        _npcList.Remove(npc);
    }

    public void AddPickupItem(PickupItem pickupItem)
    {
        _pickupItems.Add(pickupItem);
    }

    public void RemovePickupItem(PickupItem pickupItem)
    {
        _pickupItems.Remove(pickupItem);
    }
}