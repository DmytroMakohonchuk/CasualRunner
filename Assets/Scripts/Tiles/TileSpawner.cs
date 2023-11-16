using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Zenject;

public class TileSpawner : MonoBehaviour, ITileSpawner
{
    public event Action<LevelTile> OnTileChanged;

    [Inject] TileMovementController m_Controller;

    [SerializeField] LevelTile initialTile;

    private LevelTile? groundTile;

    private List<GameObject> spawnedTiles = new List<GameObject>();
    private int _consecutiveTileCount = 0;
    private DiContainer _container;
    private Collider _col;
    //[Inject] IPlayerTouchMovement _playerTouchMovement;

    //[Inject] PlayerController _playerController;

    [Inject] 
    private void Constructor(DiContainer container)
    {
        _container = container;
    }

    public int ConsecutiveTileCount
    {
        get { return _consecutiveTileCount; }
        set { _consecutiveTileCount = value; }
    }

    public GameObject[] tilePrefabs;

    private float spawnOnZAxis = -12.0f;

    public float SpawnOnZAxis
    {
        get { return spawnOnZAxis; }
        set { spawnOnZAxis = value; }
    }

    private int amountOfTilesOnScreen = 10;
    private List<Vector3> originalTilePositions = new List<Vector3>();

    private float tileLength;

    private bool spawnSamePrefabTiles = true;
    private int numberOfSamePrefabTiles = 3; // Задайте кількість тайлів одного префабу

    private void Start()
    {
        //_playerTouchMovement.OnGroundChanged += UpdateGroundTile;
        SetMaxXPos();
    }

    public void RestartTiles()
    {
        ConsecutiveTileCount = 0;
        SpawnOnZAxis = -12.0f;
        foreach (var tile in spawnedTiles)
        {
            Destroy(tile);
        }
        spawnedTiles.Clear();
        m_Controller.MovementVector = new Vector3(0, 0, -m_Controller.Speed);
        StartSpawnTiles();
    }

    public List<Vector3> GetOriginalTilePositions()
    {
        return originalTilePositions;
    }

    public List<GameObject> GetSpawnedTiles()
    {
        return spawnedTiles;
    }

    private int startTilesSpawned; // Кількість вже спавнутих початкових тайлів

    public void StartSpawnTiles()
    {
        startTilesSpawned = 0;
        GetTileLength();
        for (int i = 0; i < amountOfTilesOnScreen; i++)
        {
            if (startTilesSpawned < 3) // Перевірка, чи ще не спавнили всі початкові тайли
            {
                SpawnTileWithStartTag(); // Спавнувати тайл з тегом "StartTiles"
            }
            else
            {
                SpawnTile(); // Звичайний спавн тайла
            }
        }
    }

    private void SpawnTileWithStartTag()
    {
        // Отримати індекси всіх тайлів з тегом "StartTiles"
        List<int> startTileIndexes = new List<int>();
        for (int i = 0; i < tilePrefabs.Length; i++)
        {
            if (tilePrefabs[i].CompareTag("StartTile"))
            {
                startTileIndexes.Add(i);
            }
        }

        if (startTileIndexes.Count > 0)
        {
            int randomStartTileIndex = startTileIndexes[UnityEngine.Random.Range(0, startTileIndexes.Count)];
            float spawnPosition = CalculateSpawnPosition();

            SpawnPrefabAtIndex(randomStartTileIndex, spawnPosition);

            CheckAndRemoveExcessTiles();

            UpdateOriginalTilePositions();

            startTilesSpawned++; // Збільшити лічильник спавнутих початкових тайлів
        }
        else
        {
            Debug.LogWarning("No tiles with the 'StartTiles' tag found.");
        }
    }


    public void GetTileLength()
    {
        tileLength = initialTile.GetColliderLength();
    }

    private void SetMaxXPos()
    {
        //if (_playerController != null && initialTile != null)
        //{
        //    var width = initialTile.GetColliderWidth();

        //    _playerController.SetMaxXPosition(width);
        //    Debug.Log(width);
        //}
    }

    public void SpawnTile(int prefabIndex = -1)
{
    if (prefabIndex == -1)
    {
        prefabIndex = GenerateRandomPrefabIndex();
    }
    else
    {
        ResetConsecutiveTileCount();
    }

    float spawnPosition = CalculateSpawnPosition();

    SpawnPrefabAtIndex(prefabIndex, spawnPosition);

    CheckAndRemoveExcessTiles();

    UpdateOriginalTilePositions();

}

private int GenerateRandomPrefabIndex()
{
    int randomIndex;
    if (_consecutiveTileCount < 5)
    {
        do
        {
            randomIndex = UnityEngine.Random.Range(0, tilePrefabs.Length);
        } while (randomIndex == 1);
        _consecutiveTileCount++;
    }
    else
    {
        randomIndex = UnityEngine.Random.Range(0, tilePrefabs.Length);
    }
    return randomIndex;
}

private void ResetConsecutiveTileCount()
{
    _consecutiveTileCount = 0;
}

    //private float CalculateSpawnPosition()
    //{
    //    if (spawnedTiles.Count != 0)
    //    {
    //        return spawnedTiles[spawnedTiles.Count - 1].transform.position.z + tileLength;
    //    }
    //    return spawnOnZAxis;
    //}

    //public float GetMinPos()
    //{
    //    //float minXPos = -initialTile.GetColliderWidth() / 2;
    //    ////Debug.Log($"ATTENTION MIN POS IS: {minXPos}");
    //    //return minXPos;
    //}

    //public float GetMaxPos()
    //{
    //    //float maxXPos = initialTile.GetColliderWidth() / 2;
    //    ////Debug.Log($"ATTENTION MAX POS IS: {maxXPos}");
    //    //return maxXPos;
    //}

    private float CalculateSpawnPosition()
    {
        float spawnPosition = spawnOnZAxis;

        if (spawnedTiles.Count != 0)
        {
            // Отримати довжину колайдера для останнього спавнутого тайла
            float lastTileLength = spawnedTiles[spawnedTiles.Count - 1].GetComponent<LevelTile>().GetColliderLength();

            // Розрахувати позицію спавну на основі довжини колайдера
            spawnPosition = spawnedTiles[spawnedTiles.Count - 1].transform.position.z + lastTileLength;
        }

        return spawnPosition;
    }


    private void SpawnPrefabAtIndex(int prefabIndex, float spawnPosition)
{
    var prefab = tilePrefabs[prefabIndex];
    GameObject gameObj = _container.InstantiatePrefab(prefab, transform);
    gameObj.transform.position = Vector3.forward * spawnPosition;
    spawnedTiles.Add(gameObj);
}

private void CheckAndRemoveExcessTiles()
{
    if (spawnedTiles.Count > amountOfTilesOnScreen)
    {
        Destroy(spawnedTiles[0]);
        spawnedTiles.RemoveAt(0);
    }
}

private void UpdateOriginalTilePositions()
{
    originalTilePositions.Clear();
    int count = Mathf.Min(5, spawnedTiles.Count);
    for (int i = 0; i < count; i++)
    {
        originalTilePositions.Add(spawnedTiles[i].transform.position);
    }
}

    //private float GetGroundTileWidth()
    //{
    //    if (initialTile != null)
    //    {
    //        Collider groundCollider = initialTile.GetComponent<Collider>();
    //        if (groundCollider != null)
    //        {
    //            return groundCollider.bounds.size.x;
    //        }
    //        else
    //        {
    //            Debug.LogError("Ground collider is missing on the ground tile.");
    //        }
    //    }
    //    else
    //    {
    //        Debug.LogError("No ground tile found.");
    //    }

    //    return 0f; // Повернення 0 у випадку невдачі
    //}

}