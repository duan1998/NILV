using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class StartVideo : MonoBehaviour
{
    VideoPlayer videoPlayer;
    int currentVideoIndex;

    string[] startGameVideos =
    {
        Application.streamingAssetsPath + "/Videos/StartGame.mp4",
        Application.streamingAssetsPath + "/Videos/StartGameWaitingCycle.mp4",
        Application.streamingAssetsPath + "/Videos/StartGameEnter.mp4"
    };

    bool[] isVideoLooping =
    {
        false,
        true,
        false
    };

    void Start()
    {
        InitializeVedioPlayer();
    }

    void Update()
    {
        DealWithInput();
    }

    void InitializeVedioPlayer()
    {
        // Will attach a VideoPlayer to the main camera.
        GameObject camera = GameObject.Find("Camera");

        // VideoPlayer automatically targets the camera backplane when it is added
        // to a camera object, no need to change videoPlayer.targetCamera.
        videoPlayer = camera.AddComponent<VideoPlayer>();

        // Play on awake defaults to true. Set it to false to avoid the url set
        // below to auto-start playback since we're in Start().
        videoPlayer.playOnAwake = false;

        // By default, VideoPlayers added to a camera will use the far plane.
        // Let's target the near plane instead.
        videoPlayer.renderMode = VideoRenderMode.CameraNearPlane;

        // This will cause our Scene to be visible through the video being played.
        videoPlayer.targetCameraAlpha = 1f;
        videoPlayer.playbackSpeed = 1f;

        currentVideoIndex = 0;
        PlayNextVedio(videoPlayer);
    }


    void PlayNextVedio(VideoPlayer vp)
    {
        if (currentVideoIndex < startGameVideos.Length)
        {
            vp.url = startGameVideos[currentVideoIndex];
            vp.isLooping = isVideoLooping[currentVideoIndex];

            // 注册错误接收事件
            vp.errorReceived += (source, message) =>
            {
                Debug.Log("无法播放视频: " + message);
                PlayNextVedio(vp); // 直接播放下一个视频
            };

            // 增加播放完成回调
            AddCallback(vp);

            // 进行播放并增加视频索引
            vp.Play();
            currentVideoIndex++;
        }
        else
        {
            vp.Stop();
            LevelManager.Instance.StartVideoFinished();
        }
    }

    void AddCallback(VideoPlayer vp)
    {
        vp.loopPointReached -= PlayNextVedio;
        if (!isVideoLooping[currentVideoIndex])
        {
            vp.loopPointReached += PlayNextVedio;
        }
    }

    void DealWithInput()
    {
        var input = Input.inputString;
        if (!string.IsNullOrEmpty(input) || Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("key");
            PlayNextVedio(videoPlayer);
        }
    }
}