using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace DefaultNamespace
{
    public class EnemyVisionDetector : MonoBehaviour
    {
        [SerializeField] private Enemy enemy;
        public UnityEvent<Transform> OnTargetChanged;

        private List<Transform> targets = new();


        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player"))
            {
                return;
            }

            targets.Add(other.transform);
            // StartCoroutine(ChangeTarget());
            var theClosestTarget = GetClosestTarget();
            OnTargetChanged?.Invoke(theClosestTarget);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!other.CompareTag("Player"))
            {
                return;
            }

            targets.Remove(other.transform);
            // if (other.transform == enemy.CurrentTarget)
            // {
            //     // StartCoroutine(ChangeTarget());
            //     var theClosestTarget = GetClosestTarget();
            //     if (enemy.CurrentState is Enemy.State.Attack)
            //     {
            //         
            //     }
            //     else
            //     {
            //         OnTargetChanged?.Invoke(theClosestTarget);
            //     }
            //
            //     
            // }
            enemy.MoveTargetDisapeared();
        }

        public Transform GetClosestTarget()
        {
            return targets.Concat(new List<Transform> { enemy.DefaultTarget })
                .OrderBy(target => Vector2.Distance(target.transform.position, transform.position)).First();
        }

        private IEnumerator ChangeTarget()
        {
            yield return new WaitForSeconds(1);
            var theClosestTarget = GetClosestTarget();
            OnTargetChanged?.Invoke(theClosestTarget);
        }
    }
}