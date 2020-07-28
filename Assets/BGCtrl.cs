﻿using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGCtrl : MonoBehaviour
{
    public Transform followCameraTrans;
    private Vector3 fcLastFramePos;
    // Start is called before the first frame update
    private void Start()
    {
        fcLastFramePos = followCameraTrans.position;
    }
    Vector3 offset=Vector3.zero;
    // Update is called once per frame
    void Update()
    {
        offset.y = followCameraTrans.position.y - fcLastFramePos.y;
        transform.position += offset;
        fcLastFramePos = followCameraTrans.position;
    }
}