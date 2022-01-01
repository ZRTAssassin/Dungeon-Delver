using System;
using System.Collections;
using UnityEngine;

public class Attacker : AbilityBase, IAttack
{
    [SerializeField] int damage = 1;
    [SerializeField] float attackOffset = 1f;
    [SerializeField] float attackRadius = 2f;
    [SerializeField] float attackImpactDelay = 0.5f;
    [SerializeField] float attackRange = 2f;
    
    LayerMask layerMask;

    public int Damage => damage;

    Collider[] attackResults;
    Animator animator;
    
    void Awake()
    {
        string currentLayer = LayerMask.LayerToName(gameObject.layer);
        layerMask = ~LayerMask.GetMask(currentLayer);
        
        animator = GetComponentInChildren<Animator>();

        var AnimationImpactWatcher = GetComponentInChildren<AnimationImpactWatcher>();
        if (AnimationImpactWatcher != null)
        {
            AnimationImpactWatcher.OnImpact += AnimationImpactWatcher_OnImpact;
        }

        attackResults = new Collider[10];
    }


    public void Attack(ITakeHit target)
    {
        animator.SetTrigger(animationTrigger);
        attackTimer = 0;
        StartCoroutine(DoAttack(target));
    }
    protected override void OnUse()
    {
        Attack();
    }
    public void Attack()
    {
        animator.SetTrigger(animationTrigger);
    }

    public bool InAttackRange(ITakeHit target)
    {
        if (target.Alive == false)
            return false;

        var distance = Vector3.Distance(transform.position, target.transform.position);
        return distance < attackRange;
    }

    IEnumerator DoAttack(ITakeHit target)
    {
        yield return new WaitForSeconds(attackImpactDelay);
        if (target.Alive && InAttackRange(target))
            target.TakeHit(this);
    }


    /// <summary>
    /// Called by animation event via AnimationImpactWatcher
    /// </summary>
    void AnimationImpactWatcher_OnImpact()
    {
        Vector3 position = transform.position + transform.forward * attackOffset;
        int hitCount = Physics.OverlapSphereNonAlloc(position, attackRadius, attackResults, layerMask);
        

        for (int i = 0; i < hitCount; i++)
        {
            var takeHit = attackResults[i].GetComponent<ITakeHit>();
            if (takeHit != null)
                takeHit.TakeHit(this);
        }
    }

    void OnDrawGizmosSelected()
    {
        Vector3 position = transform.position + transform.forward * attackOffset;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(position, attackRadius);
    }
}