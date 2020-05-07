using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeathCountText : LevelText
{
    protected override void Start()
    {
        base.Start();
        StartCoroutine("UpdateDeathCount", 1f);
    }

    private IEnumerator UpdateDeathCount(float waitTime)
    {
        while (true)
        {
            ShowDeathCount();
            yield return new WaitForSeconds(waitTime);
        }
    }

    void ShowDeathCount()
    {
        int currentDeathCount = SaveManager.Instance.LoadDeathCount();
        showText.text = "DEATH" + Environment.NewLine + "   ×" + currentDeathCount.ToString();
    }
}
