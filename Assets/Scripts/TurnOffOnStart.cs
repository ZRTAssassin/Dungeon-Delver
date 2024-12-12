using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnOffOnStart : MonoBehaviour
{
    List<Transform> children = new List<Transform>();

    void Awake()
    {
        for (int i = 0; i < transform.childCount - 1; i++)
        {
            children.Add(transform.GetChild(i));
        }
    }

    void Start()
    {
        foreach (var child in children)
        {
            gameObject.SetActive(false);
        }
    }
}