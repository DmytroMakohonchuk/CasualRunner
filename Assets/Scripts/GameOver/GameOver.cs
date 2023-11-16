using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class GameOver : MonoBehaviour
{
    [Inject] private PauseMenu _pauseMenu;
    [Inject] private TileSpawner _tileSpawner;
    [Inject] private CharacterMovement _characterMovement;

    [SerializeField] private GameObject _player;
    [SerializeField] GameObject _gameOverPanel;
    [SerializeField] Button restartButton;

    // Start is called before the first frame update
    void Start()
    {
        _gameOverPanel.SetActive(false);
        restartButton.onClick.AddListener(RestartGame);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("Obstacle"))
        {
            _pauseMenu.PauseGame();
            _gameOverPanel.SetActive(true);
        }
    }


    private void RestartGame()
    {
        _gameOverPanel.SetActive(false);
        _characterMovement.ResetPlayerPosition();
        _tileSpawner.RestartTiles();
        _pauseMenu.ResumeGame();
    }

}
