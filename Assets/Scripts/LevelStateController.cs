using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelStateController : MonoBehaviour
{
    public GameObject levelState;
    public GameObject gameOverElement;
    public GameObject wonTextElement;

    // Start is called before the first frame update
    void Start()
    {
        levelState.SetActive(false);
        
    }

    public void ShowGameOverScreen()
    {
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        levelState.SetActive(true);
        gameOverElement.SetActive(true);
        wonTextElement.SetActive(false);
    }

    public void ShowWonScreen()
    {
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        levelState.SetActive(true);
        gameOverElement.SetActive(false);
        wonTextElement.SetActive(true);
    }
}
