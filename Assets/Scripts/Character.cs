using System.Collections.Generic;
using UnityEngine;
using System;
using System.Collections;

public class Character : PooledMonoBehaviour, ITakeHit, IDie
{
    public static List<Character> All = new List<Character>();

    [Tooltip("Character Setup")]
    [SerializeField] float moveSpeed = 5.0f;
    //[SerializeField] int damage = 1;
    [SerializeField] int maxHealth = 10;
    

    Controller controller;
    IAttack attacker;
    Animator animator;
    int currentHealth;
    new Rigidbody rigidbody;

    [Tooltip("Audio Setup")]
    [SerializeField] List<AudioClip> _audioClips;
    AudioSource _audioSource;
    float _audioClipLength = 0.24f;
    float _timeSinceLastAudio = 0;


    public event Action<int, int> OnHealthChanged = delegate { };
    public event Action<IDie> OnDied = delegate { };
    public event Action OnHit = delegate {  };


    //public int Damage => damage;
    public bool Alive { get; private set; }

    void Awake()
    {
        attacker = GetComponent<IAttack>();
        animator = GetComponentInChildren<Animator>();
        rigidbody = GetComponent<Rigidbody>();
        _audioSource = GetComponent<AudioSource>();
    }


    internal void SetController(Controller controller)
    {
        this.controller = controller;
        foreach (var ability in GetComponents<AbilityBase>())
        {
            ability.SetController(controller);
        }
    }

    void Update()
    {
        _timeSinceLastAudio += Time.deltaTime;
        Vector3 direction = controller.GetDirection();
        var deadZone = 0.25f;
        if (direction.magnitude > deadZone)
        {
            direction.Normalize();
            var velocity = (direction * moveSpeed).With(y: rigidbody.velocity.y);
            rigidbody.velocity = velocity;
            transform.position += direction * (Time.deltaTime * moveSpeed);
            transform.forward = direction * 360f;

            animator.SetFloat("Speed", direction.magnitude);

            if (_timeSinceLastAudio >= _audioClipLength)
            {
                if (_audioClips.Count > 0)
                {
                    int randomIndex = UnityEngine.Random.Range(0, _audioClips.Count);
                    var clip = _audioClips[randomIndex];
                    StartCoroutine(PlaySoundEffects(clip));
                }
                
            }
            
        }
        else
        {
            animator.SetFloat("Speed", 0);
        }
    }

    IEnumerator PlaySoundEffects(AudioClip audioClip)
    {
        if (_audioSource != null)
        {
            _audioSource.PlayOneShot(audioClip);
            _timeSinceLastAudio = 0;
            yield return new WaitForSeconds(_audioClipLength);
        }
    }

    void OnEnable()
    {
        Alive = true;
        currentHealth = maxHealth;
        if (All.Contains(this) == false)
            All.Add(this);
    }
    protected override void OnDisable()
    {
        if (All.Contains(this))
            All.Remove(this);
        base.OnDisable();
    }

    public void TakeHit(IDamage hitBy)
    {
        if (currentHealth <= 0)
            return;
        ModifyHealth(-hitBy.Damage);
        OnHit();

        if (currentHealth <= 0)
        {
            Die();
        }
    }
    
    public void Heal(int healAmount)
    {
        ModifyHealth(healAmount);
        
    }

    void ModifyHealth(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Min(currentHealth, maxHealth);
        OnHealthChanged(currentHealth, maxHealth);
    }

    void Die()
    {
        Alive = false;
        OnDied(this);
    }


}