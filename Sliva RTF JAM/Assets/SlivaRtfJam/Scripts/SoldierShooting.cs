using System;
using SlivaRtfJam.Scripts.Guns;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class SoldierShooting : MonoBehaviour
{
    [Header("Gun Settings")]
    [SerializeField] private Transform gunRotator;
    [SerializeField] private Gun gun;
    [SerializeField] private SpriteRenderer gunSpriteRenderer;
    
    private Camera camera;

    private void Awake()
    {
        camera = Camera.main;
    }

    public void OnShoot(InputAction.CallbackContext context)
    {
        gun.OnShoot(context);
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
        gun.transform.localScale = Mathf.Sin(rad - 1.5f) > 0 ? new Vector3(gun.transform.localScale.x, -Math.Abs(gun.transform.localScale.y), gun.transform.localScale.z) : new Vector3(gun.transform.localScale.x, Math.Abs(gun.transform.localScale.y), gun.transform.localScale.z);
        
    }
}