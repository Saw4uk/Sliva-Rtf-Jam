using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootAnimator : MonoBehaviour
{
    [SerializeField] private Animator animator;

    public void Animate(float animationTimeInSeconds)
    {
        animator.SetTrigger("Shoot");
    }
}
