using CI.QuickSave;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : Singleton<SaveManager>
{
    //QuickSaveReader reader;
    //QuickSaveWriter writer;
    override protected void Awake()
    {
        base.Awake();
    }
    private void Start()
    {
        //try
        //{
        //    writer = QuickSaveWriter.Create("Data");
        //    if (!QuickSaveRoot.Exists("Data"))
        //    {
        //        writer.Commit();
        //    }
        //    reader = QuickSaveReader.Create("Data");
        //}
        //catch (System.Exception e)
        //{
        //    UIManager.Instance.DebugLog(e.ToString());
        //}
    }
    public void SaveMaskNum(int maskNum)
    {
        //SaveNum("MaskNum", maskNum);
        PlayerPrefs.SetInt("MaskNum", maskNum);
    }

    public int LoadMaskNum()
    {
        if (PlayerPrefs.HasKey("MaskNum"))
            return PlayerPrefs.GetInt("MaskNum");
        else
            return 0;
        //return LoadNum("MaskNum");
    }

    public void SaveDeathCount(int deathCount)
    {
        //SaveNum("DeathCount", deathCount);
        PlayerPrefs.SetInt("DeathCount", deathCount);
    }

    public int LoadDeathCount()
    {
        if (PlayerPrefs.HasKey("DeathCount"))
            return PlayerPrefs.GetInt("DeathCount");
        else
            return 0;
        //return LoadNum("DeathCount");
    }


    //private void SaveNum(string key, int maskNum)
    //{
    //    writer.Write(key, maskNum);
    //    writer.Commit();
    //}

    //private int LoadNum(string key)
    //{
    //    int maskNum;
    //    if (reader.TryRead<int>(key, out maskNum))
    //    {
    //        return maskNum;
    //    }
    //    return 0;
    //}

    public void Clear()
    {
        if (PlayerPrefs.HasKey("DeathCount"))
            PlayerPrefs.SetInt("DeathCount", 0);
        if (PlayerPrefs.HasKey("MaskNum"))
            PlayerPrefs.SetInt("MaskNum",0);
    }
}
