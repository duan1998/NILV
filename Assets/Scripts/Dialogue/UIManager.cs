using System;
using System.Collections.Generic;
using UnityEngine;
using Zero.Dialogue;
using Zero.Inventory;
using UnityEngine.UI;
public class UIManager : Singleton<UIManager>
{
    private UI_DialoguePanel _dialoguePanel; // 对话框面板
    private UI_PickupTipPanel _pickupTipPanel; // 拾取提示面板


    public Text m_text;

    protected override void Awake()
    {
        base.Awake();

        if(m_text==null)
        {
            if(transform.Find("Text")==null)
            {

            }
            else
            {
                m_text = transform.Find("Text").GetComponent<Text>();
                m_text.text = "";
            }
        }
        


        _dialoguePanel = transform.Find("DialoguePanel").GetComponent<UI_DialoguePanel>();
        _pickupTipPanel = transform.Find("PickupTipPanel").GetComponent<UI_PickupTipPanel>();
    }


    #region 对话框

    public bool HasDialogueMessages()
    {
        return _dialoguePanel.HasDialogueMessages();
    }

    public void DialogueInit(List<DialogueMessage> dialogueMessages, Action onComplete = null)
    {
        _dialoguePanel.OnTalkCompleteAndClose += onComplete;
        _dialoguePanel.Init(dialogueMessages);
    }

    public void DialogueInput()
    {
        _dialoguePanel.InputHandler();
    }

    #endregion

    #region 拾取提示

    public void ShowTip(Vector3 worldPosition, string text)
    {
        _pickupTipPanel.Show(worldPosition, text);
    }

    public void HideTip()
    {
        _pickupTipPanel.Hide();
    }

    #endregion


    public void DebugLog(string str)
    {
        if(m_text!=null)
            m_text.text += str + "\n";
    }
}