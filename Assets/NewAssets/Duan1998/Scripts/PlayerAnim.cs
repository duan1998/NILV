using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

namespace Duan1998
{
    public class PlayerAnim : MonoBehaviour
    {
        private SkeletonAnimation m_anim;
        private Spine.AnimationState m_animState;

        private PlayerCtrl m_playerCtrl;

        public enum AnimationName
        {
            idle,
            run,
            jump,
            touch,
            run_dr,
            _pack_idle,
            _pack_walk,
            _pack_fall,
            _pack_touch
        }

        private AnimationName m_curExecuteAnimName;

        

        private void Awake()
        {
            m_playerCtrl = this.GetComponentInParent<PlayerCtrl>();
            m_anim = GetComponent<SkeletonAnimation>();
            
        }
        

        private void Start()
        {
            m_animState = m_anim.state;
            m_curExecuteAnimName = AnimationName.idle;
            //添加转身事件
            m_playerCtrl._FilpRight = TurnRight;
            m_playerCtrl._FilpLeft = TurnLeft;
        }

        void TurnRight()
        {
            m_anim.skeleton.ScaleX=1f;
        }
        void TurnLeft()
        {
            m_anim.skeleton.ScaleX = -1f;
        }

        private void Update()
        {
            Idle();
            Run();
            Jump();
            Touch();
            PackIdle();
            PackWalk();
            PackFall();

        }

        private bool m_runCompete = false;
        void Run()
        {
            switch (m_curExecuteAnimName)
            {
                case AnimationName.idle:
                    if (m_playerCtrl._HorizontalMove)
                    {
                        //Debug.Log(m_playerCtrl._HorizontalSpeed);
                        m_runCompete = false;
                        SetAnimation(AnimationName.run, true, (go) => { m_runCompete = true; }) ;
                    }
                    break;
                case AnimationName._pack_walk:
                    if (!m_playerCtrl.IsBearGoods)
                    {
                        m_runCompete = false;
                        SetAnimation(AnimationName.run, true, (go) => { m_runCompete = true; });
                    }
                    break;
            }
        }
        void Jump()
        {
            if (!m_playerCtrl._IsGround && !m_playerCtrl._LastFrameIsGround && !m_playerCtrl.IsBearGoods)
            {
                SetAnimation(AnimationName.jump, false);
            }
            else
            {
                switch (m_curExecuteAnimName)
                {
                    case AnimationName.idle:
                        if (!m_playerCtrl._IsGround && m_playerCtrl._LastFrameIsGround)
                            SetAnimation(AnimationName.jump, false);
                        break;
                    case AnimationName.run:
                        if (!m_playerCtrl._IsGround && m_playerCtrl._LastFrameIsGround)
                            SetAnimation(AnimationName.jump, false);
                        break;
                }
            }
        }

        void Idle()
        {
            switch (m_curExecuteAnimName)
            {
                case AnimationName.run:
                    if (!m_playerCtrl._HorizontalMove)
                    {
                        SetAnimation(AnimationName.idle, true);
                    }
                    break;
                case AnimationName.jump:
                    if (m_playerCtrl._IsGround&&!m_playerCtrl._LastFrameIsGround)
                        SetAnimation(AnimationName.idle, true);
                    break;
                case AnimationName._pack_idle:
                    if (!m_playerCtrl.IsBearGoods)
                        SetAnimation(AnimationName.idle, true);
                    break;
                case AnimationName.touch:
                    if (!m_playerCtrl._IsDrag)
                        SetAnimation(AnimationName.idle, true);
                    break;
            }
        }

        void PackIdle()
        {
            switch (m_curExecuteAnimName)
            {
                case AnimationName._pack_fall:
                    if (m_packFallCompete)
                        SetAnimation(AnimationName._pack_idle, true);
                    break;
                case AnimationName._pack_walk:
                    if (!m_playerCtrl._HorizontalMove)
                        SetAnimation(AnimationName._pack_idle, true);
                    break;
                case AnimationName.idle:
                    if (m_playerCtrl.IsBearGoods)
                        SetAnimation(AnimationName._pack_idle, true);
                    break;
                case AnimationName._pack_touch:
                    if (!m_playerCtrl._IsDrag)
                        SetAnimation(AnimationName._pack_idle, true);
                    break;
            }
        }


        bool m_packWalkCompete;
        void PackWalk()
        {
            switch (m_curExecuteAnimName)
            {
                case AnimationName._pack_idle:
                    if (m_playerCtrl._HorizontalMove&& m_playerCtrl._IsGround)
                    {
                        m_packWalkCompete = false;
                        SetAnimation(AnimationName._pack_walk, true,(go)=> { m_packWalkCompete = true; });
                    }
                    break;
                case AnimationName.run:
                    if (m_playerCtrl.IsBearGoods)
                    {
                        m_packWalkCompete = false;
                        SetAnimation(AnimationName._pack_walk, true, (go) => { m_packWalkCompete = true; });
                    }
                    break;
            }

        }

        private bool m_packFallCompete;
        void PackFall()
        {
            if (!m_playerCtrl._LastFrameIsGround && m_playerCtrl._IsGround && m_playerCtrl.IsBearGoods)
            {
                m_packFallCompete = false;
                SetAnimation(AnimationName._pack_fall, false);
                m_animState.Complete += (go) => { m_packFallCompete = true; };
            }
        }

        void Touch()
        {
            if(m_playerCtrl._IsDrag&& !m_playerCtrl.IsBearGoods)
            {
                SetAnimation(AnimationName.touch, false);
            }
        }
        void PackTouch()
        {
            if (m_playerCtrl._IsDrag && !m_playerCtrl.IsBearGoods)
            {
                SetAnimation(AnimationName._pack_touch, false);
            }
        }


        /// <summary>
        /// 执行动作
        /// </summary>
        /// <param name="name">动作名字</param>
        /// <param name="loop">是否循环</param>
        /// <param name="OnCompete">动作执行一次后所要调用的</param>
        void SetAnimation(AnimationName name, bool loop,Spine.AnimationState.TrackEntryDelegate OnCompete=null)
        {
            if (m_anim.AnimationName == name.ToString()) return;
            ClearBlur();
            m_animState.SetAnimation(0, name.ToString(), loop);
            m_animState.ClearListenerNotifications();
            m_curExecuteAnimName = name;
            if(OnCompete!=null)
            {
                m_animState.Complete += OnCompete;
            }

        }
        void ClearBlur()
        {
            m_anim.skeleton.SetToSetupPose();
            m_animState.ClearTracks();
        }


        

    }

}

