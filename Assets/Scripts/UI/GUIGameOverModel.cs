using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIGameOverModel
{
    public event Action OnRestartButtonPressed;

    public void OnRestartPressedHandler()
    {
        OnRestartButtonPressed?.Invoke();
    }
}
