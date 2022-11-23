using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] Transform[] spawnPoints;
    [SerializeField] WaveSO[] easyWaves;
    [SerializeField] WaveSO[] middleWaves;
    [SerializeField] WaveSO[] hardWaves;

    // Ships Types
    [SerializeField] GameObject fighter;
    [SerializeField] GameObject corvette;
    [SerializeField] GameObject cruiser;
    [SerializeField] GameObject destroyer;
    [SerializeField] GameObject harpoon;
    [SerializeField] GameObject nukeBomber;

    [SerializeField] GameObject portal;

    Transform spawnPointSelected;
    Transform enemyShipsPool;
    Transform portalsPool;
    Transform targetPlanet;
    WarningDialog warningDialog;
    GameManager gm;

    public enum threatLevel { Easy, Middle, High }

    threatLevel currentThreatLevel;

    List<WaveSO> wavesInGame;

    int wavesGroup = 1;

    int waveCounter = 0;
    public int WaveCounter { get => waveCounter; }

    int wavesToMiddleLevel = 6;
    int wavesToHardLevel = 12;

    int waveIncrement = 0;

    float delaySpawnPortal = 15f;
    float delaySpawnFirstShip = 4f;
    float timeBetweenShips = 2f;
    float timeBetweenWaves = 3f;
    float timeNextLevel = 25f;

    float minPosDeviation = -6f;
    float maxPosDeviation = 6f;

    void Awake()
    {
        enemyShipsPool = GameObject.FindGameObjectWithTag("EnemyShipsPool").transform;
        portalsPool = GameObject.FindGameObjectWithTag("PortalsPool").transform;
        targetPlanet = GameObject.FindGameObjectWithTag("PlanetBase").transform;
        warningDialog = GameObject.FindGameObjectWithTag("WarningDialog").GetComponent<WarningDialog>();
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    void Start()
    {
        currentThreatLevel = threatLevel.Easy;

        Invoke("InitiateSpawnProcess", delaySpawnPortal);
    }

    void CreateListOfWavesInGame(WaveSO[] array)
    {
        wavesInGame = new List<WaveSO>(wavesGroup);

        for (int i = 0; i < wavesGroup; i++)
        {
            wavesInGame.Add(array[Random.Range(0, array.Length)]);
        }
    }

    void InitiateSpawnProcess()
    {
        if (gm.IsGameOver) return;

        int waveNumber = waveCounter + 1;

        warningDialog.ActivateWarningDialog(GetThreatLevel(), waveNumber.ToString());

        CreateListOfWavesInGame(GetWavesLevel());
        StartCoroutine(SpawnWaves());
    }

    IEnumerator SpawnWaves()
    {   
        SelectSpawnSpoint();

        OpenPortal();

        yield return new WaitForSeconds(delaySpawnFirstShip);

        foreach (var wave in wavesInGame)
        {
            for (int i = 0; i < wave.enemyAmount; i++)
            {
                SpawnShip(wave);

                yield return new WaitForSeconds(timeBetweenShips);
            }

            yield return new WaitForSeconds(timeBetweenWaves);
        }

        Destroy(portalsPool.GetChild(0).gameObject);

        yield return new WaitForSeconds(timeNextLevel);

        NextWaveLevel();
    }

    void SelectSpawnSpoint()
    {
        spawnPointSelected = spawnPoints[Random.Range(0, spawnPoints.Length)];
    }

    void OpenPortal()
    {
        Instantiate(portal, spawnPointSelected.position,
            Quaternion.AngleAxis(GetAngleFromVectorFloat(targetPlanet.transform.position
            - spawnPointSelected.position)
            - 180f, Vector3.forward),
            portalsPool
            );
    }

    void SpawnShip(WaveSO wave)
    {
        Instantiate(GetShipType(wave.enemyType),
                spawnPointSelected.localPosition + new Vector3(-2, Random.Range(minPosDeviation, maxPosDeviation), 0f),
                Quaternion.identity,
                enemyShipsPool);
    }

    void NextWaveLevel()
    {
        waveCounter++;

        if (waveCounter == wavesToMiddleLevel)
        {
            currentThreatLevel = threatLevel.Middle;
        }
        else if (waveCounter == wavesToHardLevel)
        {
            currentThreatLevel = threatLevel.High;
        }

        IncrementWavesGroup();

        InitiateSpawnProcess();
    }

    void IncrementWavesGroup()
    {
        waveIncrement++;

        if (waveIncrement % 2 == 0)
        {
            wavesGroup++;
        }
    }

    GameObject GetShipType(EnemyType enemyType)
    {
        switch(enemyType)
        {
            case EnemyType.Fighter:
                return fighter;
            case EnemyType.Corvette:
                return corvette;
            case EnemyType.Cruiser:
                return cruiser;
            case EnemyType.Destroyer:
                return destroyer;
            case EnemyType.Harpoon:
                return harpoon;
            case EnemyType.NukeBomber:
                return nukeBomber;
            default:
                return null;
        }
    }

    string GetThreatLevel()
    {
        switch (currentThreatLevel)
        {
            case threatLevel.Easy:
            return "LOW";
            case threatLevel.Middle:
            return "MIDDLE";
            case threatLevel.High:
            return "HIGH";
            default:
            return "NO";
        }
    }

    WaveSO[] GetWavesLevel()
    {
        switch (currentThreatLevel)
        {
            case threatLevel.Easy:
            return easyWaves;
            case threatLevel.Middle:
            return middleWaves;
            case threatLevel.High:
            return hardWaves;
            default:
            return null;
        }
    }

    public static float GetAngleFromVectorFloat(Vector3 dir)
    {
        float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (n < 0) n += 360;

        return n;
    }
}
