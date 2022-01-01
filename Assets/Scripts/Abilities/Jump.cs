using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : AbilityBase
{
    //[SerializeField] float jumpForce = 100;
    new Rigidbody rigidbody;
    void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    protected override void OnUse()
    {
        
        //rigidbody.AddForce(Vector3.up * jumpForce);
    }
}
