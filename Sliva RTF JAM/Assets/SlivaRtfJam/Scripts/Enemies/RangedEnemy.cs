using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using Pathfinding;
using SlivaRtfJam.Scripts.Guns;
using UnityEngine;
using UnityEngine.Serialization;

public class RangedEnemy : Enemy
{
    [SerializeField] private Projectile projectilePrefab;
    [SerializeField] private float projectileSpeed;

    protected override void Attack()
    {
        var direction = CurrentTarget.position - transform.position;
        var projectile = Instantiate(projectilePrefab, transform.position,
            Quaternion.Euler(direction));
        projectile.LaunchProjectile(direction, projectileSpeed, damage, Beat.Player);
        timeToAttack = attackSpeed;
    }
}