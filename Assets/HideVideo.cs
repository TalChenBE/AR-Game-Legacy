using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class HideVideo : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public GameObject canvas;
    public bool isPlayerStarted = false;
    public bool isDestroy = false;

    void Update()
    {
        if (isPlayerStarted == false && videoPlayer.isPlaying == true)
        {
            // When the player is started, set this information
            isPlayerStarted = true;
        }
        if (isPlayerStarted == true && videoPlayer.isPlaying == false)
        {
            // When the player stopped playing, remove it
            Destroy(canvas.gameObject);
            isDestroy = true;
        }
    }
}
