using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class TilePauseManager : MonoBehaviour, ITilePauseManager
{
    private bool isControlEnabled;

    private float speed;

    private List<GameObject> spawnedTiles;
    private List<Vector3> pausedTilePositions = new List<Vector3>();

    [Inject] private ITileSpawner tileSpawner;
    [Inject] private ITileMovementController tileMovementController;

    private void Start()
    {
        speed = tileMovementController.Speed;
        Subscribe();
    }

    private void Subscribe()
    {
        GlobalEventManager.OnPause += PauseCheck;
    }

    public void PauseTiles()
    {
        if (isControlEnabled)
        {
            tileMovementController.MovementVector = Vector3.zero;
            StoreTilePositions();
        }
    }

    public void RestartTiles()
    {
        tileSpawner.ConsecutiveTileCount = 0;
        tileSpawner.SpawnOnZAxis = -12.0f;
        foreach (var tile in spawnedTiles)
        {
            Destroy(tile);
        }
        spawnedTiles.Clear();
        tileMovementController.MovementVector = new Vector3(0, 0, -speed);
        tileSpawner.StartSpawnTiles();
    }

    private void PauseCheck(bool isPaused)
    {
        if (isPaused)
        {
            isControlEnabled = false;
            PauseTiles();
        }
        else
        {
            isControlEnabled = true;
            ResumeTiles();
        }
    }

    private void StoreTilePositions()
    {
        pausedTilePositions.Clear();
        foreach (var tile in spawnedTiles)
        {
            pausedTilePositions.Add(tile.transform.position);
        }
    }

    public void ResumeTiles()
    {
        if (isControlEnabled)
        {
            for (int i = 0; i < spawnedTiles.Count; i++)
            {
                spawnedTiles[i].transform.position += Vector3.zero;
            }
        }
    }

    private void FixedUpdate()
    {
        spawnedTiles = tileSpawner.GetSpawnedTiles();
    }
}