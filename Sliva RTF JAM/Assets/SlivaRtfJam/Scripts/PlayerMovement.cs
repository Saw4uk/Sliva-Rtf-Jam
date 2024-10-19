using System;
using System.Collections;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rigidBody;
    [SerializeField] private float speed;
    [SerializeField] private Animator animator;
    private bool isBlocked;
    public bool isBlockedMovement { get; set; }
    private Vector2 direction;
    private bool isDied;

    public bool IsBlocked
    {
        get => isBlocked;
        set => isBlocked = value;
    }

    private void Start()
    {
        GameManager.Instance.OnWaveEnded.AddListener(TryRevive);
    }

    public event Action<GameObject> OnUseShop;

    private void Update()
    {
        if (isBlocked) return;
        animator.SetFloat("Horizontal", direction.x);
        animator.SetFloat("Vertical", direction.y);
        animator.SetFloat("Speed", direction.magnitude);
    }

    private void FixedUpdate()
    {
        if (isBlocked) return;
        rigidBody.MovePosition(rigidBody.position + direction * speed * Time.fixedDeltaTime);
    }

    public void OnMovement(InputAction.CallbackContext context)
    {
        if (isBlocked) return;
        if (isBlockedMovement) return;
        direction = context.ReadValue<Vector2>().normalized;
    }

    public void OnShopUse(InputAction.CallbackContext context)
    {
        if (isBlocked) return;
        if (context.performed)
        {
            OnUseShop?.Invoke(gameObject);
        }
    }

    public void Die()
    {
        animator.SetTrigger("Die");
        IsBlocked = true;
        StartCoroutine(Disappear());
    }

    public IEnumerator Disappear()
    {
        yield return new WaitForSeconds(2f);
        isDied = true;
        gameObject.SetActive(false);
    }

    public void TryRevive()
    {
        if (!isDied)
        {
            return;
        }

        isDied = false;
        GetComponent<Healthable>().RestoreHalfHP();
        Release();
        gameObject.SetActive(true);
    }

    public void Release()
    {
        IsBlocked = false;
    }
}