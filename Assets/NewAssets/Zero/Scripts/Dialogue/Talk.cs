using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Zero.Utils;

namespace Zero.Dialogue
{
    /// <summary>
    /// 对话
    /// </summary>
    public abstract class Talk : MonoBehaviour
    {
        /// <summary>
        /// 获得对话消息
        /// </summary>
        /// <returns></returns>
        public abstract List<DialogueMessage> GetDialogueMessages();
    }
}