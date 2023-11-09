using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITileSpawner 
{
    //float GetMaxPos();
    //float GetMinPos();
    float SpawnOnZAxis { get; set; }
    int ConsecutiveTileCount { get; set; }
    void StartSpawnTiles();
    void SpawnTile(int prefabIndex = -1);
    List<Vector3> GetOriginalTilePositions();
    List<GameObject> GetSpawnedTiles();
    void GetTileLength();
}
