using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelComplete : MonoBehaviour
{
    void OnTriggerEnter(Collider collider)
    {
        var character = collider.GetComponent<Character>();
        if (character != null)
        {
            MenuManager.Instance.DislayEndScreen();
        }
    }
}
