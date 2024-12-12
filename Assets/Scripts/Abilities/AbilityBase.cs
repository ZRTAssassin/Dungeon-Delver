using UnityEngine;

public abstract class AbilityBase : MonoBehaviour
{
    [SerializeField] float attackRefreshSpeed = 1.5f;
    [SerializeField] PlayerButton button;
    [SerializeField] protected string animationTrigger;

    protected float attackTimer;
    Controller controller;
    Animator animator;

    public bool CanAttack => (attackTimer >= attackRefreshSpeed);
    protected abstract void OnUse();

    public void SetController(Controller controller)
    {
        this.controller = controller;
    }

    void Update()
    {
        attackTimer += Time.deltaTime;

        if (animator == null)
            animator = GetComponentInChildren<Animator>();

        if (ShouldTryUse())
        {
            if (string.IsNullOrEmpty(animationTrigger) == false)
            {
                animator.SetTrigger(animationTrigger);
            }

            attackTimer = 0;
            OnUse();
        }
    }

    bool ShouldTryUse()
    {
        return controller != null && CanAttack && controller.ButtonDown(button);
    }
}