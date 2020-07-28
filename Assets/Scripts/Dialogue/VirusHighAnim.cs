using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SkeletonAnimation))]
public class VirusHighAnim : MonoBehaviour
{
    private SkeletonAnimation m_anim;
    private Spine.AnimationState m_animState;

    private VirusHighController mVirusHighController;

    public enum AnimationName
    {
        atvie, //从无到有
        disabled, //无
        disappear, //从有到无
        enabled //有
    }


    private AnimationName m_curExecuteAnimName;


    private void Awake()
    {
        mVirusHighController = this.GetComponentInParent<VirusHighController>();
        m_anim = GetComponent<SkeletonAnimation>();
    }

    private void Start()
    {
        m_animState = m_anim.state;
        m_curExecuteAnimName = AnimationName.disabled;
    }

    private void Update()
    {
        Active();
        Disabled();
        Disappear();
        Enabled();
    }


    bool m_activeCompete;

    void Active()
    {
        if (m_curExecuteAnimName == AnimationName.disabled)
        {
            if (mVirusHighController.IsActive)
            {
                m_activeCompete = false;
                SetAnimation(AnimationName.atvie, false, (go) => m_activeCompete = true);
            }
        }
    }

    void Disabled()
    {
        if (m_curExecuteAnimName == AnimationName.disappear && m_dispearCompete)
        {
            m_dispearCompete = false;
            SetAnimation(AnimationName.disabled, true);
        }
    }

    bool m_dispearCompete;

    void Disappear()
    {
        if (m_curExecuteAnimName == AnimationName.enabled)
        {
            if (!mVirusHighController.IsActive)
            {
                m_dispearCompete = false;
                SetAnimation(AnimationName.disappear, false, (go) => m_dispearCompete = true);
            }
        }
    }

    void Enabled()
    {
        if (m_curExecuteAnimName == AnimationName.atvie && m_activeCompete)
        {
            m_activeCompete = false;
            SetAnimation(AnimationName.enabled, true);
        }
    }


    /// <summary>
    /// 执行动作
    /// </summary>
    /// <param name="name">动作名字</param>
    /// <param name="loop">是否循环</param>
    /// <param name="OnCompete">动作执行一次后所要调用的</param>
    void SetAnimation(AnimationName name, bool loop, Spine.AnimationState.TrackEntryDelegate OnCompete = null)
    {
        if (m_anim.AnimationName == name.ToString()) return;
        ClearBlur();
        m_animState.SetAnimation(0, name.ToString(), loop);
        m_animState.ClearListenerNotifications();
        m_curExecuteAnimName = name;
        if (OnCompete != null)
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