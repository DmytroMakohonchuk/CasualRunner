using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITileMovementController 
{
    float Speed { get; }
    Vector3 MovementVector { get; set; }
    void MoveTiles();
    void UpdateTilePosition();
    void CheckTileEndPoint();
    void UpdateOriginalTilePositions();
}
