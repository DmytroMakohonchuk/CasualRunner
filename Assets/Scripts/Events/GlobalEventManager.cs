using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GlobalEventManager
{
    public static event Action OnColisionGameOver;

    public static event Action OnPlayerBump;

    public static event Action<bool> OnPause;

    public static void GameOver()
    {
        OnColisionGameOver?.Invoke();
    }

    public static void PlayerBump()
    {
        OnPlayerBump?.Invoke();
    }

    public static void Pause(bool isPaused)
    {
        OnPause?.Invoke(isPaused);
    }
}
