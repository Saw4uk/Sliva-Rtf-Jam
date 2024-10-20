using System;
using System.Collections;
using System.Collections.Generic;
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

        [Header("Sfx")]
        [SerializeField] protected List<AudioClip> shootingSfxs;
        [SerializeField] protected List<AudioClip> reloadSfxs;


        private bool isReloading;
        protected bool isShooting;
        protected float remainingShootingDelay;
        protected int currentAmmoInMag;
        protected int currentAmmoTotal;
        protected int magUpgradesAmount = 2;

        public bool IsReloading => isReloading;

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
            if(!isActiveAndEnabled || IsReloading)
            {
                return;
            }

            isShooting = context.performed;
            if (isShooting && remainingShootingDelay <= 0 && currentAmmoInMag > 0)
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
            if(shootingSfxs is { Count: > 0 })
                SfxManager.Instance.PlayOneShot(shootingSfxs);
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
            if (!isActiveAndEnabled || CurrentAmmoTotal == 0 || IsReloading)
            {
                yield break;
            }

            isReloading = true;
            gunAnimator.SetTrigger("Reload");
            if(reloadSfxs is { Count: > 0 })
                SfxManager.Instance.PlayOneShot(reloadSfxs);

            yield return new WaitForSeconds(reloadSpeedInSeconds);

            currentAmmoTotal += currentAmmoInMag;
            currentAmmoInMag = 0;
            var ammo = Math.Min(MaxAmmo, CurrentAmmoTotal);
            CurrentAmmoTotal -= ammo;
            CurrentAmmo += ammo;
            isReloading = false;
            endReloading.Invoke();
        }
    }
}