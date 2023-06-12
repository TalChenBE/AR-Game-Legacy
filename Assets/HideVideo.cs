using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class HideVideo : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public GameObject canvas;
    public GameObject btnCanvas;
    public Button skipBtn;
    public bool isPlayerStarted = false;
    public bool isDestroy = false;

    void Start()
    {
        skipBtn.enabled = true;
        skipBtn.onClick.AddListener(skipVideo);
    }

    void skipVideo()
    {
        Destroy(canvas.gameObject);
        Destroy(btnCanvas.gameObject);
        videoPlayer.Stop();
        isDestroy = true;
    }

    void Update()
    {
        if (isPlayerStarted == false && videoPlayer.isPlaying == true)
        {
            isPlayerStarted = true;
        }
        if (isPlayerStarted == true && videoPlayer.isPlaying == false)
        {
            Destroy(btnCanvas.gameObject);
            Destroy(canvas.gameObject);
            isDestroy = true;
        }
    }
}
