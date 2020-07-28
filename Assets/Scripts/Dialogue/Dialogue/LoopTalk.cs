using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Zero.Dialogue
{
    /// <summary>
    /// 一般重复对话
    /// </summary>
    public class LoopTalk : Talk
    {
        [Header("对话")]
        [Tooltip("对话消息")]
        public List<DialogueMessage> messages;

        public override List<DialogueMessage> GetDialogueMessages()
        {
            return messages;
        }
    }
}