using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    [SerializeField] Button pauseButton;
    [SerializeField] Button resumeButton;

    private bool isPaused;

    // Start is called before the first frame update
    void Start()
    {
        pauseMenu.SetActive(false);
        // Додаємо обробник події натискання кнопки
        pauseButton.onClick.AddListener(TogglePause);
        resumeButton.onClick.AddListener(TogglePause);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Функція для включення або виключення паузи
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
    }

    private void ResumeGame()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }
}
