//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class UIManager : Singleton<UIManager>
//{
//    //[SerializeField]
//    //private MainMenu _mainMenu;
//    //[SerializeField]
//    //private PauseMenu _pauseMenu;
//    [SerializeField]
//    private Camera _dummyCamera;

//    public Events.EventFadeComplete OnMainMenuFadeComplete;
//    private void Start()
//    {
//        _mainMenu.OnMainMenuFadeComplete.AddListener(HandleMainMenuFadeComplete);
//        GameManager.Instance.OnGameStateChange.AddListener(
//            HandleGameStateChanged);
//    }

//    private void HandleMainMenuFadeComplete(bool fadeOut)
//    {
//        OnMainMenuFadeComplete.Invoke(fadeOut);
//    }

//    private void HandleGameStateChanged(
//        GameManager.GameState currentState,
//        GameManager.GameState previousState)
//    {
//        bool isActive = currentState == GameManager.GameState.PAUSED;
//        _pauseMenu.gameObject.SetActive(isActive);
//    }

//    private void Update()
//    {
//        if (GameManager.Instance.CurrentGameState !=
//            GameManager.GameState.PREGAME)
//        {
//            return;
//        }

//        if (Input.GetKeyDown(KeyCode.Space))
//        {
//            //_mainMenu.FadeOut();
//            GameManager.Instance.StartGame();
//        }
//    }

//    public void SetDummyCameraActive(bool active)
//    {
//        _dummyCamera.gameObject.SetActive(active);
//    }
//}
