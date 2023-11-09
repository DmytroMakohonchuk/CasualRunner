using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerTouchMovement
{
    event Action<LevelTile> OnGroundChanged;
    bool IsGrounded { get; }
    bool IsGameOver { get; set; }

    Vector3 GetPlayerCurrentPosition();
    void PauseAnimator();
    void ResumeAnimator();
    void ResetPlayerPosition();
    void DisableMovement(bool disable);
    void JumpButtonClicked();
    void BendButtonClicked();
    void DashButtonClicked();
}
