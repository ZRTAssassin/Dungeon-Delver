using System;
using UnityEngine;

public class Projectile : PooledMonoBehaviour, IDamage
{
    [Header("Projectile Setup")]
    [Tooltip("How fast the projectile moves after spawned.")]
    [SerializeField] float moveSpeed = 10f;
    [Tooltip("How much damage this projectile does on a hit.")]
    [SerializeField] int damage;
    [Header("Misc Setup")]
    [SerializeField] PooledMonoBehaviour impactParticlePrefab;
    [Header("Audio Setup")]
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip spawnClip;
    [SerializeField] AudioClip impactClip;

    public int Damage => damage;

    void Awake()
    {
        PlayAudioSound(spawnClip);
    }

    void FixedUpdate()
    {
        transform.position += transform.forward * (moveSpeed * Time.deltaTime);
    }


    void OnCollisionEnter(Collision collision)
    {
        PlayParticle();
        PlayAudioSound(impactClip);
        var hit = collision.collider.GetComponent<ITakeHit>();
        if (hit != null)
        {
            Impact(hit);
        }
        else
        {
            ReturnToPool();
        }
    }

    void PlayAudioSound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }

    void PlayParticle()
    {
        if (impactParticlePrefab != null)
        {
            impactParticlePrefab.Get<PooledMonoBehaviour>(transform.position, Quaternion.identity);
        }
    }

    void Impact(ITakeHit hit)
    {
        hit.TakeHit(this);

        ReturnToPool();
    }
}