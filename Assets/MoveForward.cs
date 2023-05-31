using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveForward : MonoBehaviour
{
    public HideVideo hideVideo;
    public AudioSource audioSource;
    public bool isPlay = false;
    private void Start()
    {
        audioSource.Stop();
    }

    void FixedUpdate()
    {
        if(hideVideo.isDestroy && isPlay == false)
        {
            audioSource.Play();
            isPlay = true;
        }
    }
}
