using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

public class MenuController : MonoBehaviour
{
    public GameObject settingsMenu;
    public Slider volumeSlider;
    public AudioSource audioSource;

    private bool isPaused = false;

    void Start()
    {
        settingsMenu.SetActive(false);
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

    public void ChangeVolume()
    {
        audioSource.volume = volumeSlider.value;
    }
}