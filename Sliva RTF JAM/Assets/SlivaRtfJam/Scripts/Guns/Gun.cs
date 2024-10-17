using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

namespace SlivaRtfJam.Scripts.Guns
{
    public abstract class Gun : MonoBehaviour
    {
        //[SerializeField] private AudioClip shootSound;
        //[SerializeField] private AudioSource audioSource;

        [Header("Gun Settings")] 
        //[SerializeField] private Transform gunRotator;

        //[SerializeField] private Animator gunAnimator;
        [SerializeField] protected Transform barrel;

        [SerializeField] protected ShootAnimator shootAnimator;
        [SerializeField] protected Animator gunAnimator;

        [Header("Shoot Settings")] [SerializeField]
        protected Projectile projectilePrefab;
        [SerializeField] protected float projectileSpeed;

        
        [Header("Model Settings")]
        [SerializeField] protected float shootingDelay;
        [SerializeField] protected float bulletDamageStart;
        [SerializeField] protected float bulletDamageEnd;
        [SerializeField] protected float bulletDegreeRandomDegrees;
        [SerializeField] protected int maxAmmo;
        [SerializeField] protected float reloadSpeedInSeconds;


        protected bool isShooting;
        protected float remainingShootingDelay;
        protected int currentAmmoInMag;
        protected int currentAmmoTotal;

        
        public UnityEvent<int> ammoChanged;
        public UnityEvent startReloading;

        public int CurrentAmmo
        {
            set
            {
                currentAmmoInMag = value;
                ammoChanged.Invoke(currentAmmoInMag);
            }
            get => currentAmmoInMag;
        }

        public int CurrentAmmoTotal
        {
            get => currentAmmoTotal;
            set => currentAmmoTotal = value;
        }

        public int MaxAmmo
        {
            get => maxAmmo;
            set
            {
                maxAmmo = value;
                ammoChanged.Invoke(maxAmmo);
            }
        }

        public bool IsFullAmmo => CurrentAmmo == maxAmmo;


        private void Awake()
        {
            remainingShootingDelay = 0;
            CurrentAmmo = maxAmmo;
        }

        private void FixedUpdate()
        {
            remainingShootingDelay -= Time.fixedDeltaTime;
        }

        public virtual void OnShoot(InputAction.CallbackContext context)
        {
            if(!isActiveAndEnabled) return;
            isShooting = context.performed;
            if (isShooting && remainingShootingDelay <= 0 && currentAmmoInMag > 0)
            {
                
                var gunTransform = transform;
                var rads = (gunTransform.rotation.eulerAngles.z + Random.Range(-bulletDegreeRandomDegrees, + bulletDegreeRandomDegrees)) * Mathf.Deg2Rad;
                var direction = new Vector3(Mathf.Cos(rads), Mathf.Sin(rads), 0);
                Shoot(direction, barrel.position);
                shootAnimator.Animate(remainingShootingDelay);
                gunAnimator.SetTrigger("Shoot");
            }
        }
        

        protected virtual void Shoot(Vector2 direction, Vector3 shootPosition)
        {
            if(!isActiveAndEnabled) return;
            
            var bullet = Instantiate(projectilePrefab, shootPosition, transform.rotation);
            bullet.LaunchProjectile(direction, projectileSpeed,Random.Range(bulletDamageStart,bulletDamageEnd));

            CurrentAmmo -= 1;
            remainingShootingDelay = shootingDelay;

            //gunAnimator.SetTrigger("Fire");
            //audioSource.PlayOneShot(shootSound);
        }

        public virtual void AddAmmo(int count)
        {
            CurrentAmmoTotal += Mathf.Clamp(count, 0, maxAmmo);
        }

        public IEnumerator Reload()
        {
            if (!isActiveAndEnabled || CurrentAmmoTotal == 0)
                yield break;
            yield return new WaitForSeconds(reloadSpeedInSeconds);
            var ammo = Math.Min(MaxAmmo, CurrentAmmoTotal);
            CurrentAmmoTotal -= ammo;
            CurrentAmmo += ammo;
        }
    }
}