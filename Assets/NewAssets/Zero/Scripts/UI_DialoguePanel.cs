using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Zero.Dialogue
{
    public class UI_DialoguePanel : MonoBehaviour
    {
        private enum State
        {
            Show,
            Hide
        }

        [Tooltip("文字显示速度")]
        public float textSpeed = 10f;
        [Tooltip("背景淡入淡出时间")]
        public float backgroundFadeTime = 1f;
        [Tooltip("道具淡入淡出时间")]
        public float itemFadeTime = 1f;

        private CanvasGroup _backgroundCanvasGroup;

        private Transform _showItem; // 显示对话道具
        private CanvasGroup _showItemCanvasGroup;
        private Text _message; // 消息主体
        private Text _closeTip; // 关闭提示

        private State _state = State.Hide;


        private bool _isWaiting = false; // 是否等待中

        /// <summary>
        /// 是会否已经完成
        /// </summary>
        public bool IsCompleted { get; private set; }

        private Tween _fadeTween;

        private List<DialogueMessage> _dialogueMessages; // 对话消息列表
        private int _messageIndex = 0; // 消息索引

        public event Action OnTalkComplete; // 对话完成事件
        public event Action OnTalkCompleteAndClose; // 对话完成后关闭事件

        private void Awake()
        {
            _backgroundCanvasGroup = GetComponent<CanvasGroup>();

            _showItem = transform.Find("Content/ShowItem");
            _showItemCanvasGroup = _showItem.GetComponent<CanvasGroup>();
            _message = transform.Find("Content/Message").GetComponent<Text>();
            _closeTip = transform.Find("CloseTip").GetComponent<Text>();

            _backgroundCanvasGroup.alpha = 0f;
            _showItemCanvasGroup.alpha = 0f;
            _closeTip.enabled = false;
        }

        /// <summary>
        /// 是否有对话消息
        /// </summary>
        /// <returns></returns>
        public bool HasDialogueMessages() => _dialogueMessages != null && _dialogueMessages.Count > 0;

        /// <summary>
        /// 初始化对话消息
        /// </summary>
        /// <param name="dialogueMessages"></param>
        public void Init(List<DialogueMessage> dialogueMessages)
        {
            _dialogueMessages = dialogueMessages;

            // 没有消息
            if (_dialogueMessages == null || _dialogueMessages.Count == 0)
            {
                IsCompleted = true;
                OnTalkCompleteAndClose?.Invoke();
                return;
            }

            // 重置消息索引
            _messageIndex = 0;

            // 设置为未完成状态
            IsCompleted = false;

            // 清除淡入淡出tween
            _fadeTween?.Kill();

            // 隐藏关闭提示
            if (_closeTip != null) _closeTip.enabled = false;
        }

        /// <summary>
        /// 输入处理
        /// </summary>
        public void InputHandler()
        {
            // 没有消息
            if (_dialogueMessages == null || _dialogueMessages.Count == 0) return;

            // 等待中
            if (_isWaiting) return;

            // 消息是否未打印完
            if (_messageIndex < _dialogueMessages.Count)
            {
                // 显示消息...
                _isWaiting = true;
                PrintMessage(_dialogueMessages[_messageIndex], () =>
                {
                    _isWaiting = false;
                    _messageIndex++;

                    // 对话已经结束
                    if (_messageIndex >= _dialogueMessages.Count)
                    {
                        // 显示"关闭对话提示"
                        _closeTip.text = "按'L键'关闭对话";
                        _closeTip.enabled = true;

                        OnTalkComplete?.Invoke();
                    }
                    else
                    {
                        // 显示"继续对话提示"
                        _closeTip.text = "按'L键'继续对话";
                        _closeTip.enabled = true;
                    }
                });
            }
            else
            {
                // 隐藏消息...
                _isWaiting = true;
                Hide(() =>
                {
                    _isWaiting = false;
                    _dialogueMessages = null; // 清除当前对话消息
                    OnTalkCompleteAndClose?.Invoke();
                    OnTalkCompleteAndClose = null;
                });
            }
        }

        private void PrintMessage(DialogueMessage dialogueMessage, Action onComplete = null)
        {
            var sequence = DOTween.Sequence();

            if (_state == State.Hide)
            {
                // 第一次显示...

                // 显示对话框对象
                gameObject.SetActive(true);

                // 背景淡入
                sequence.Append(_backgroundCanvasGroup.DOFade(1f, backgroundFadeTime));
            }

            _state = State.Show;
            
            // 隐藏"继续对话提示"
            _closeTip.enabled = false;

            // 清除之前的消息
            _message.text = string.Empty;
            // 没有文字信息则取消激活
            _message.gameObject.SetActive(!string.IsNullOrEmpty(dialogueMessage.message));

            if (!dialogueMessage.isHoldPreviousItem)
            {
                // 不显示道具
                _showItemCanvasGroup.alpha = 0f;

                // 销毁之前的道具
                for (int i = 0; i < _showItem.childCount; i++)
                    Destroy(_showItem.GetChild(i).gameObject);

                if (dialogueMessage.dialogueItem != null)
                {
                    _showItem.gameObject.SetActive(true);
                    Instantiate(dialogueMessage.dialogueItem, _showItem); // 实例化显示的道具
                    sequence.Append(_showItemCanvasGroup.DOFade(1f, itemFadeTime)); // 显示道具淡入
                }
                else
                {
                    _showItem.gameObject.SetActive(false);
                }
            }

            sequence.OnComplete(() =>
            {
                // 开始输出信息
                _message.DOText(dialogueMessage.message, textSpeed).SetSpeedBased().SetEase(Ease.Linear)
                    .OnComplete(() => onComplete?.Invoke());
            });
            _fadeTween = sequence;
        }

        private void Hide(Action onComplete = null)
        {
            _state = State.Hide;

            _fadeTween = _backgroundCanvasGroup.DOFade(0f, backgroundFadeTime) // 背景淡出
                .OnComplete(() =>
                {
                    gameObject.SetActive(false);
                    onComplete?.Invoke();
                });
        }
    }
}