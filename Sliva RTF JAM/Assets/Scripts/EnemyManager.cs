using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using Pathfinding;
using UnityEngine;
using Random = DefaultNamespace.Random;

public class EnemyManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private List<Transform> spawnPoints = new();
    // [SerializeField] private GameObject enemy;
    [SerializeField] private List<Wave> waves = new();
    [SerializeField] private Transform target;
    private int currentWaveIndex = 0;
    [SerializeField] private float timeToNextWave = 0;
    [SerializeField] private float timeBetweenWaves = 20;

    private void Start()
    {
        // var wave = waves[0];
        // spawnNow = true;
        // var allEnemies = wave.GetAllEnemies();
        // StartCoroutine(SpawnRandomEnemy(allEnemies, 2f));
    }

    void Update()
    {
        timeToNextWave -= Time.deltaTime;
        if (timeToNextWave <= 0)
        {
            timeToNextWave = timeBetweenWaves;
            StartCoroutine(SpawnWave());
        }
    }

    private void SpawnEnemy(Enemy enemy)
    {
        var spawnPoint = spawnPoints.GetRandomElement();
        // enemy.SetDefaultTarget(target);
        // enemy.ChangeTargetToDefault();
        var enemyObject = Instantiate(enemy, spawnPoint.position, spawnPoint.rotation);
        enemyObject.SetDefaultTarget(target);
        enemyObject.ChangeTargetToDefault();
    }

    private IEnumerator SpawnWave()
    {
        var enemies = waves[currentWaveIndex].GetAllEnemies();
        Debug.Log(enemies.Count);
        while (enemies.Any())
        {
            var enemy = enemies.GetRandomElement();
            SpawnEnemy(enemy);
            enemies.Remove(enemy);
            yield return new WaitForSeconds(waves[currentWaveIndex].TimeToNextEnemy);
        }
        // foreach (var enemy in enemies)
        // {
        //     SpawnEnemy(enemy);
        //     yield return new WaitForSeconds(waves[currentWaveIndex].TimeToNextEnemy);
        // }

        currentWaveIndex++;
    }

    [Serializable]
    private class Wave
    {
        [SerializeField] private List<WaveEnemy> waveEnemy;
        [SerializeField] private float timeToNextEnemy = 1;
        public float TimeToNextEnemy => timeToNextEnemy;


        // public Dictionary<Enemy, int> GetAllEnemies()
        // {
        //     var result = new Dictionary<Enemy, int>();
        //     waveEnemy.ForEach(enemy => result.Add(enemy.Enemy, enemy.Count));
        //     return result;
        // }
        public List<Enemy> GetAllEnemies()
        {
            var result = new List<Enemy>();
            foreach (var enemy in waveEnemy)
            {
                for (var i = 0; i < enemy.Count; i++)
                {
                    result.Add(enemy.Enemy);
                }
            }

            return result;
        }
    }

    [Serializable]
    private class WaveEnemy
    {
        [SerializeField] private Enemy enemy;
        [SerializeField] private int count;

        public Enemy Enemy => enemy;
        public int Count => count;
    }
}