using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rigidBody;
    [SerializeField] private float speed;

    private Vector2 direction;

    public event Action onUseShop;

    private void FixedUpdate()
    {
        rigidBody.MovePosition(rigidBody.position + direction * speed * Time.fixedDeltaTime);
    }

    public void OnMovement(InputAction.CallbackContext context)
    {
        direction = context.ReadValue<Vector2>().normalized;
    }

    public void OnShopUse(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            onUseShop?.Invoke();
        }
    }
}