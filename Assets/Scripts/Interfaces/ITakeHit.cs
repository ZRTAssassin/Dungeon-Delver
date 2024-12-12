﻿using System;
using UnityEngine;

public interface ITakeHit
{
    Transform transform { get; }
    
    event Action OnHit;
    void TakeHit(IDamage hitBy);
    bool Alive { get; }
    
    
}
