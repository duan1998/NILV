using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TipObject : MonoBehaviour
{
    [SerializeField, Tooltip("提示内容")]
    private string m_tipText;
    [Tooltip("拾取提示偏移量")]
    public Vector3 pickupTipOffset = Vector3.zero;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            UIManager.Instance.ShowTip(transform.position + pickupTipOffset, m_tipText);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            UIManager.Instance.HideTip();
        }
    }
}


