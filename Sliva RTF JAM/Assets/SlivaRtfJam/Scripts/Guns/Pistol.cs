using UnityEngine;
using UnityEngine.InputSystem;

namespace SlivaRtfJam.Scripts.Guns
{
    public class Pistol : Gun
    {
        [SerializeField] private bool isAutomatigShooting;

        private bool isShootPerformed;
        private bool isShootCanceled;

        protected override void Update()
        {
            if (remainingShootingDelay >= 0)
            {
                remainingShootingDelay -= Time.deltaTime;
            }
            
            if (isShooting)
            {
                if (remainingShootingDelay <= 0 && currentAmmoInMag > 0 && isAutomatigShooting)
                {
                    MakeShoot();
                }
            }
        }

        public override void OnShoot(InputAction.CallbackContext context)
        {
            if(!isActiveAndEnabled)
            {
                return;
            }

            isShooting = context.performed;
            
            if (isShooting && remainingShootingDelay <= 0 && currentAmmoInMag > 0 && !isAutomatigShooting)
            {
                MakeShoot();
            }
        }
    }
}