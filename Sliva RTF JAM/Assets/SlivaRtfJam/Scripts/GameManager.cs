using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using SlivaRtfJam.Scripts;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Healthable enemiesTargetHealth;
    [SerializeField] private List<BorderGenerator> borderGenerators;
    [SerializeField] private AstarPath astarPath;
    private int needScanNextFrame;

    // Start is called before the first frame update
    void Start()
    {
        enemiesTargetHealth.OnDie.AddListener(GameOver);
        EnemyManager.Instance.OnWaveStarted.AddListener(StartWave);
        EnemyManager.Instance.OnWaveEnded.AddListener(EndWave);
        needScanNextFrame = 1;
        // astarPath.Scan();
    }

    // Update is called once per frame
    void Update()
    {
        if (needScanNextFrame > 0)
        {
            needScanNextFrame--;
            
            // astarPath.Scan();
            // needScanNextFrame = false;
        }
        else if (needScanNextFrame == 0)
        {
            astarPath.Scan();
            needScanNextFrame--;
        }
    }

    private void StartWave()
    {
        // astarPath.Scan();
    }

    private void EndWave()
    {
        foreach (var borderGenerator in borderGenerators)
        {
            borderGenerator.RandomGenerateNoiseBorder();
        }

        // astarPath.Scan();
        needScanNextFrame = 1;
    }

    private void GameOver()
    {
        Debug.Log("You Lost");
    }
}