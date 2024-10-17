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
        [SerializeField] private Transform barrel;
        [SerializeField] private Transform centerTransform;
        [SerializeField] private SpriteRenderer gunSpriteRenderer;

        [Header("Shoot Settings")] [SerializeField]
        private Projectile projectilePrefab;
        [SerializeField] private float projectileSpeed;

        
        [Header("Model Settings")]
        [SerializeField] private float shootingDelay;
        [SerializeField] private float bulletDamage;
        [SerializeField] private float bulletDegreeRandomRadian;
        [SerializeField] private int maxAmmo;
        [SerializeField] private float reloadSpeedInSeconds;


        private bool isShooting;
        private float remainingShootingDelay;
        private int currentAmmoInMag;
        private int currentAmmoTotal;

        
        public UnityEvent<int> ammoChanged;
        public UnityEvent<int> maxAmmoChanged;
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
                maxAmmoChanged.Invoke(maxAmmo);
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
            isShooting = context.performed;
            if (isShooting && remainingShootingDelay <= 0 && currentAmmoInMag > 0)
            {
                var gunTransform = transform;
                var rads = (gunTransform.rotation.eulerAngles.z) * Mathf.Deg2Rad;
                var direction = new Vector3(Mathf.Cos(rads+Random.Range(-bulletDegreeRandomRadian, +bulletDegreeRandomRadian)), Mathf.Sin(rads + Random.Range(-bulletDegreeRandomRadian, +bulletDegreeRandomRadian)), 0);
                Shoot(direction, barrel.position);
            }
        }
        

        protected virtual void Shoot(Vector2 direction, Vector3 shootPosition)
        {
            var bullet = Instantiate(projectilePrefab, shootPosition, transform.rotation);
            bullet.LaunchProjectile(direction, projectileSpeed,bulletDamage);

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
            if (CurrentAmmoTotal == 0)
                yield break;
            yield return new WaitForSeconds(reloadSpeedInSeconds);
            var ammo = Math.Min(MaxAmmo, CurrentAmmoTotal);
            CurrentAmmoTotal -= ammo;
            CurrentAmmo += ammo;
        }
    }
}