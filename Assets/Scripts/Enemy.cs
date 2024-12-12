using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Attacker))]
[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : PooledMonoBehaviour, ITakeHit, IDie
{
    [SerializeField] int maxHealth = 3;
    [SerializeField] List<AudioClip> hitClips;
    [SerializeField] AudioClip deathClip;

    int currentHealth;


    Animator animator;
    NavMeshAgent navMeshAgent;
    Attacker attacker;
    Character target;
    new Rigidbody rigidbody;
    bool isInitialized;

    AudioSource audioSource;

    public event Action<int, int> OnHealthChanged = delegate { };
    public event Action<IDie> OnDied = delegate { };
    public event Action OnHit = delegate { };

    bool IsDead => currentHealth <= 0;
    public bool Alive { get; private set; }

    void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        attacker = GetComponent<Attacker>();
        rigidbody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    IEnumerator Initailiaze()
    {
        navMeshAgent.enabled = false;
        yield return new WaitForSeconds(1.0f);
        navMeshAgent.enabled = true;
        isInitialized = true;
    }

    void OnEnable()
    {
        isInitialized = false;
        currentHealth = maxHealth;
        Alive = true;
        StartCoroutine(Initailiaze());
    }

    void Update()
    {
        if (IsDead || !isInitialized)
            return;
        if (target == null || target.Alive == false)
        {
            AcquireTarget();
        }
        else
        {
            if (attacker.InAttackRange(target) == false)
            {
                FollowTarget();
            }
            else
            {
                TryAttack();
            }
        }
    }

    void AcquireTarget()
    {
        target = Character.All
            .OrderBy(t => Vector3.Distance(transform.position, t.transform.position))
            .FirstOrDefault();
        animator.SetFloat("Speed", 0f);
    }

    void FollowTarget()
    {
        animator.SetFloat("Speed", 1f);
        navMeshAgent.isStopped = false;
        navMeshAgent.SetDestination(target.transform.position);
    }

    void TryAttack()
    {
        animator.SetFloat("Speed", 0f);
        navMeshAgent.isStopped = true;

        if (attacker.CanAttack)
        {
            attacker.Attack(target);
        }
    }


    public void TakeHit(IDamage hitBy)
    {
        currentHealth -= hitBy.Damage;

        OnHealthChanged(currentHealth, maxHealth);
        OnHit();

        PlayHitSound();

        if (currentHealth <= 0)
        {
            animator.SetTrigger("Hit");
            Die();
        }
        else
        {
            animator.SetTrigger("Hit");
        }
    }

    void PlayHitSound()
    {
        if (hitClips.Count > 0 && audioSource != null)
        {
            int randomIndex = UnityEngine.Random.Range(0, hitClips.Count);
            var clip = hitClips[randomIndex];
            audioSource.PlayOneShot(clip);
        }
        
    }

    void PlayDeathSound()
    {
        audioSource.PlayOneShot(deathClip);
    }

    void Die()
    {
        PlayDeathSound();
        animator.SetTrigger("Die");
        navMeshAgent.isStopped = true;
        GetComponent<Collider>().enabled = false;
        rigidbody.constraints = RigidbodyConstraints.FreezeAll;

        Alive = false;
        OnDied(this);
        ReturnToPool(6f);
    }
}