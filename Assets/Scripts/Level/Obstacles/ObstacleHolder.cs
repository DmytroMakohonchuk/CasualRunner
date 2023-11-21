using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleHolder : MonoBehaviour
{
    [SerializeField] private List<Pair> pairs;

    public ObstaclesTypes GetStylePrefabs(LocationStyle style)
    {
        foreach(var pair in pairs)
        {
            if(pair.locationStyle == style)
            {
                return pair.tilesPrefabs;
            }
        }
        throw new System.Exception();
    }
}

[Serializable]
public class Pair
{
    [field: SerializeField] public LocationStyle locationStyle { get; set; }
    [field: SerializeField] public ObstaclesTypes tilesPrefabs { get; set; }
}
