using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;
using TMPro;

public class MenuController : MonoBehaviour
{
    public GameObject settingsMenu;

    private bool isPaused = false;

    public TMP_Text volumeText;


    void Start()
    {

        settingsMenu.SetActive(false);
        volumeText.text = Mathf.RoundToInt(AudioListener.volume * 100).ToString();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        settingsMenu.SetActive(false);
        isPaused = false;
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void Reset()
    {
        Resume();
        SceneManager.LoadScene("FirstLevel");
    }

    public void Pause()
    {
        settingsMenu.SetActive(true);
        isPaused = true;
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void Exit()
    {
    #if UNITY_EDITOR
         UnityEditor.EditorApplication.isPlaying = false;
    #else
        UnityEngine.Application.Quit();
    #endif

    }

    public void IncrementAudio()
    {
        AudioListener.volume = Math.Min(1f, AudioListener.volume + 0.1f);
        volumeText.text = Mathf.RoundToInt(AudioListener.volume * 100).ToString();

    }

    public void DecrementAudio()
    {
        AudioListener.volume = Math.Max(0f, AudioListener.volume - 0.1f);
        volumeText.text = Mathf.RoundToInt(AudioListener.volume * 100).ToString();
    }
}