using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
using TMPro; 
using UnityEngine.AI; 
using System.Timers; 

public class ArenaManager : MonoBehaviour
{
    [Header("Varibles")]
    public float timeBetweenRounds;
    public float spawnTimeRange; 
    public int totalWaveEnemies;   
    public int currentWaveEnemies; 
    [SerializeField] int currentWaveNumber = 0; 
    [Header("Assignables")]
    [SerializeField] TextMeshProUGUI roundCounter; 
    [SerializeField] GameObject[] enemiesToSpawn; 
    [SerializeField] GameObject[] enemiesSpawned; 
    [SerializeField] GameObject ragdollParentObject; 
    public Transform[] spawnPoints;
    [Header("Info")]
    bool isTransitioningWave = false; 
    private static System.Timers.Timer roundIntervalTimer;
    private static System.Timers.Timer spawnerIntervals;
    void Start()
    {
        roundCounter = SceneMaster.instance.player.GetComponentInChildren<UiData>().roundCounterText; 
        roundCounter.text = "0";  
    }

    void OnEnable() 
    {
        currentWaveNumber = 0;
        print("Nala");   
        enemiesSpawned = new GameObject[totalWaveEnemies];
        for (int i = 0; i < totalWaveEnemies; i++)
        {
            GameObject dude = Instantiate(enemiesToSpawn[Random.Range(1, enemiesToSpawn.Length) - 1], ragdollParentObject.transform);
            dude.GetComponent<ActiveRagdoll>().waveModeEvent.AddListener(HandleEnemy);
            enemiesSpawned[i] = dude; 
            dude.SetActive(false); 
            print("awww simba"); 
        }
        roundIntervalTimer = new Timer(timeBetweenRounds); 
        roundIntervalTimer.Elapsed += OnBetweenRoundTimerFinish;  
    }

    
    void Update()
    {
    
    }

    void HandleEnemy()
    {
        totalWaveEnemies--; 
        currentWaveEnemies--; 
        if (totalWaveEnemies <= 0)
        {
            WaveTransition(); 
        }
    }

    void WaveTransition()
    {
        isTransitioningWave = true; 
        currentWaveNumber++; 
        roundCounter.text = currentWaveNumber.ToString(); 
        roundIntervalTimer.Enabled = true;  
    }

    void WaveStart()
    {
        foreach (GameObject dude in enemiesSpawned)
        {
            
        }

    }

    void ModeEnd()
    {

    }

    void DudeSpawner(int arrayIndex)
    {
        GameObject dude = enemiesSpawned[arrayIndex].gameObject;
        ActiveRagdoll dudeScrippy = dude.GetComponent<ActiveRagdoll>();
        dude.SetActive(true); 
        if(!dudeScrippy.isAlive)
        {
            dudeScrippy.RagdollRevive(); 
        }
    }
    private void OnBetweenRoundTimerFinish(System.Object source, ElapsedEventArgs e)
    {
        isTransitioningWave = false; 
        WaveStart(); 
    }

    private void OnEnemyInterval(System.Object source, ElapsedEventArgs e)
    {
        
    }
}
