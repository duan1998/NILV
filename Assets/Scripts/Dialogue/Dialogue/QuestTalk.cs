using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Zero.Quest;

namespace Zero.Dialogue
{
    /// <summary>
    /// 任务对话
    /// </summary>
    public class QuestTalk : Talk
    {
        [Header("任务")]
        [Tooltip("任务数据")]
        public ScriptableQuest questData;

        [Header("对话")]
        [Tooltip("接任务前的消息")]
        public List<DialogueMessage> questBeforeMessages;
        [Tooltip("接任务后的消息")]
        public List<DialogueMessage> questAfterMessages;
        [Tooltip("完成任务时的消息")]
        public List<DialogueMessage> questCompleteMessages;
        [Tooltip("完成任务后的消息")]
        public List<DialogueMessage> questCompleteAfterMessages;

        [Header("任务对话事件")]
        public UnityEvent onQuestAccept; // 接受任务时
        public UnityEvent onQuestComplete; // 完成任务时

        private Zero.Quest.Quest _quest;

        protected void Awake()
        {
            _quest = new Zero.Quest.Quest(questData);
        }

        public override List<DialogueMessage> GetDialogueMessages()
        {
            // 选择要显示的消息
            List<DialogueMessage> messages = null;
            if (questData != null)
            {
                if (!_quest.IsAccepted)
                {
                    _quest.Accept();
                    onQuestAccept?.Invoke();

                    // 接受任务前的消息
                    messages = questBeforeMessages;
                }
                else
                {
                    if (_quest.IsCompleted)
                    {
                        // 完成任务后的消息
                        messages = questCompleteAfterMessages;
                    }
                    else
                    {
                        if (_quest.CheckComplete)
                        {
                            _quest.Complete();
                            onQuestComplete?.Invoke();

                            // 完成任务时的消息
                            messages = questCompleteMessages;
                        }
                        else
                        {
                            // 接收任务后的消息
                            messages = questAfterMessages;
                        }
                    }
                }
            }

            if (messages == null) throw new UnityException("任务对话生异常");
            return messages;
        }
    }
}