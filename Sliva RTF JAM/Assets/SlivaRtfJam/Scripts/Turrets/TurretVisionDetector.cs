using System;
using System.Collections.Generic;
using System.Linq;
using SlivaRtfJam.Scripts;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

namespace DefaultNamespace
{
    public class TurretVisionDetector : MonoBehaviour
    {
        [SerializeField] private Turret turret;
        
        private List<Transform> targets = new();
        
        public UnityEvent<Transform> OnTargetChanged;

        private void Update()
        {
            var theClosestTarget = GetClosestTarget();
            
            if (theClosestTarget != turret.CurrentTarget)
            {
                OnTargetChanged?.Invoke(theClosestTarget);
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Enemy"))
            {
                return;
            }

            if (!targets.Contains(other.transform))
            {
                targets.Add(other.transform);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!other.CompareTag("Enemy"))
            {
                return;
            }
            
            targets.Remove(other.transform);
        }

        private Transform GetClosestTarget()
        {
            return targets.OrderBy(target => Vector2.Distance(target.transform.position, transform.position)).FirstOrDefault();
        }
    }
}