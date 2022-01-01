using System;
using UnityEngine;

public class DeathParticles : MonoBehaviour
{

    [SerializeField] PooledMonoBehaviour deathParticlePrefab;
    [SerializeField] Vector3 deathParticleOffset;

    IDie entity;

    void Awake()
    {
        entity = GetComponent<IDie>();
    }

    void OnEnable()
    {
        entity.OnDied += EntityOnDied;
    }

    void EntityOnDied(IDie entity)
    {
        entity.OnDied += EntityOnDied;
        deathParticlePrefab.Get<PooledMonoBehaviour>((transform.position + deathParticleOffset), Quaternion.identity);
    }

    void OnDisable()
    {
        entity.OnDied += EntityOnDied;
    }
}