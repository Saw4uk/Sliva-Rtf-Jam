using System;
using UnityEngine;
using UnityEngine.Events;

namespace DefaultNamespace
{
    public class EnemyAttackDetector : MonoBehaviour
    {
        [SerializeField] private Enemy enemy;
        public UnityEvent OnEnemyDetected;
        public UnityEvent OnEnemyDisapeared;

        private void OnTriggerEnter2D(Collider2D other)
        {
            Debug.Log(other.tag);
            if (!other.CompareTag("Player"))
            {
                return;
            }
            if (other.transform == enemy.CurrentTarget)
            {
                Debug.Log(other);
                OnEnemyDetected?.Invoke();
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!other.CompareTag("Player"))
            {
                return;
            }
            if (other.transform == enemy.CurrentTarget)
            {
                OnEnemyDisapeared?.Invoke();
            }
        }
    }
}