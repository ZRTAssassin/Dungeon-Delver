using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knockback : AbilityBase, IDamage
{
    [SerializeField] int damage = 1;
    [SerializeField] float attackRadius = 2f;
    [SerializeField] float impactDelay = 0.25f;
    [SerializeField] float forceAmount = 10f;
    
    Collider[] attackResults;
    LayerMask layerMask;

    public int Damage => damage;

    void Awake()
    {
        string currentLayer = LayerMask.LayerToName(gameObject.layer);
        layerMask = ~LayerMask.GetMask(currentLayer);
        
        attackResults = new Collider[10];
    }

    protected override void OnUse()
    {
        Attack();
    }

    void Attack()
    {
        StartCoroutine(DoAttack());
    }

    IEnumerator DoAttack()
    {
        yield return new WaitForSeconds(impactDelay);
        Vector3 position = transform.position + transform.forward;
        int hitCount = Physics.OverlapSphereNonAlloc(position, attackRadius, attackResults, layerMask);
       

        for (int i = 0; i < hitCount; i++)
        {
            var takeHit = attackResults[i].GetComponent<ITakeHit>();
            if (takeHit != null)
            {
                takeHit.TakeHit(this);
            }

            var hitRigidbody = attackResults[i].GetComponent<Rigidbody>();
            if (hitRigidbody != null)
            {
                var direction = Vector3.Normalize(hitRigidbody.transform.position - transform.position);
                hitRigidbody.AddForce(direction * forceAmount, ForceMode.Impulse);
                
            }
        }
    }
}