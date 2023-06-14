using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MoviePlayer : MonoBehaviour
{
    public UnityEngine.Video.VideoPlayer videoPlayer;
    private bool isPaused;
    private bool onlyOnce;

    // Start is called before the first frame update
    void Start()
    {
        isPaused = false;
        onlyOnce = false;
        videoPlayer.isLooping = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                isPaused = false;
                videoPlayer.Play();
            }
            else
            {
                isPaused = true;
                videoPlayer.Pause();
            }
        }

        videoPlayer.loopPointReached += VideoPlayer_loopPointReached;
    }

    private void VideoPlayer_loopPointReached(UnityEngine.Video.VideoPlayer source)
    {
        if (!onlyOnce) {
            onlyOnce = !onlyOnce;
            SceneManager.LoadScene(1);
        }
    }
}
