using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Zenject;

public class GameOverEvent : MonoBehaviour
{
    //[SerializeField] private GameObject _gameOverPanel;

    //[SerializeField] private Button _restartButton;

    [Inject] private IPlayerTouchMovement playerTouchMovement;
    [Inject] private ITilePauseManager tilePauseManager;
    [Inject] private ICoinCounter coinCounter;
    [Inject] private GUIProvider guiProvider;
    [Inject] HighScoreReaderWriter highScoreReaderWriter;
    [Inject] CoinsDropEvent coinsDropEvent;

    private void Start()
    {
        GlobalEventManager.OnColisionGameOver += OnPlayerCollision;
        guiProvider.GuiGameOverView.OnShow += OnRestartSubscribe;
        //guiProvider.GuiGameOverView.OnHide += OnRestartUnsubscribe;
    }

    private void OnRestartSubscribe()
    {
        guiProvider.GuiGameOverView.Model.OnRestartButtonPressed += OnRestartButtonClick;
    }

    private void OnRestartUnsubscribe()
    {
        guiProvider.GuiGameOverView.Model.OnRestartButtonPressed -= OnRestartButtonClick;
    }

    private void OnPlayerCollision()
    {
        coinCounter.MinusCoinsCount();

        if (coinCounter.IsLastLife())
        {
            // ѕерев≥р€Їмо, чи к≥льк≥сть монеток гравц€ дор≥внюЇ нулю
            if (coinCounter.GetCurrentCoins() <= 0)
            {
                // √равець програв, виконуЇмо в≥дпов≥дн≥ д≥њ
                StopGame();
                highScoreReaderWriter.UpdateHighScore(coinCounter.GetCurrentCoins());
                guiProvider.GuiView.Hide();
                guiProvider.GuiGameOverView.gameObject.SetActive(true);
                guiProvider.GuiGameOverView.SetActive(true);
            }
        }
    }


    public void OnRestartButtonClick()
    {
        OnRestartUnsubscribe();
        Debug.Log("ButtonPressed!");
        coinsDropEvent.DestroyPreviousCoins();
        guiProvider.GuiView.gameObject.SetActive(true);
        guiProvider.GuiGameOverView.SetActive(false);
        playerTouchMovement.ResetPlayerPosition();
        playerTouchMovement.DisableMovement(false);
        tilePauseManager.RestartTiles();
        coinCounter.ResetCointCount();
        playerTouchMovement.IsGameOver = false; // «м≥нити на false
    }

    private void StopGame()
    {
        tilePauseManager.PauseTiles();
        playerTouchMovement.DisableMovement(true);
    }
}
