using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.ProBuilder;
using UnityEngine.ProBuilder.MeshOperations;
using static UnityEngine.Rendering.DebugUI;
using Random = UnityEngine.Random;

public class ObstaclesCalculations : MonoBehaviour
{
    int itemSpace = 15;
    int itemCoundInMap = 5;

    [SerializeField] Collider tilePrefabCollider;
    [SerializeField] private GameObject[] emptyPrefabs;
    [SerializeField] private GameObject[] rampPrefabs;
    [SerializeField] private GameObject[] jumpPrefabs;
    [SerializeField] private GameObject[] bendPrefabs;
    [SerializeField] private GameObject[] blockPrefabs;
    [SerializeField] private GameObject coinPrefab;

    private Dictionary<Obstacles, GameObject[]> prefabsDictionary;
    private Dictionary<Obstacles, int> prefabsIndexesDictionary;

    private float xSize;
    private float zSize;
    private float zCoordinate;
    [SerializeField]private  int xSpawnPointsCount = 3;
    [SerializeField]private  int zSpawnPointsCount = 4;

    private Obstacles[,] obstaclesIndexes;

    int coinsCountItem = 10;
    float coinsHeight = 0.5f;

    private float xPosStep;
    private float yPosStep;

    [SerializeField] private int emptyChance = 40;

    private Vector3[,] tilePossibleSpawnPoints;

    public enum CoinStyle
    {
        Line,
        Jump,
        Ramp
    }

    public enum Obstacles
    {
        Empty = 0,
        Ramp = 1,
        Jump = 2,
        Bend = 3,
        Block = 4
    }

    private void Start()
    {
        InitializeObstaclesIndexes();
        SetPrefabsDefaultIndexes();
        SetSpawnRules();
        FillPrefabsDictionary();
        //FillObstaclesIndexes();
        GetTileSize();
        CalculateXposSpawnStep();
        CalculateYposSpawnStep();
        SetSpawnPointsArr();
        SpawnObstacles();
    }

    private void CreateCoins(CoinStyle style)
    {
        Vector3 coinPos = Vector3.zero;
        if(style == CoinStyle.Line)
        {
            
        }
    }
    
    private void SetPrefabsDefaultIndexes()
    {
        prefabsIndexesDictionary = new Dictionary<Obstacles, int>()
        {
            { Obstacles.Empty, 0 },
            { Obstacles.Ramp, 0 },
            { Obstacles.Jump, 0 },
            { Obstacles.Bend, 0 },
            { Obstacles.Block, 0 }
        };
    }

    private Obstacles GetRandomIndexFromDictionary(Dictionary<Obstacles, int> dictionary)
    {
        int count = 0;
        int randomValue = 0;
        int sumOfCurrentAndNext = -1;  

        foreach (var value  in dictionary.Values)
        {
            count += value;
        }

        randomValue = Random.Range(0, count);

        for (int i = 0; i < dictionary.Count; i++)
        {
                sumOfCurrentAndNext += dictionary.ElementAt(i).Value;

                if (sumOfCurrentAndNext >= randomValue)
                {
                    return dictionary.ElementAt(i).Key;
                }
        }

        Debug.Log("KNOWN ERROR!");
        return Obstacles.Empty;
    }

    private void SetSpawnRules()
    {
        int zeroChance = 0;
        int averageChance = 1;
        int maxChance = 5;
        int lowChance = 3;
        int blockCounter;

        for(int j = 0; j < zSpawnPointsCount; j++)
        {
            blockCounter = 0;

            for (int i = 0; i < xSpawnPointsCount; i++)
            {
               if(j != 0)
                {
                    var previousObstacle = obstaclesIndexes[i, j - 1];
                    if(obstaclesIndexes[i, j] == Obstacles.Block)
                    {
                        if(blockCounter == 1)
                        {
                            continue;
                        }

                        blockCounter++;
                    }

                    switch (previousObstacle)
                    {
                        case Obstacles.Empty:
                            SetSpawnPercentForAllValues(averageChance);
                            break;

                        case Obstacles.Ramp:
                            SetSpawnPercentForAllValues(zeroChance);
                            prefabsIndexesDictionary[Obstacles.Block] = maxChance;
                            break;

                        case Obstacles.Jump:
                            SetSpawnPercentForAllValues(zeroChance);
                            prefabsIndexesDictionary[Obstacles.Ramp] = lowChance;
                            prefabsIndexesDictionary[Obstacles.Empty] = maxChance;
                            prefabsIndexesDictionary[Obstacles.Bend] = lowChance;
                            break;

                        case Obstacles.Bend:
                            SetSpawnPercentForAllValues(zeroChance);
                            prefabsIndexesDictionary[Obstacles.Ramp] = maxChance;
                            prefabsIndexesDictionary[Obstacles.Empty] = maxChance;
                            prefabsIndexesDictionary[Obstacles.Jump] = lowChance;
                            break;

                        case Obstacles.Block:
                            SetSpawnPercentForAllValues(zeroChance);
                            prefabsIndexesDictionary[Obstacles.Block] = averageChance;
                            prefabsIndexesDictionary[Obstacles.Empty] = averageChance;
                            break;
                    }
                    if(j == zSpawnPointsCount - 1)
                    {
                        prefabsIndexesDictionary[Obstacles.Ramp] = 0;
                    }
                }
                else
                {
                    SetSpawnPercentForAllValues(zeroChance);
                    prefabsIndexesDictionary[Obstacles.Empty] = averageChance;
                }
                obstaclesIndexes[i, j] = GetRandomIndexFromDictionary(prefabsIndexesDictionary);
            }
        }

        int randomColumnValue = Random.Range(0, xSpawnPointsCount);
        for (int j = 1; j < zSpawnPointsCount; j++)
        {
            
            if (obstaclesIndexes[randomColumnValue, j] == Obstacles.Block && !(obstaclesIndexes[randomColumnValue, j-1] == Obstacles.Ramp))
            {
                obstaclesIndexes[randomColumnValue, j] = Obstacles.Empty;
            }
        }
    }

    private void SetSpawnPercentForAllValues(int value)
    {
        for (int k = 0; k < prefabsIndexesDictionary.Count; k++)
        {
            prefabsIndexesDictionary[(Obstacles)k] = value;
        }
    }

    private void FillPrefabsDictionary()
    {
        prefabsDictionary = new Dictionary<Obstacles, GameObject[]>()
    {
        { Obstacles.Empty, emptyPrefabs },
        { Obstacles.Ramp, rampPrefabs },
        { Obstacles.Jump, jumpPrefabs },
        { Obstacles.Bend, bendPrefabs },
        { Obstacles.Block, blockPrefabs }

    };
    }



    private void InitializeObstaclesIndexes()
    {
        obstaclesIndexes = new Obstacles[xSpawnPointsCount, zSpawnPointsCount];
    }

    private void GetTileSize()
    {
        xSize = tilePrefabCollider.bounds.size.x;
        zSize = tilePrefabCollider.bounds.size.z;
        zCoordinate = tilePrefabCollider.transform.parent.position.z;
    }

    private void SetSpawnPointsArr()
    {
        tilePossibleSpawnPoints = new Vector3[xSpawnPointsCount, zSpawnPointsCount];
    }

    private void CalculateXposSpawnStep()
    {
        xPosStep = xSize/(xSpawnPointsCount * 2);
    }

    private void CalculateYposSpawnStep()
    {
        yPosStep = zSize / (zSpawnPointsCount * 2);
    }

    private void SpawnObstacles()
    {
        List<GameObject> obstaclesToSpawn = new List<GameObject>();
        for (int i = 0; i < xSpawnPointsCount; i++)
        {
            for (int j = 0; j < zSpawnPointsCount; j++)
            {
                var currentEnum = obstaclesIndexes[i, j];
                GameObject obstacle;
                var obstacles = prefabsDictionary[currentEnum];
                var randomNumber = Random.Range(0, obstacles.Length);
                obstacle = obstacles[randomNumber];
                tilePossibleSpawnPoints[i, j] = new Vector3(xPosStep * (i * 2 + 1) - (xSize / 2), 1, yPosStep * (j * 2 + 1) - (zSize / 2) + zCoordinate);
                var point = tilePossibleSpawnPoints[i, j];
                var obstacleToSpawn = Instantiate(obstacle, point, Quaternion.identity, tilePrefabCollider.transform);
                obstaclesToSpawn.Add(obstacleToSpawn);
            }
        }
    }

    private bool GenerateRandomChance(int chance)
    {
        var currentNum = Random.Range(1, 101);
        return chance > currentNum;
    }
}
