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
    [SerializeField] protected float attackSpeed = 1;
    protected float timeToAttack;
    private Healthable _healthable;

    void Start()
    {
        _healthable = GetComponent<Healthable>();
        _healthable.OnDie.AddListener(() => Destroy(gameObject));
        visionDetector.OnTargetChanged.AddListener(ChangeTarget);
        attackDetector.OnEnemyDetected.AddListener(ChangeToAttack);
        attackDetector.OnEnemyDisapeared.AddListener(ChangeToMove);
        ChangeToMove();
    }

    // Update is called once per frame
    void Update()
    {
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
        DealDamage(currentTarget.GetComponent<Healthable>());
        timeToAttack = attackSpeed;
    }

    public void ChangeToAttack()
    {
        ChangeState(State.Attack);
        // GetComponent<AILerp>().speed = 0;
        GetComponent<AILerp>().canMove = false;
    }

    public void ChangeToMove()
    {
        ChangeState(State.Move);
        // GetComponent<AILerp>().speed = speed;
        GetComponent<AILerp>().canMove = true;
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

    private enum State
    {
        Move,
        Attack
    }
}