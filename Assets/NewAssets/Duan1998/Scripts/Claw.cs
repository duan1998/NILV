using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Claw : MonoBehaviour
{
    private Transform m_handle;
    private Transform m_rope;
    private LineRenderer m_linerenderer;

    [SerializeField]
    private float m_upwardMoveSpeed = 50;



    private Transform m_targetTrans;

    private Transform m_upMotionStopPoint;

    private Vector3 m_offset;

    public Vector3 _HandlePosition => m_handle.position;

    private PlatformCtrl m_hookPlatform;

    private bool m_compete;
    private bool m_putDown;
    private bool m_isUsed;

    public UnityAction _OnCompete
    {
        get;
        set;
    }
    public bool _CouldUse => !m_isUsed;

    private void Awake()
    {
        m_handle = transform.Find("handle");
        m_rope = transform.Find("rope");
        m_upMotionStopPoint = transform.Find("upMotionStopPoint");
        m_linerenderer = m_rope.GetComponent<LineRenderer>();
        if(transform.Find("Platform")!=null)
            m_hookPlatform = transform.Find("Platform").GetComponent<PlatformCtrl>();
    }

    private void Start()
    {
        m_linerenderer.SetPosition(1, new Vector3(0, m_handle.localPosition.y- m_rope.localPosition.y, 0));
        m_compete = true;
        m_putDown = false;
        m_isUsed = false;

    }

    private void Update()
    {
        if (m_compete != true)
        {
            if (!m_putDown && Vector3.Distance(m_upMotionStopPoint.position, m_handle.position) > 0.002f && m_handle.position.y <= m_upMotionStopPoint.position.y)
            {
                m_handle.position += m_upwardMoveSpeed * Time.deltaTime * Vector3.up;
                m_linerenderer.SetPosition(1, m_handle.position - m_rope.position);
                m_targetTrans.position = m_handle.position + m_offset;
            }
            else if (!m_putDown)
            {
                m_putDown = true;
                m_isUsed = true;
                _OnCompete();
            }
            else if (m_putDown && Vector3.Distance(m_rope.position, m_handle.position) > 0.002f && m_handle.position.y <= m_rope.position.y)
            {
                m_handle.position += m_upwardMoveSpeed * Time.deltaTime * Vector3.up;
                m_linerenderer.SetPosition(1, m_handle.position - m_rope.position);
                m_compete = true;
            }

        }
    }

    public void UpwardMotion(Transform targetTrans)
    {
        m_targetTrans = targetTrans;
        if (m_targetTrans.CompareTag("Player"))
            m_offset = m_targetTrans.position - m_handle.position;
        else if (m_targetTrans.CompareTag("Goods"))
            m_offset = Vector3.zero;
        if(m_hookPlatform!=null)
            m_hookPlatform.DownFall();
        m_compete = false;
    }


}
