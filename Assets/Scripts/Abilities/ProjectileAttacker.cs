using System.Collections;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class ProjectileAttacker : AbilityBase, IAttack
{
    [SerializeField] Projectile projectilePrefab;
    [SerializeField] Transform projectileSpawnPosition;
    [SerializeField] float launchDelay = 1.0f;

    Animator animator;
    
    public int Damage { get { return 0; } }


    void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }

    public void Attack()
    {
        animator.SetTrigger(animationTrigger);
        StartCoroutine(LaunchAfterDelay());
        
    }

    IEnumerator LaunchAfterDelay()
    {
        yield return new WaitForSeconds(launchDelay);
        projectilePrefab.Get<Projectile>(projectileSpawnPosition.position, transform.rotation);
    }

    protected override void OnUse()
    {
        Attack();
    }
}
