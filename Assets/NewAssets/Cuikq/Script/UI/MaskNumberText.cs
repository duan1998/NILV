using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MaskNumberText : LevelText
{
    void Update()
    {
        int maskNum = Zero.Inventory.InventoryManager.Instance.GetItemCount(Zero.Inventory.ItemKey.Mask);
        showText.text = "MASK" + Environment.NewLine +"   ×" + maskNum.ToString();
    }
}
