using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformCtrl : MonoBehaviour
{
    private float m_fallSpeed=0;
    private float m_delectionRadius=0.1f;
    private float m_gravitySpeed = -9.81f;

    private Transform m_feet;

    private void Awake()
    {
        m_feet = transform.Find("feet");
        m_isRun = true;
    }
    public void DownFall()
    {
        m_lastFrameFeetPos = m_feet.position;
        StartCoroutine("Fall");
    }
    static Collider2D[] s_colliders=new Collider2D[10];

    private bool m_isRun=false;
    private void OnDrawGizmos()
    {
        if(m_isRun)
        {
            Gizmos.DrawWireSphere(m_feet.position, m_delectionRadius);
           
        }
    }

    Vector3 m_lastFrameFeetPos;

    IEnumerator Fall()
    {
        while (true)
        {
            m_fallSpeed -= m_gravitySpeed*Time.deltaTime;
            transform.position = new Vector3(transform.position.x, transform.position.y - m_fallSpeed * Time.deltaTime, transform.position.z);
            float t = m_feet.position.y - m_lastFrameFeetPos.y;
            int num = Physics2D.OverlapCapsuleNonAlloc(new Vector2(m_lastFrameFeetPos.x, m_lastFrameFeetPos.y + t / 2), new Vector2(m_delectionRadius, t), CapsuleDirection2D.Vertical, 0, s_colliders);
            if (num > 0)
            {
                //计算feet和platform之间的y值偏移
                float dis = Mathf.Abs(transform.position.y - m_feet.position.y);
                bool triggerPlug = false;
                foreach (var go in s_colliders)
                {
                    if (go != null && go.CompareTag("Plug"))
                    {
                        triggerPlug = true;
                        BoxCollider2D box2D = go.GetComponent<BoxCollider2D>();
                        float targetY=dis+go.transform.position.y + box2D.size.y * go.transform.localScale.y - box2D.size.x / 2 * go.transform.localScale.x;
                        transform.position =new Vector3(transform.position.x, targetY,0);
                        break;
                    }   
                }
                if (triggerPlug)
                {
                    break;
                }
            }
            m_lastFrameFeetPos = m_feet.position;
            yield return null;
        }
    }

}
