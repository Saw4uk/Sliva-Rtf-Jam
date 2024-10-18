using UnityEngine;
using UnityEngine.InputSystem;

namespace SlivaRtfJam.Scripts.Guns
{
    public class Shotgun : Gun
    {
        [SerializeField] private int shotsAmount;
        protected override void Shoot(Vector2 direction, Vector3 shootPosition)
        {
            if(!isActiveAndEnabled)
            {
                return;
            }

            var bullet = Instantiate(projectilePrefab, shootPosition, transform.rotation);
            bullet.LaunchProjectile(direction, projectileSpeed,Random.Range(bulletDamageStart,bulletDamageEnd));
        }

        public override void OnShoot(InputAction.CallbackContext context)
        {
            if(!isActiveAndEnabled)
            {
                return;
            }

            isShooting = context.performed;
            if (isShooting && remainingShootingDelay <= 0 && currentAmmoInMag > 0)
            {
                MakeShoot();
            }
        }

        protected override void MakeShoot()
        {
            var gunTransform = transform;
            for (int s = 0; s < shotsAmount; s++)
            {
                var rads = (gunTransform.rotation.eulerAngles.z + Random.Range(-bulletDegreeRandomDegrees, + bulletDegreeRandomDegrees)) * Mathf.Deg2Rad;
                var direction = new Vector3(Mathf.Cos(rads), Mathf.Sin(rads), 0);
                Shoot(direction, barrel.position);
            }
            
            remainingShootingDelay = shootingDelay;
            shootAnimator.Animate(remainingShootingDelay);
            gunAnimator.SetTrigger("Shoot");
            CurrentAmmo -= 1;
        }
    }
}