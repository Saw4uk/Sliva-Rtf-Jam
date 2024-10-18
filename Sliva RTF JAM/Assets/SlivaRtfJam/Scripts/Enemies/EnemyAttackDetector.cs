using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace DefaultNamespace
{
    public class EnemyAttackDetector : MonoBehaviour
    {
        [SerializeField] private Enemy enemy;
        public UnityEvent OnEnemyDetected;
        public UnityEvent OnEnemyDisapeared;
        public List<Transform> Targets;

        // private void OnTriggerEnter2D(Collider2D other)
        // {
        //     // Debug.Log(other.tag);
        //     if (!other.CompareTag("Player"))
        //     {
        //         return;
        //     }
        //
        //     Targets.Add(other.transform);
        //
        //     if (other.transform == enemy.CurrentTarget)
        //     {
        //         OnEnemyDetected?.Invoke();
        //     }
        // }
        //
        // private void OnTriggerExit2D(Collider2D other)
        // {
        //     if (!other.CompareTag("Player"))
        //     {
        //         return;
        //     }
        //     Debug.Log("Exited");
        //     Targets.Remove(other.transform);
        //     if (other.transform == enemy.CurrentTarget)
        //     {
        //         var closestTarget = GetClosestTarget();
        //         if (closestTarget is not null)
        //         {
        //             Debug.Log("ShootNext");
        //             enemy.ChangeTarget(closestTarget);
        //         }
        //         else
        //         {
        //             Debug.Log("Move");
        //             enemy.ChangeToMoveToDefault();
        //             // OnEnemyDisapeared?.Invoke();
        //         }
        //     }
        // }
        //
        // private Transform GetClosestTarget()
        // {
        //     return Targets.OrderBy(target => Vector2.Distance(target.transform.position, transform.position))
        //         .FirstOrDefault();
        // }
        private void OnTriggerEnter2D(Collider2D other)
        {
            // Debug.Log(other.tag);
            if (!other.CompareTag("Player"))
            {
                return;
            }

            Targets.Add(other.transform);

            if (other.transform == enemy.CurrentTarget)
            {
                OnEnemyDetected?.Invoke();
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!other.CompareTag("Player"))
            {
                return;
            }
            Debug.Log("Exited");
            Targets.Remove(other.transform);
            // if (other.transform == enemy.CurrentTarget)
            // {
            //     var closestTarget = GetClosestTarget();
            //     if (closestTarget is not null)
            //     {
            //         Debug.Log("ShootNext");
            //         enemy.ChangeTarget(closestTarget);
            //     }
            //     else
            //     {
            //         Debug.Log("Move");
            //         enemy.ChangeToMoveToDefault();
            //         // OnEnemyDisapeared?.Invoke();
            //     }
            // }
            enemy.AttackTargetDisapeared();
        }

        public Transform GetClosestTarget()
        {
            return Targets.OrderBy(target => Vector2.Distance(target.transform.position, transform.position))
                .FirstOrDefault();
        }
    }
}