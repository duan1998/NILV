using UnityEngine;
using Zero.Dialogue;
using Zero.Utils;

public class NPC : MonoBehaviour, ITalkable
{
    private Transform _bubble; // 气泡
    private Talk _talk; // 对话

    private void Awake()
    {
        // 获得气泡
        _bubble = transform.Find("Bubble");

        // 获得对话
        _talk = GetComponent<Talk>();

        // 默认不激活气泡
        ToggleBubble(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerCtrl>().AddNPC(this);
            ToggleBubble(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerCtrl>().RemoveNPC(this);
            ToggleBubble(false);
        }
    }

    /// <summary>
    /// 显示/隐藏气泡
    /// </summary>
    /// <param name="isShow">true:显示 false:隐藏</param>
    public void ToggleBubble(bool isShow)
    {
        if (_bubble != null) _bubble.gameObject.SetActive(isShow);
    }

    public Talk GetTalk() => _talk;
}