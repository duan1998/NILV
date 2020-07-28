using System;
using UnityEngine;
using UnityEngine.Events;

namespace Zero.Utils
{
    public class Fade : Singleton<Fade>
    {
        private enum State
        {
            None,
            FadeIn, // 淡入
            FadeOut // 淡出
        }

        public float fadeTime = 1f; // 淡出时间

        private State _state = State.None; // 淡入淡出状态
        private Texture2D _blackTexture; // 黑屏纹理(1像素大小)
        private float _alpha; // 纹理的alpha值

        public UnityEvent onFadeInCompleted; // 淡入完成事件
        public UnityEvent onFadeOutCompleted; // 淡出完成事件

        private void Start()
        {
            DontDestroyOnLoad(gameObject);

            // 创建1像素大小的2D纹理
            _blackTexture = new Texture2D(1, 1);
            // 默认alpha值为1
            _alpha = 1f;
            // 设置纹理的像素颜色为黑色
            _blackTexture.SetPixel(0, 0, new Color(0f, 0f, 0f, _alpha));
            // 应用到纹理
            _blackTexture.Apply();
        }

        private void OnGUI()
        {
            GUI.DrawTexture(new Rect(0f, 0f, Screen.width, Screen.height), _blackTexture);
        }

        private void Update()
        {
            // 淡出
            if (_state == State.FadeOut)
            {
                if (_alpha > 0f)
                {
                    _alpha = Mathf.Clamp01(_alpha - fadeTime * Time.deltaTime);
                    _blackTexture.SetPixel(0, 0, new Color(0f, 0f, 0f, _alpha));
                    _blackTexture.Apply();
                }
                else
                {
                    _state = State.None;
                    onFadeOutCompleted?.Invoke();
                }
            }

            // 淡入
            if (_state == State.FadeIn)
            {
                if (_alpha < 1f)
                {
                    _alpha = Mathf.Clamp01(_alpha + fadeTime * Time.deltaTime);
                    _blackTexture.SetPixel(0, 0, new Color(0f, 0f, 0f, _alpha));
                    _blackTexture.Apply();
                }
                else
                {
                    _state = State.None;
                    onFadeInCompleted?.Invoke();
                }
            }
        }

        /// <summary>
        /// 淡入(从透明到纯黑)
        /// </summary>
        /// <param name="isInitial">是否从初始值开始</param>
        public void FadeIn(bool isInitial = false)
        {
            if (isInitial) _alpha = 0f;
            _state = State.FadeIn;
        }

        /// <summary>
        /// 淡出(从纯黑到透明)
        /// </summary>
        /// <param name="isInitial">是否从初始值开始</param>
        public void FadeOut(bool isInitial = false)
        {
            if (isInitial) _alpha = 1f;
            _state = State.FadeOut;
        }
    }
}