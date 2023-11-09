using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
[DisallowMultipleComponent]
public class FloatingJoystick : MonoBehaviour
{
    [HideInInspector]
    public RectTransform rectTransform;
    public RectTransform knob;

    public void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }
}
