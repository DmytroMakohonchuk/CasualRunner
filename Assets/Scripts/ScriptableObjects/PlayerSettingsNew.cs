using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPlayerSettings", menuName = "CustomObjects/NewPlayerSettings", order = 1)]
public class PlayerSettingsNew : ScriptableObject
{
    [Header("Jump settings")]
    public float defaultGravityScale = 1.0f;
    public float fallGravityScale = 5f;
    public float jumpForce = 10f;

    [Header("Dash settings")]
    public float dashDuration = 0.5f; // тривалість дешу
    public Vector3 dashDirection = Vector3.down;
    public float dashDistance = 10f; // відстань дешу вниз
}
