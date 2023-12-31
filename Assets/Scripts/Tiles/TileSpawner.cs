using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using Zenject;
using Random = UnityEngine.Random;

public class TileSpawner : MonoBehaviour, ITileSpawner
{
    public event Action<LevelTile> OnTileChanged;

    [Inject] TileMovementController m_Controller;
    [SerializeField] GameObject startTile;
    [SerializeField] LevelTile initialTile;
    [SerializeField] List<ListOfGameObject> allTilesPrefabs;

    private LevelTile? groundTile;
    private int counter;
    private int randomNumber;
    private int randomNumberNew;
    private List<GameObject> spawnedTiles = new List<GameObject>();
    private int _consecutiveTileCount = 0;
    private DiContainer _container;
    private Collider _col;
    private int sameTypeTileCounter = 0;

    public int ConsecutiveTileCount
    {
        get { return _consecutiveTileCount; }
        set { _consecutiveTileCount = value; }
    }

    public List<GameObject> tilePrefabs;

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
    [SerializeField] private int numberOfSamePrefabTiles = 4; // ������� ������� ����� ������ �������
    //[Inject] IPlayerTouchMovement _playerTouchMovement;

    //[Inject] PlayerController _playerController;

    [Inject] 
    private void Constructor(DiContainer container)
    {
        _container = container;
    }

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
        sameTypeTileCounter = 0;
        SetTilePrefabs();
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

    private int startTilesSpawned; // ʳ������ ��� ��������� ���������� �����

    public void StartSpawnTiles()
    {
        startTilesSpawned = 0;
        GetTileLength();
        for (int i = 0; i < amountOfTilesOnScreen; i++)
        {
            if (startTilesSpawned < 2) // ��������, �� �� �� �������� �� �������� �����
            {
                SpawnTileWithStartTag(); // ���������� ���� � ����� "StartTiles"
            }
            else
            {
                SpawnTile(); // ��������� ����� �����
            }
        }
    }

    private void SpawnTileWithStartTag()
    {
        tilePrefabs = new List<GameObject>()
        {
            startTile,
        };
        // �������� ������� ��� ����� � ����� "StartTiles"
        List<int> startTileIndexes = new List<int>();
        for (int i = 0; i < tilePrefabs.Count; i++)
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
            startTilesSpawned++; // �������� �������� ��������� ���������� �����
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
        SetTilePrefabs();
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
                randomIndex = UnityEngine.Random.Range(0, tilePrefabs.Count);
            } while (randomIndex == 1);
            _consecutiveTileCount++;
        }
        else
        {
            randomIndex = UnityEngine.Random.Range(0, tilePrefabs.Count);
        }
        return randomIndex;
    }

    private void ResetConsecutiveTileCount()
    {
        _consecutiveTileCount = 0;
    }

    private float CalculateSpawnPosition()
    {
        float spawnPosition = spawnOnZAxis;

        if (spawnedTiles.Count != 0)
        {
            // �������� ������� ��������� ��� ���������� ���������� �����
            float lastTileLength = spawnedTiles[spawnedTiles.Count - 1].GetComponent<LevelTile>().GetColliderLength();

            // ����������� ������� ������ �� ����� ������� ���������
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

    private void SetTilePrefabs()
    {
        sameTypeTileCounter++;
        if (sameTypeTileCounter > numberOfSamePrefabTiles || tilePrefabs == null)
        {
            GenerateNewValue();
            tilePrefabs = allTilesPrefabs[randomNumber].tilesPrefabs;
            sameTypeTileCounter = 0;
        }

        void GenerateNewValue()
        {
            randomNumberNew = Random.Range(0, allTilesPrefabs.Count);
            if (randomNumber == randomNumberNew)
            {
                counter++;
                if (counter > 1)
                {
                    GenerateNewValue();
                }
            }
            else
            {
                counter = 0;
            }
            randomNumber = randomNumberNew;
        }
    }

    [Serializable]
    public class ListOfGameObject
    {
        [field: SerializeField]public List<GameObject> tilesPrefabs { get; set; }
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

    //    return 0f; // ���������� 0 � ������� �������
    //}

}