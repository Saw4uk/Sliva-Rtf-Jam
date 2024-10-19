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
    [SerializeField] private Transform pointToSpawnProjectile;

    protected override void Attack()
    {
        animator.SetTrigger("Attack");
        // var direction = CurrentTarget.position - transform.position;
        // var projectile = Instantiate(projectilePrefab, transform.position,
        //     Quaternion.Euler(direction));
        // projectile.LaunchProjectile(direction, projectileSpeed, damage, Beat.Player);
        StartCoroutine(SpawnProjectile());
        timeToAttack = attackSpeed;
    }

    private IEnumerator SpawnProjectile()
    {
        yield return new WaitForSeconds(0.33f);
        var direction = CurrentTarget.position - pointToSpawnProjectile.position;
        var projectile = Instantiate(projectilePrefab, pointToSpawnProjectile.position,
            Quaternion.Euler(direction));
        projectile.LaunchProjectile(direction, projectileSpeed, damage, Beat.Player);
    }
}