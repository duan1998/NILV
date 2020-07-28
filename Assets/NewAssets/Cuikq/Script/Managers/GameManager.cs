using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : Singleton<GameManager>
{
    public enum GameState
    {
        PREGAME,
        RUNNING,
        PAUSED
    }

    public GameObject[] SystemPrefabs;

    private List<GameObject> _instancedSystemPrefabs;
    List<AsyncOperation> _loadOperations;

    GameState _currentGameState = GameState.PREGAME;

    private string _currentLevelName = string.Empty;

    public GameState CurrentGameState
    {
        get { return _currentGameState; }
        private set { _currentGameState = value; }
    }
    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        _instancedSystemPrefabs = new List<GameObject>();
        _loadOperations = new List<AsyncOperation>();

        InstantiateSystemPrefabs();
        //用Listener的意义何在？？就不能直接调用吗？
        //UIManager.Instance.OnMainMenuFadeComplete.AddListener(HandleMainMenuFadeComplete);
    }

    private void Update()
    {
        if (GameManager.Instance.CurrentGameState ==
            GameManager.GameState.PREGAME)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameManager.Instance.TogglePause();
        }
    }

    //等到异步切换场景完成后会调用这里
    void OnLoadOperationComplete(AsyncOperation ao)
    {
        if (_loadOperations.Contains(ao))
        {
            _loadOperations.Remove(ao);
            if (_loadOperations.Count == 0)
            {
                UpdateState(GameState.RUNNING);
            }
        }

        Debug.Log("Load Complete.");
    }

    void OnUnloadOperationComplete(AsyncOperation ao)
    {
        //if (_loadOperations.Contains(ao))
        //{
        //    _loadOperations.Remove(ao);
        //}
        Debug.Log("Unload Complete.");
    }

    void InstantiateSystemPrefabs()
    {
        GameObject prefabInstance;
        for (int i = 0; i < SystemPrefabs.Length; i++)
        {
            prefabInstance = Instantiate(SystemPrefabs[i]);
            _instancedSystemPrefabs.Add(prefabInstance);
        }
    }

    void HandleMainMenuFadeComplete(bool fadeOut)
    {
        if (!fadeOut)
        {
            UnloadLevel(_currentLevelName);
        }
    }

    //这一部分是否可以单独放在一个Level Manager类中？
    void UpdateState(GameState state)
    {
        GameState previousGameState = _currentGameState;
        _currentGameState = state;
        switch (_currentGameState)
        {
            case GameState.PREGAME:
                Time.timeScale = 1.0f;
                break;
            case GameState.RUNNING:
                Time.timeScale = 1.0f;
                break;
            case GameState.PAUSED:
                Time.timeScale = 0.0f;
                break;
            default:
                break;
        }
        //OnGameStateChange.Invoke(_currentGameState, previousGameState);
    }

    void LoadLevel(string levelName)
    {
        AsyncOperation ao = SceneManager.LoadSceneAsync(levelName,
            LoadSceneMode.Additive);
        if (ao == null)
        {
            Debug.LogError(
                "[GameManager] Unable to load level" + levelName);
            return;
        }
        ao.completed += OnLoadOperationComplete;
        _loadOperations.Add(ao);
        _currentLevelName = levelName;
    }

    void UnloadLevel(string levelName)
    {
        AsyncOperation ao = SceneManager.UnloadSceneAsync(levelName);
        if (ao == null)
        {
            Debug.LogError(
                "[GameManager] Unable to unload level" + levelName);
            return;
        }

        ao.completed += OnUnloadOperationComplete;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        for (int i = 0; i < _instancedSystemPrefabs.Count; ++i)
        {
            Destroy(_instancedSystemPrefabs[i]);
        }
        _instancedSystemPrefabs.Clear();
    }

    public void StartGame()
    {
        LoadLevel("Main");
    }

    public void TogglePause()
    {
        UpdateState(_currentGameState == GameState.RUNNING ?
            GameState.PAUSED : GameState.RUNNING);
    }

    public void RestartGame()
    {
        UpdateState(GameState.PREGAME);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
