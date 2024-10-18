using System;
using DefaultNamespace;
using SlivaRtfJam.Scripts.Guns;
using UnityEngine;
using UnityEngine.Serialization;

namespace SlivaRtfJam.Scripts
{
    public class Turret : MonoBehaviour
    {
        [SerializeField] private Transform gunRotator;
        [SerializeField] private TurretVisionDetector turretVisionDetector;
        
        [Header("Projectile Settings")]
        [SerializeField] protected Projectile projectilePrefab;
        
        [Header("Automatic Settings")]
        [SerializeField] protected float automaticProjectileSpeed;
        [SerializeField] protected float automaticBulletDamage;
        [SerializeField] protected float automaticShootingDelay;
        
        // [Header("Controlled Settings")]
        // [SerializeField] protected float projectileSpeed;
        // [SerializeField] protected float bulletDamage;
        // [SerializeField] protected float shootingDelay;

        private State state = State.Automatic;
        private Transform currentTarget;
        private float remainingShootingDelay;

        public Transform CurrentTarget => currentTarget;

        private void Awake()
        {
            turretVisionDetector.OnTargetChanged.AddListener(ChangeTarget);
        }

        private void Update()
        {
            if (remainingShootingDelay >= 0)
            {
                remainingShootingDelay -= Time.deltaTime;
            }

            switch (state)
            {
                case State.Automatic:
                {
                    AutomaticAttack();
                    break;
                }
                case State.Controlled:
                {
                    ControlledAttack();
                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void ChangeTarget(Transform target)
        {
            currentTarget = target;
        }

        private void AutomaticAttack()
        {
            if (currentTarget is not null)
            {
                RotateToPoint(currentTarget.position);
                
                if (remainingShootingDelay <= 0)
                {
                    var targetPos = new Vector2(currentTarget.position.x, currentTarget.position.y);
                    var pos = new Vector2(transform.position.x, transform.position.y);
                    var direction = (targetPos - pos).normalized;

                    Shoot(direction, transform.position);
                }
            }
        }

        private void ControlledAttack()
        {
            Debug.Log("ControlledAttack");
        }

        protected virtual void Shoot(Vector2 direction, Vector3 shootPosition)
        {
            var bullet = Instantiate(projectilePrefab, shootPosition, Quaternion.Euler(direction));
            bullet.LaunchProjectile(direction, automaticProjectileSpeed, automaticBulletDamage);

            remainingShootingDelay = automaticShootingDelay;
        }

        private void RotateToPoint(Vector3 targetPos)
        {
            var delta_x = targetPos.x - transform.position.x;
            var delta_y = targetPos.y - transform.position.y;
            var angle_rad = Math.Atan2(delta_y, delta_x);
            var angle_deg = Mathf.Rad2Deg * angle_rad;

            var rotate = gunRotator.transform.eulerAngles;
            rotate.z = (float)angle_deg;
            gunRotator.transform.rotation = Quaternion.Euler(rotate);
        }

        private enum State
        {
            Automatic,
            Controlled
        }
    }
}