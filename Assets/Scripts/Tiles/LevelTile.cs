using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class LevelTile : MonoBehaviour
{
    [SerializeField] Collider _collider;

    //private void Awake()
    //{
    //    _collider = GetComponent<Collider>();
    //}

    public float GetColliderLength()
    {
        if (_collider != null)
        {
            //return _collider.bounds.size.z;
            var length = _collider.bounds.size.z;
            Debug.Log(length);
            return length;
        }
        else
        {
            Debug.Log("Collider is null");
            return 0;
        }
    }

    //private void FixedUpdate()
    //{ 
    //    GetColliderLength();
    //}

    public float GetColliderWidth(Collider collider)
    {
        return collider.bounds.size.x;
    }

    private void UpdateCollider(Collider collider)
    {
        _collider = collider;
    }
}
