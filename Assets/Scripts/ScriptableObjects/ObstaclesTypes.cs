using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ObstaclesTypes", menuName = "CustomObjects/ObstaclesTypes", order = 1)]
public class ObstaclesTypes : ScriptableObject
{
    [SerializeField] public GameObject[] emptyPrefabs;
    [SerializeField] public GameObject[] rampPrefabs;
    [SerializeField] public GameObject[] jumpPrefabs;
    [SerializeField] public GameObject[] bendPrefabs;
    [SerializeField] public GameObject[] blockPrefabs;
}

public enum LocationStyle
{
    fortress = 0,
    farm = 1,
    city = 2
}
