using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Zero.Inventory
{
    public class UI_PickupTipPanel : MonoBehaviour
    {
        private enum State
        {
            None,
            Show,
            Hide
        }

        public float fadeTime = 0.5f;

        private CanvasGroup _canvasGroup;

        private Camera _mainCamera;
        private Vector3 _worldPosition;

        private State _state = State.None;

        private Tween _fadeTween;

        private Text m_tipText;

        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            _canvasGroup.alpha = 0f;

            _mainCamera = Camera.main;
            m_tipText = GetComponentInChildren<Text>();

            Hide();
        }

        /// <summary>
        /// 是否能显示提示
        /// </summary>
        /// <returns></returns>
        private static bool IsCanShow() => !UIManager.Instance.HasDialogueMessages();

        public void Show(Vector3 worldPosition, string text)
        {
            gameObject.SetActive(true);

            _worldPosition = worldPosition;
            m_tipText.text = text;

            // 清除淡入淡出Tween
            _fadeTween?.Kill();

            // 正在显示中...先隐藏再显示
            if (_state == State.Show) _canvasGroup.alpha = 0f;

            // 更改为显示状态
            _state = State.Show;

            // 开始淡入
            if (IsCanShow()) _fadeTween = _canvasGroup.DOFade(1f, fadeTime);
        }


        public void Hide()
        {
            if (_state == State.Hide) return;

            _state = State.Hide;

            // 清除淡入淡出Tween
            _fadeTween?.Kill();

            _fadeTween = _canvasGroup.DOFade(0f, fadeTime)
                .OnComplete(() => gameObject.SetActive(false));
        }

        private void Update()
        {
            if (_state == State.Show)
            {
                if (!_fadeTween.IsActive())
                {
                    // 对话中隐藏交互提示
                    _canvasGroup.alpha = IsCanShow() ? 1f : 0f;
                }
            }
        }

        private void LateUpdate()
        {
            if (gameObject.activeSelf)
            {
                // 设置物品位置
                var screenPoint = _mainCamera.WorldToScreenPoint(_worldPosition);
                transform.position = screenPoint;
            }
        }
    }
}