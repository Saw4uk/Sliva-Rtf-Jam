using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using Pathfinding;
using UnityEngine;
using UnityEngine.Serialization;
using Random = DefaultNamespace.Random;

public class EnemyManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private List<Transform> spawnPoints = new();
    // [SerializeField] private GameObject enemy;
    [SerializeField] private List<Wave> waves = new();
    [SerializeField] private Transform target;
    [SerializeField] private float timeBetweenWaves = 20;

    private void Start()
    {
        StartCoroutine(SpawnWaves());
    }

    private void SpawnEnemy(Enemy enemy)
    {
        var spawnPoint = spawnPoints.GetRandomElement();
        var enemyObject = Instantiate(enemy, spawnPoint.position, spawnPoint.rotation);
        enemyObject.SetDefaultTarget(target);
        enemyObject.ChangeTarget(target);
        // enemyObject.ChangeTargetToDefault();
    }

    private IEnumerator SpawnWaves()
    {
        foreach (var wave in waves)
        {
            yield return StartCoroutine(SpawnWave(wave));
            yield return new WaitForSeconds(timeBetweenWaves);
        }
    }

    private IEnumerator SpawnWave(Wave wave)
    {
        foreach (var smallWave in wave.SmallWaves)
        {
            yield return StartCoroutine(SpawnSmallWave(smallWave));
            yield return new WaitForSeconds(wave.TimeBetween);
        }
    }

    private IEnumerator SpawnSmallWave(SmallWave smallWave)
    {
        var enemies = smallWave.Enemies;
        while (enemies.Any())
        {
            var enemy = enemies.GetRandomElement();
            SpawnEnemy(enemy);
            enemies.Remove(enemy);
            yield return new WaitForSeconds(smallWave.TimeBetween);
        }
    }

    [Serializable]
    private class Wave
    {
        [FormerlySerializedAs("waves")] [SerializeField]
        private List<SmallWave> smallWaves;
        [SerializeField] private float timeBetween = 2f;
        public List<SmallWave> SmallWaves => smallWaves;
        public float TimeBetween => timeBetween;
    }
    [Serializable]
    private class SmallWave
    {
        [SerializeField] private List<Enemy> enemies;
        [SerializeField] private float timeBetween = 0.3f;

        public List<Enemy> Enemies => enemies;
        public float TimeBetween => timeBetween;
    }
}