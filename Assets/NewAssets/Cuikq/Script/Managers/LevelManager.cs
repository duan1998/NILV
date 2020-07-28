using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelManager : Singleton<LevelManager>
{
    private string MainSceneName = "Main";
    public LevelList_SO levelList;
    private List<AsyncOperation> _loadOperations;
    private string _currentLevelName = string.Empty;
    public ScreenFade fade;
    public GameObject levelSelectScreen;

    protected override void Awake()
    {
        base.Awake();

        DontDestroyOnLoad(gameObject);
        _loadOperations = new List<AsyncOperation>();
        levelSelectScreen.SetActive(false);
    }

    public void LoadStartLevel()
    {
        if (levelList.levels.Length > 0)
        {
            LoadLevel(levelList.levels[0]);
        }
    }

    public void LoadLevel(string levelName)
    {
        _currentLevelName = levelName;

        levelSelectScreen.SetActive(false);
        // 在Inspector设置，淡出后回调OnScreenFadeOut()
        fade.FadeOut();
        //不适合使用进度条，不用这种方式
        //StartCoroutine(DoLoadLevelEnumerator(levelName));
    }

    //level从1开始计数
    public void LoadLevel(int level)
    {
        if (level < 1 || level > levelList.levels.Length)
        {
            return;
        }

        LoadLevel(levelList.levels[level - 1]);
    }

    public void LoadNextLevel()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        for (int i = 0; i < levelList.levels.Length; i++)
        {
            if (levelList.levels[i] == currentSceneName)
            {
                //如果已经是最后一关，回到Main场景
                if (i == levelList.levels.Length - 1)
                {
                    LoadMainScene();
                    break;
                }

                LoadLevel(levelList.levels[i + 1]);
            }
        }
    }

    // 在Inspector中的Fade中设置，淡出后回调OnScreenFadeOut()
    public void OnScreenFadeOut()
    {
        DoLoadLevel(_currentLevelName);
    }

    void DoLoadLevel(string levelName)
    {
        AsyncOperation ao = SceneManager.LoadSceneAsync(levelName, LoadSceneMode.Single);

        if (ao == null)
        {
            Debug.LogError(
                "[GameManager] Unable to load level" + levelName);
            return;
        }

        ao.completed += OnLoadOperationComplete;
        _loadOperations.Add(ao);
    }


    //void UnloadLevel(string levelName)
    //{
    //    AsyncOperation ao = SceneManager.UnloadSceneAsync(levelName);
    //    if (ao == null)
    //    {
    //        Debug.LogError(
    //            "[GameManager] Unable to unload level" + levelName);
    //        return;
    //    }

    //    ao.completed += OnUnloadOperationComplete;
    //}
    void OnLoadOperationComplete(AsyncOperation ao)
    {
        levelSelectScreen.SetActive(false);
        fade.FadeIn();
        Debug.Log("Load Complete.");
    }

    public void StartVideoFinished()
    {
        levelSelectScreen.SetActive(true);
    }

    public void ReloadScene()
    {
        LoadLevel(SceneManager.GetActiveScene().name);
    }

    public void LoadMainScene()
    {
        LoadLevel(MainSceneName);
    }
    //void OnUnloadOperationComplete(AsyncOperation ao)
    //{
    //    Debug.Log("Unload Complete.");
    //}
}


//不适合使用进度条，不用这种方式
//IEnumerator DoLoadLevelEnumerator(string levelName)
//{
//    loadScreen.SetActive(true);
//    AsyncOperation ao = SceneManager.LoadSceneAsync(levelName);
//    ao.allowSceneActivation = false;

//    while(!ao.isDone)
//    {
//        slider.value = ao.progress;
//        text.text = ao.progress * 100 + "%";
//        if(ao.progress >= 0.9f)
//        {
//            slider.value = 1;
//            text.text = "100%";
//            ao.allowSceneActivation = true;
//            loadScreen.SetActive(false);

//            // 按键触发，在这个项目里不需要，直接载入
//            //text.text = "Press AnyKey to continue";
//            //if(Input.anyKeyDown)
//            //{
//            //    ao.allowSceneActivation = true;
//            //}
//        }
//        yield return null;
//    }
//}