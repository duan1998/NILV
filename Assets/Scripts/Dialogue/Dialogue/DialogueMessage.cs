using System;
using UnityEngine;

namespace Zero.Dialogue
{
    /// <summary>
    /// 对话框信息
    /// </summary>
    [Serializable]
    public class DialogueMessage
    {
        [Tooltip("是否保持之前的道具")]
        public bool isHoldPreviousItem;
        [Tooltip("对话时显示的道具")]
        public DialogueItem dialogueItem;
        [Tooltip("消息"), TextArea]
        public string message;
    }
}