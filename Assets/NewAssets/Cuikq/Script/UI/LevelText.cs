using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelText : MonoBehaviour
{
    protected Text showText;
    // Start is called before the first frame update
    protected virtual void Start()
    {
        showText = GetComponent<Text>();
    }
}
