using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace DefaultNamespace
{
    public class EnemyVisionDetector : MonoBehaviour
    {
        [SerializeField] private Enemy enemy;
        public UnityEvent<Transform> OnEnemyDetected;
        public UnityEvent OnEnemyDisapeared;

        private List<Transform> targets = new();


        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player"))
            {
                return;
            }

            targets.Add(other.transform);
            Debug.Log(targets[0]);
            var theClosestTarget =
                targets.OrderBy(target => Vector2.Distance(target.transform.position, transform.position)).First();
            Debug.Log(theClosestTarget);
            OnEnemyDetected?.Invoke(theClosestTarget);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!other.CompareTag("Player"))
            {
                return;
            }

            targets.Remove(other.transform);
            if (other.transform == enemy.CurrentTarget)
            {
                if (targets.Count > 0)
                {
                    var theClosestTarget =
                        targets.OrderBy(target => Vector2.Distance(target.transform.position, transform.position))
                            .First();
                    OnEnemyDetected?.Invoke(theClosestTarget);
                }
                else
                {
                    OnEnemyDisapeared?.Invoke();
                }
            }
        }
    }
}