using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class Interactable : MonoBehaviour
{
    [SerializeField] Vector3 _hitParticleOffset;
    [SerializeField] PooledMonoBehaviour _interactParticle;
    [SerializeField] Animator _animator;
    [SerializeField] AudioSource _audiosource;
    
    bool _canBeUsed = true;
    int _usedHash = Animator.StringToHash("Used");

    void Awake()
    {
        if (_audiosource == null)
            Debug.LogWarning("Interactable object " + gameObject.name + " missing audio source");
        if (_animator == null)
            Debug.LogWarning("Interactable Object " + gameObject.name + " missing animator.");
        
    }

    void OnTriggerEnter(Collider collider)
    {
        var character = collider.GetComponents<Character>();
        if (character != null && _canBeUsed)
        {
            UseInteractableObject();
            PlayParticle();
            PlaySoundEffect();
        }
        
    }

    void PlaySoundEffect()
    {
        if (_audiosource != null)
            _audiosource.Play();
    }

    void PlayParticle()
    {
        if (_interactParticle != null)
        {
            _interactParticle.Get<PooledMonoBehaviour>(transform.position + _hitParticleOffset, Quaternion.identity);
        }
    }

    void UseInteractableObject()
    {
        if (_animator != null)
        {
            _canBeUsed = false;
            _animator.SetTrigger(_usedHash);
        }
    }
}
