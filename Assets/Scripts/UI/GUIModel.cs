using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIModel 
{
    public event Action OnPauseButtonPressed;
    public event Action OnJumpButtonPressed;
    public event Action OnBendButtonPressed;

    public void OnPausePressedHandler()
    {
        OnPauseButtonPressed?.Invoke();
    }

    public void OnJumpPressedHandler()
    {
        OnJumpButtonPressed?.Invoke();
    }

    public void OnBendPressedHandler()
    {
        OnBendButtonPressed?.Invoke();
    }
}
