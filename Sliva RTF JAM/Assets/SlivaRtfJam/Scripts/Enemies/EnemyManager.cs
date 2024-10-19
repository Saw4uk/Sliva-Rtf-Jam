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
    public bool IsAllEnemiesSpawned;
    public int EnemiesCount;

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
        StartCoroutine(SpawnWave(waves[0]));
    }

    private void SpawnEnemy(Enemy enemy)
    {
        var spawnPoint = spawnPoints.GetRandomElement();
        var enemyObject = Instantiate(enemy, spawnPoint.position, spawnPoint.rotation);
        enemyObject.SetDefaultTarget(target);
        enemyObject.ChangeTarget(target);
        EnemiesCount += 1;
        // enemyObject.ChangeTargetToDefault();
    }

    // private IEnumerator SpawnWaves()
    // {
    //     // yield return new WaitForSeconds(10f);
    //     foreach (var wave in waves)
    //     {
    //         IsAllEnemiesSpawned = false;
    //         // if (currentWaveNumber == 0)
    //         // {
    //         //     yield return new WaitForSeconds(10f);
    //         // }
    //         currentWaveNumber++;
    //         OnWaveStarted?.Invoke();
    //         yield return StartCoroutine(SpawnWave(wave));
    //         OnWaveEnded?.Invoke();
    //         IsAllEnemiesSpawned = true;
    //         yield return new WaitForSeconds(timeBetweenWaves);
    //     }
    // }

    public IEnumerator SpawnNextWave()
    {
        yield return new WaitForSeconds(timeBetweenWaves);
        StartCoroutine(SpawnWave(waves[currentWaveNumber]));
    }

    private IEnumerator SpawnWave(Wave wave)
    {
        IsAllEnemiesSpawned = false;
        currentWaveNumber++;
        OnWaveStarted?.Invoke();
        foreach (var smallWave in wave.SmallWaves)
        {
            yield return new WaitForSeconds(wave.TimeBetween);
            yield return StartCoroutine(SpawnSmallWave(smallWave));
        }

        IsAllEnemiesSpawned = true;
        // OnWaveEnded?.Invoke();
    }

    private IEnumerator SpawnSmallWave(SmallWave smallWave)
    {
        var enemiesCount = smallWave.EnemiesCount;
        while (enemiesCount > 0)
        {
            enemiesCount--;
            SpawnEnemy(enemyPrefab);
            if (enemiesCount == 0)
            {
                yield return new WaitForSeconds(smallWave.TimeBetween);
            }
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