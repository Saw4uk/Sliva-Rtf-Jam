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
        private bool isReloading;
        protected float remainingShootingDelay;
        protected int currentAmmoInMag;
        protected int currentAmmoTotal;
        protected int magUpgradesAmount = 2;


        public UnityEvent<int> ammoChanged;
        public UnityEvent endReloading;

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
            set
            {
                currentAmmoTotal = value;
                ammoChanged.Invoke(currentAmmoInMag);
            }
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

        public bool IsReloading => isReloading;

        private void Awake()
        {
            remainingShootingDelay = 0;
            CurrentAmmo = maxAmmo;
            CurrentAmmoTotal = CurrentAmmo * magUpgradesAmount;
        }

        protected virtual void Update()
        {
            if (remainingShootingDelay >= 0)
            {
                remainingShootingDelay -= Time.deltaTime;
            }
        }

        public virtual void OnShoot(InputAction.CallbackContext context)
        {
            if(!isActiveAndEnabled)
            {
                return;
            }

            isShooting = context.performed;
            if (isShooting && remainingShootingDelay <= 0 && currentAmmoInMag > 0 && !IsReloading)
            {
                MakeShoot();
            }
        }

        protected virtual void MakeShoot()
        {
            if(IsReloading) return;
            var gunTransform = transform;
            var rads = (gunTransform.rotation.eulerAngles.z + Random.Range(-bulletDegreeRandomDegrees, + bulletDegreeRandomDegrees)) * Mathf.Deg2Rad;
            var direction = new Vector3(Mathf.Cos(rads), Mathf.Sin(rads), 0);
            Shoot(direction, barrel.position);
            remainingShootingDelay = shootingDelay;
            shootAnimator.Animate(remainingShootingDelay);
            gunAnimator.SetTrigger("Shoot");
        }
        

        protected virtual void Shoot(Vector2 direction, Vector3 shootPosition)
        {
            if(!isActiveAndEnabled || IsReloading)
            {
                return;
            }

            var bullet = Instantiate(projectilePrefab, shootPosition, transform.rotation);
            bullet.LaunchProjectile(direction, projectileSpeed,Random.Range(bulletDamageStart,bulletDamageEnd));

            CurrentAmmo -= 1;

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
            {
                yield break;
            }
            isReloading = true;
            gunAnimator.SetTrigger("Reload");
            yield return new WaitForSeconds(reloadSpeedInSeconds);
            CurrentAmmoTotal += currentAmmoInMag;
            currentAmmoInMag = 0;
            var ammo = Math.Min(MaxAmmo, CurrentAmmoTotal);
            CurrentAmmoTotal -= ammo;
            CurrentAmmo += ammo;
            endReloading.Invoke();
            isReloading = false;
        }
    }
}