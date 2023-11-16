using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    [SerializeField] Button pauseButton;
    [SerializeField] Button resumeButton;

    private bool isPaused;

    public static bool isMovementEnabled = true;

    // Start is called before the first frame update
    void Start()
    {
        pauseMenu.SetActive(false);

        pauseButton.onClick.AddListener(TogglePause);
        resumeButton.onClick.AddListener(TogglePause);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void TogglePause()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            PauseGame();
            pauseButton.gameObject.SetActive(false);
        }
        else
        {
            ResumeGame();
            pauseButton.gameObject.SetActive(true);
        }
    }

    private void PauseGame()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
        isMovementEnabled = false;
    }

    private void ResumeGame()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
        isMovementEnabled = true;
    }
}
