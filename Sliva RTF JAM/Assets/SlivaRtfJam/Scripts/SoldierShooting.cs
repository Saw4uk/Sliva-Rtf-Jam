using System;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using SlivaRtfJam.Scripts.Guns;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class SoldierShooting : MonoBehaviour
{
    [Header("Gun Settings")]
    [SerializeField] private Transform gunRotator;
    [SerializeField] private Gun chosedGun;
    [SerializeField] private SerializedDictionary<GunType, Gun> guns;
    private List<GunType> availableGuns;
    private GunType choosenGunType;

    public GunType ChoosenGunType => choosenGunType;

    public Gun ChosedGun
    {
        get => chosedGun;
        set
        {
            if (chosedGun != null)
            {
                chosedGun.ammoChanged.RemoveListener(OnChosedGunAmmoChanged);
                chosedGun.endReloading.RemoveListener(EndReloading);
            }
                
            chosedGun = value;
            if (chosedGun != null)
            {
                chosedGun.ammoChanged.AddListener(OnChosedGunAmmoChanged);
                chosedGun.endReloading.AddListener(EndReloading);
                AmmoChanged.Invoke(chosedGun.CurrentAmmo, chosedGun.CurrentAmmoTotal);
            }
        }
    }   

    private void OnChosedGunAmmoChanged(int arg0)
    {
        AmmoChanged.Invoke(chosedGun.CurrentAmmo,chosedGun.CurrentAmmoTotal);
    }

    public UnityEvent<int, int> AmmoChanged;
    public UnityEvent GunParametersChanged;
    public UnityEvent startReloading;
    public UnityEvent endReloading;
    private Camera camera;

    private void Awake()
    {
        camera = Camera.main;
        availableGuns = new List<GunType>();
        availableGuns.Add(GunType.Pistol);
        ChosedGun = guns[GunType.Pistol];
    }

    public void OnShoot(InputAction.CallbackContext context)
    {
        ChosedGun.OnShoot(context);
    }
    
    public void OnLook(InputAction.CallbackContext context)
    {
        var mouseScreenPosition = context.ReadValue<Vector2>();
        var mouseWordPosition = camera.ScreenToWorldPoint(mouseScreenPosition);


        var delta_x = mouseWordPosition.x - transform.position.x ;
        var delta_y = mouseWordPosition.y - transform.position.y ;
        var angle_rad = Math.Atan2(delta_y, delta_x);
        var angle_deg = Mathf.Rad2Deg * angle_rad;
        
        var rotate = gunRotator.transform.eulerAngles;
        rotate.z = (float)angle_deg;
        gunRotator.transform.rotation = Quaternion.Euler(rotate);
        var rad = gunRotator.rotation.eulerAngles.z * Mathf.Deg2Rad;
        ChosedGun.transform.localScale = Mathf.Sin(rad - 1.5f) > 0 ? new Vector3(chosedGun.transform.localScale.x, -Math.Abs(chosedGun.transform.localScale.y), chosedGun.transform.localScale.z) : new Vector3(chosedGun.transform.localScale.x, Math.Abs(chosedGun.transform.localScale.y), chosedGun.transform.localScale.z);
    }

    public void ChoseGun(GunType gunType)
    {
        if (!availableGuns.Contains(gunType) || choosenGunType == gunType || chosedGun.IsReloading)
        {
            return;
        }

        chosedGun.gameObject.SetActive(false);
        ChosedGun = guns[gunType];
        choosenGunType = gunType;
        chosedGun.gameObject.SetActive(true);
        GunParametersChanged.Invoke();
    }

    public void ChoosePistol()
    {
        ChoseGun(GunType.Pistol);
    }
    
    public void ChooseAk()
    {
        ChoseGun(GunType.Automat);
    }
    
    public void ChooseShotgun()
    {
        ChoseGun(GunType.Shotgun);
    }
    
    public void ChooseRifle()
    {
        ChoseGun(GunType.Rifle);
    }

    public void StartReload()
    {
        if (!ChosedGun.IsFullAmmo && ChosedGun.CurrentAmmoTotal > 0 && ChosedGun.IsReloading == false)
        {
            startReloading.Invoke();
            StartCoroutine(ChosedGun.Reload());
        }
    }

    private void EndReloading()
    {
        endReloading.Invoke();
    }
    
    public void UnblockGun(GunType gunType)
    {
        availableGuns.Add(gunType);
        GunParametersChanged.Invoke();
    }

    public bool CanHaveGun(GunType gunType) => availableGuns.Contains(gunType);
}