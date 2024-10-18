using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using Pathfinding;
using UnityEngine;
using UnityEngine.Serialization;

public class Enemy : MonoBehaviour
{
    [SerializeField] protected float damage;
    [SerializeField] private EnemyVisionDetector visionDetector;
    [SerializeField] private EnemyAttackDetector attackDetector;
    private Transform defaultTarget;
    private Transform currentTarget;
    public Transform DefaultTarget => defaultTarget;
    public Transform CurrentTarget => currentTarget;
    private State state = State.Move;
    public State CurrentState => state;
    [SerializeField] protected float attackSpeed = 1;
    protected float timeToAttack;
    private Healthable _healthable;
    protected Animator animator;
    private AILerp aiLerp;
    [SerializeField] private Transform glitchedPrefab;
    private bool isDestroyingSelf;

    void Start()
    {
        aiLerp = GetComponent<AILerp>();
        animator = GetComponent<Animator>();
        _healthable = GetComponent<Healthable>();
        _healthable.OnDie.AddListener(Die);
        visionDetector.OnTargetChanged.AddListener(ChangeTarget);
        attackDetector.OnEnemyDetected.AddListener(ChangeToAttack);
        attackDetector.OnEnemyDisapeared.AddListener(ChangeToMove);
        ChangeToMove();
    }

    private void Die()
    {
        animator.SetTrigger("Die");
        aiLerp.canMove = false;
        StartCoroutine(DestroySelf());
    }

    private IEnumerator DestroySelf()
    {
        isDestroyingSelf = true;
        yield return new WaitForSeconds(1f);
        GetComponent<CoinDropper>().DropCoin();
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (isDestroyingSelf)
        {
            return;
        }

        var currentRotation = transform.rotation;
        if (currentTarget.position.x < transform.position.x)
        {
            currentRotation.y = 180;
        }
        else
        {
            currentRotation.y = 0;
        }

        transform.rotation = currentRotation;

        bool canAttack = false;
        if (timeToAttack < 0)
        {
            canAttack = true;
        }
        else
        {
            timeToAttack -= Time.deltaTime;
        }

        switch (state)
        {
            case State.Move:
                break;
            case State.Attack:
                if (canAttack)
                {
                    Attack();
                }

                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    protected virtual void Attack()
    {
        animator.SetTrigger("Attack");
        DealDamage(currentTarget.GetComponent<Healthable>());
        timeToAttack = attackSpeed;
    }

    public void ChangeToAttack()
    {
        ChangeState(State.Attack);
        // GetComponent<AILerp>().speed = 0;
        GetComponent<AILerp>().canMove = false;
        // animator.SetTrigger("Attack");
    }

    public void ChangeToMove()
    {
        ChangeState(State.Move);
        // GetComponent<AILerp>().speed = speed;
        GetComponent<AILerp>().canMove = true;
        // animator.SetTrigger("Move");
        Debug.Log("SET Move");
    }

    public void ChangeToMoveToDefault()
    {
        ChangeTarget(defaultTarget);
        ChangeToMove();
        // ChangeState(State.Move);
        // // GetComponent<AILerp>().speed = speed;
        // GetComponent<AILerp>().canMove = true;
    }

    public void DealDamage(Healthable target)
    {
        target.TakeDamage(damage);
    }

    public void ChangeTarget(Transform newTarget)
    {
        GetComponent<AIDestinationSetter>().target = newTarget;
        currentTarget = newTarget;
    }

    // public void ChangeTargetToDefault()
    // {
    //     GetComponent<AIDestinationSetter>().target = defaultTarget;
    //     currentTarget = defaultTarget;
    // }

    public void SetDefaultTarget(Transform newTarget)
    {
        defaultTarget = newTarget;
    }

    private void ChangeState(State newState)
    {
        state = newState;
    }

    public enum State
    {
        Move,
        Search,
        Attack
    }

    public void AttackTargetDisapeared()
    {
        var attackTarget = attackDetector.GetClosestTarget();
        var moveTarget = visionDetector.GetClosestTarget();
        if (state is State.Attack)
        {
            if (attackTarget is not null)
            {
                ChangeTarget(attackTarget);
            }
            else
            {
                ChangeTarget(moveTarget);
                ChangeToMove();
            }
        }
        //
        // if (state is State.Move)
        // {
        //     if (moveTarget != currentTarget)
        //     {
        //         return;
        //     }
        //     if (attackTarget is not null)
        //     {
        //         ChangeTarget(attackTarget);
        //     }
        //     else
        //     {
        //         ChangeTarget(moveTarget);
        //     }
        // }
    }

    public void MoveTargetDisapeared()
    {
        var attackTarget = attackDetector.GetClosestTarget();
        var moveTarget = visionDetector.GetClosestTarget();
        if (state is State.Attack)
        {
            if (attackTarget is not null)
            {
            }
            else
            {
                ChangeTarget(moveTarget);
                ChangeToMove();
            }
        }

        else if (state is State.Move)
        {
            if (moveTarget != defaultTarget)
            {
                ChangeTarget(moveTarget);
            }
            else
            {
                if (attackTarget is not null)
                {
                    ChangeTarget(attackTarget);
                    ChangeToAttack();
                }

                {
                    ChangeTarget(defaultTarget);
                    ChangeToMove();
                }
            }
        }
    }

    public virtual IEnumerator TurnIntoGlitch()
    {
        if (isDestroyingSelf)
        {
            yield break;;
        }

        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
        var glitch = Instantiate(glitchedPrefab, transform.position, transform.rotation);
        glitch.GetComponent<GlitchedEnemy>().SetDefaultTarget(defaultTarget);
        glitch.GetComponent<GlitchedEnemy>().ChangeTarget(currentTarget);
    }
}