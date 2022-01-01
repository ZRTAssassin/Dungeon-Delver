using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnabledChildrenOnEnter : MonoBehaviour
{
    [Range(1, 15)]
    [SerializeField] float _sphereRadius = 15;   
    void Awake()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponents<Character>() != null)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(true);
            }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _sphereRadius);
    }

    void OnValidate()
    {
        Collider collider = GetComponent<Collider>();
        if (collider == null)
        {
            collider.gameObject.AddComponent<SphereCollider>();
        }
        ((SphereCollider)collider).radius = _sphereRadius;
        collider.isTrigger = true;
    }
}
