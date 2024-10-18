using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using Pathfinding;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using Random = DefaultNamespace.Random;

public class EnemyManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private List<Transform> spawnPoints = new();
    [SerializeField] private Enemy enemyPrefab;
    // [SerializeField] private GameObject enemy;
    [SerializeField] private List<Wave> waves = new();
    [SerializeField] private Transform target;
    [SerializeField] private float timeBetweenWaves = 20;
    public static EnemyManager Instance;
    private int currentWaveNumber;
    public UnityEvent OnWaveEnded;
    public UnityEvent OnWaveStarted;

    private void Awake()
    {
        if (Instance is not null)
        {
            Destroy(Instance);
        }

        Instance = this;
    }

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
        // yield return new WaitForSeconds(10f);
        foreach (var wave in waves)
        {
            // if (currentWaveNumber == 0)
            // {
            //     yield return new WaitForSeconds(10f);
            // }
            currentWaveNumber++;
            OnWaveStarted?.Invoke();
            yield return StartCoroutine(SpawnWave(wave));
            OnWaveEnded?.Invoke();
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
        var enemiesCount = smallWave.EnemiesCount;
        while (enemiesCount > 0)
        {
            enemiesCount--;
            SpawnEnemy(enemyPrefab);
            yield return new WaitForSeconds(smallWave.TimeBetween);
        }
    }

    [Serializable]
    private class Wave
    {
        [SerializeField] private List<SmallWave> smallWaves;
        [SerializeField] private float timeBetween = 1f;
        public List<SmallWave> SmallWaves => smallWaves;
        public float TimeBetween => timeBetween;
    }

    [Serializable]
    private class SmallWave
    {
        [SerializeField] private int enemiesCount;
        [SerializeField] private float timeBetween = 0.1f;

        public int EnemiesCount => enemiesCount;
        public float TimeBetween => timeBetween;
    }
}