using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class PauseEvent : MonoBehaviour
{

    [Inject] private IPlayerTouchMovement playerTouchMovement;
    [Inject] private ITileManager tileManager;

    [Inject] private GUIProvider guiProvider;

    private bool isPauseActive = false;

    void Start()
    {
        //globalUIManager.PauseButton.onClick.AddListener(PausedGame);
        guiProvider.GuiView.Model.OnPauseButtonPressed += PausedGame;
    }

    private void PausedGame()
    {
        isPauseActive = !isPauseActive;
        GlobalEventManager.Pause(isPauseActive);
    }
}
