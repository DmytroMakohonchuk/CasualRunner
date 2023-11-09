using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuUIModel : MonoBehaviour
{
    public event Action OnPlayButtonPressed;

    public void OnPlayPressedHandler()
    {
        OnPlayButtonPressed?.Invoke();
    }
}
