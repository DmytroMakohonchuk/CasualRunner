using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class TileMovementController : MonoBehaviour, ITileMovementController
{
    //[Inject] private ITilePauseManager tilePauseManager;

    [Inject] private ITileSpawner tileSpawner;

    private List<GameObject> spawnedTiles = new List<GameObject>();

    private List<Vector3> originalTilePositions;

    [SerializeField, Range(0f, 1f)] private float speed = 1f;
    public float Speed => speed;

    private static bool isControlEnabled;

    private Vector3 movementVector;

    private GameObject startTile;

    [SerializeField] private GameObject tilePrefab;

    public Vector3 MovementVector
    {
        get { return movementVector; }
        set => SetMovementVector(value);
    }

    public static bool getIsControlEnabled
    {
        get => isControlEnabled;
        set => isControlEnabled = value;
    }

    private void Start()
    {
    }

    public void MoveTiles()
    {
        isControlEnabled = PlayerTouchMovement.IsControlEnabled;
        if (isControlEnabled)
        {
            CheckTileEndPoint();
            UpdateTilePosition();
        }
        else
        {
            //tilePauseManager.PauseTiles();
            //if (isControlEnabled)
            //{
            //    tilePauseManager.ResumeTiles();
            //}
        }
    }

    public void SetMovementVector(Vector3 vectorValue)
    {
        movementVector = vectorValue;
    }

    public void UpdateTilePosition()
    {
        foreach (var tile in spawnedTiles)
        {
            tile.transform.Translate(movementVector);
        }
    }

    public void CheckTileEndPoint()
    {
        if(spawnedTiles.Count > 0)
        {
            if (spawnedTiles[3].transform.position.z <= 0)
            {
                tileSpawner.SpawnTile();
            }
        }
    }

    public void UpdateOriginalTilePositions()
    {
        originalTilePositions.Clear();
        foreach (var tile in spawnedTiles)
        {
            originalTilePositions.Add(tile.transform.position);
        }
    }

    public void FixedUpdate()
    {
        movementVector = new Vector3(0, 0, -speed);
        originalTilePositions = tileSpawner.GetOriginalTilePositions();
        spawnedTiles = tileSpawner.GetSpawnedTiles();
    }
}