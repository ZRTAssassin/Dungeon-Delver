using System;
using UnityEngine;

public interface IDie
{
    event Action<int, int> OnHealthChanged;
    public event Action<IDie> OnDied;
    GameObject gameObject { get; }
}