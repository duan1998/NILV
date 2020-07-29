using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGCtrl : MonoBehaviour
{
    public Transform cameraTrans;
    private Vector3 fcLastFramePos;
    private float fcOriginY;
    // Start is called before the first frame update
    private void Start()
    {
        fcOriginY = cameraTrans.position.y;
        fcLastFramePos = cameraTrans.position;
    }
    Vector3 offset=Vector3.zero;
    Vector3 targetPos=Vector3.zero;
    // Update is called once per frame
    private void FixedUpdate()
    {
        //offset.x = cameraTrans.position.x - fcLastFramePos.x;
        //offset.y = cameraTrans.position.y - fcLastFramePos.y;
        //// Debug.Log(offset);
        //transform.position += offset;
        //fcLastFramePos = cameraTrans.position;
        targetPos.x = cameraTrans.position.x;
        targetPos.y = cameraTrans.position.y;
        transform.position = targetPos;
    }
}
