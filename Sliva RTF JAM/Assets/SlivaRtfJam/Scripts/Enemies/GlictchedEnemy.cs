using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using Pathfinding;
using SlivaRtfJam.Scripts.Guns;
using UnityEngine;
using UnityEngine.Serialization;

public class GlitchedEnemy : RangedEnemy
{
    [SerializeField] private Transform cockroachPrefab;
    [SerializeField] private Transform bugPrefab;
    [SerializeField] private Transform ratPrefab;

    [Header("Sfx")]
    [SerializeField] private List<AudioClip> morphSfxs; 

    public void TurnIntoSomething()
    {
        Destroy(gameObject);
        var enemy = Instantiate(new List<Transform> { cockroachPrefab, bugPrefab, ratPrefab }.GetRandomElement().gameObject,
            transform.position, transform.rotation);
        enemy.GetComponent<Enemy>().SetDefaultTarget(DefaultTarget);
        enemy.GetComponent<Enemy>().ChangeTarget(CurrentTarget);
        SfxManager.Instance.PlayOneShot(morphSfxs);
    }

    public override IEnumerator TurnIntoGlitch()
    {
        SfxManager.Instance.PlayOneShot(morphSfxs);
        yield break;
    }
    // protected override void Attack()
    // {
    //     animator.SetTrigger("Attack");
    //     // var direction = CurrentTarget.position - transform.position;
    //     // var projectile = Instantiate(projectilePrefab, transform.position,
    //     //     Quaternion.Euler(direction));
    //     // projectile.LaunchProjectile(direction, projectileSpeed, damage, Beat.Player);
    //     StartCoroutine(SpawnProjectile());
    //     timeToAttack = attackSpeed;
    // }

    // private IEnumerator SpawnProjectile()
    // {
    //     yield return new WaitForSeconds(0.5f);
    //     var direction = CurrentTarget.position - transform.position;
    //     var projectile = Instantiate(projectilePrefab, pointToSpawnProjectile.position,
    //         Quaternion.Euler(direction));
    //     projectile.LaunchProjectile(direction, projectileSpeed, damage, Beat.Player);
    // }
}