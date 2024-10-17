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

        gunRotator.transform.rotation = Quaternion.LookRotation(
            Vector3.forward,
            mouseWordPosition - transform.position
        );

        var rad = gunRotator.rotation.eulerAngles.z * Mathf.Deg2Rad;
        gunSpriteRenderer.flipY = Mathf.Sin(rad) > 0;
    }
}