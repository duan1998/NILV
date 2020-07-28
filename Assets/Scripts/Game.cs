using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Game : Singleton<Game>
{
    public enum PlaceType
    {
        In,
        Out
    }

    private PlaceType m_curPlaceType;
    public PlaceType _CurPlaceType
    {
        get => m_curPlaceType;
        set
        {
            m_curPlaceType = value;
        }
    }
    protected override void Awake()
    {
        base.Awake();
    }

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 100;
        m_curPlaceType = PlaceType.Out;
    }

}

