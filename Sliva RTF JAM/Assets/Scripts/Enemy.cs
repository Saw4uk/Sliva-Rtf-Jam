using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using Pathfinding;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float damage;
    [SerializeField] private EnemyVisionDetector visionDetector;
    [SerializeField] private EnemyAttackDetector attackDetector;
    private Transform defaultTarget;
    private Transform currentTarget;
    public Transform CurrentTarget => currentTarget;
    private State state = State.Move;
    [SerializeField] private float speed;
    [SerializeField] private float timeToAttackMax = 1;
    private float timeToAttack;

    void Start()
    {
        ChangeToMove();
        visionDetector.OnEnemyDetected.AddListener(ChangeTarget);
        visionDetector.OnEnemyDisapeared.AddListener(ChangeTargetToDefault);
        attackDetector.OnEnemyDetected.AddListener(ChangeToAttack);
        attackDetector.OnEnemyDisapeared.AddListener(ChangeToMove);
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

    private void Attack()
    {
        Debug.Log("Attack");
        currentTarget.GetComponent<CanBeDamaged>().TakeDamage(damage);
        timeToAttack = timeToAttackMax;
    }

    public void ChangeToAttack()
    {
        ChangeState(State.Attack);
        GetComponent<AILerp>().speed = 0;
    }

    public void ChangeToMove()
    {
        ChangeState(State.Move);
        GetComponent<AILerp>().speed = speed;
    }

    public void DealDamage(CanBeDamaged target)
    {
        target.TakeDamage(damage);
    }

    public void ChangeTarget(Transform newTarget)
    {
        GetComponent<AIDestinationSetter>().target = newTarget;
        currentTarget = newTarget;
    }

    public void ChangeTargetToDefault()
    {
        GetComponent<AIDestinationSetter>().target = defaultTarget;
        currentTarget = defaultTarget;
    }

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