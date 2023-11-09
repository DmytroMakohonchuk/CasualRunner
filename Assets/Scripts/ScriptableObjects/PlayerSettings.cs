using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSettings", menuName = "CustomObjects/PlayerSettings", order = 1)]
public class PlayerSettings : ScriptableObject
{
    [Header("Settings")]
    public Vector3 joystickSize = new Vector3(300, 300);
    public float speedMultiplier;
    public float smoothRotationSpeed = 30f;
    public float bendAmount = -45f;
    public float jumpPower = 3f;
    public float jumpDuration = 2f;
    public float dashDuration = 2f;
}
