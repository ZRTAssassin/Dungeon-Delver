using System;
using UnityEngine;

public class ImpactParticles : MonoBehaviour
{
    [SerializeField] Vector3 hitParticleOffset;
    [SerializeField] PooledMonoBehaviour impactParticle;
    ITakeHit entity;

    void Awake()
    {
        entity = GetComponent<ITakeHit>();
        entity.OnHit += HandleHit;
    }
    void OnDestroy()
    {
        entity.OnHit -= HandleHit;
    }
    void HandleHit()
    {
        impactParticle.Get<PooledMonoBehaviour>(transform.position + hitParticleOffset, Quaternion.identity);
    }
}