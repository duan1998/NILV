using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectionNumbers : MonoBehaviour
{
    public Text maskText;
    public Text deathCountText;
    public int currentMaskNum;
    public int currentDeathCount;

    // Start is called before the first frame update
    void Start()
    {
        currentMaskNum = SaveManager.Instance.LoadMaskNum();
        currentDeathCount = SaveManager.Instance.LoadDeathCount();
        ShowMaskNum();
        ShowDeathCount();
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    //测试用
    public void AddMaskNum()
    {
        currentMaskNum++;
        SaveManager.Instance.SaveMaskNum(currentMaskNum);
        ShowMaskNum();
   }

    private void ShowMaskNum()
    {
        maskText.text = "MASK" + Environment.NewLine + "   ×" + currentMaskNum.ToString();
    }

    private void ShowDeathCount()
    {
        deathCountText.text = "DEATH" + Environment.NewLine + "   ×" + currentDeathCount.ToString();
    }
}
