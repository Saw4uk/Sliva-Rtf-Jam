using System;
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
            if (other.transform == enemy.CurrentTarget)
            {
                var theClosestTarget = GetClosestTarget();
                OnTargetChanged?.Invoke(theClosestTarget);
            }
        }

        private Transform GetClosestTarget()
        {
            var a =targets.Concat(new List<Transform> { enemy.DefaultTarget })
                .OrderBy(target => Vector2.Distance(target.transform.position, transform.position)).First();
            Debug.Log(a);
            return targets.Concat(new List<Transform> { enemy.DefaultTarget })
                .OrderBy(target => Vector2.Distance(target.transform.position, transform.position)).First();
        }
    }
}