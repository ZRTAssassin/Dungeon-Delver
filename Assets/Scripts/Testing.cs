using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Testing : MonoBehaviour
{
    void Awake()
    {
        Debug.Log(SceneManager.GetActiveScene().name + " " + gameObject.name + " needs functionality added");
    }
}
