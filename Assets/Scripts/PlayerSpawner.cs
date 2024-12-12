using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] Vector3 spawnPosition;

    public Vector3 SpawnPosition => spawnPosition;

    void Awake()
    {
        spawnPosition = transform.position;
    }
}
