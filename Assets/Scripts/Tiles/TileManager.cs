using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class TileManager : MonoBehaviour, ITileManager
{
    [Inject] private ITileMovementController tileMovementController;
    [Inject] private ITileSpawner tileSpawner;

    private void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        tileSpawner.StartSpawnTiles();
    }

    private void FixedUpdate()
    {
        tileMovementController.MoveTiles();
    }

}