using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rigidBody;
    [SerializeField] private float speed;
    [SerializeField] private Animator animator;
    private bool isBlocked;
    private Vector2 direction;

    public bool IsBlocked
    {
        get => isBlocked;
        set => isBlocked = value;
    }

    public event Action<GameObject> OnUseShop;

    private void Update()
    {
        if(isBlocked) return;
        animator.SetFloat("Horizontal", direction.x);
        animator.SetFloat("Vertical", direction.y);
        animator.SetFloat("Speed",direction.magnitude);
    }

    private void FixedUpdate()
    {
        if(isBlocked) return;
        rigidBody.MovePosition(rigidBody.position + direction * speed * Time.fixedDeltaTime);
    }

    public void OnMovement(InputAction.CallbackContext context)
    {
        if(isBlocked) return;
        direction = context.ReadValue<Vector2>().normalized;
    }

    public void OnShopUse(InputAction.CallbackContext context)
    {
        if(isBlocked) return;
        if (context.performed)
        {
            OnUseShop?.Invoke(gameObject);
        }
    }
}