using System;
using UnityEngine;

public class Box : MonoBehaviour, ITakeHit
{
    new Rigidbody rigidbody;
    public event Action OnHit = delegate {  };
    [SerializeField] float forceAmount = 10f;
    [SerializeField] AudioSource _audioSource;
    [SerializeField] AudioClip[] _audioClips;
    public bool Alive => true;

    void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    

    public void TakeHit(IDamage hitBy)
    {
        var direction = Vector3.Normalize(transform.position - hitBy.transform.position);
        rigidbody.AddForce(direction * forceAmount, ForceMode.Impulse);
        PlaySoundEffect();

        OnHit();
        
    }

    void PlaySoundEffect()
    {
        if (_audioSource != null && _audioClips.Length > 0)
        {
            int randomIndex = UnityEngine.Random.Range(0, _audioClips.Length);
            var clip = _audioClips[randomIndex];
            _audioSource.PlayOneShot(clip);
        }
    }
}
