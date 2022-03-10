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
    public int totalWaveEnemies;   
    public int currentWaveEnemies; 
    [Header("Spawn Time Range")]
    public float spawnTimeRangeMin; public float spawnTimeRangeMax;
    [Header("Waves")]
    [SerializeField] int currentWaveNumber = 0; 
    [Header("Assignables")]
    [SerializeField] TextMeshProUGUI roundCounter; 
    [SerializeField] GameObject[] enemiesToInstantiate; 
    [SerializeField] GameObject[] enemiesInstantiated; 
    [SerializeField] GameObject ragdollParentObject; 
    [SerializeField] GameObject deathParticleEffectPrefab; 
    public Transform[] spawnPoints;
    [Header("Info")]
    bool isTransitioningWave = false; 
    private static System.Timers.Timer roundIntervalTimer;
    private static System.Timers.Timer spawnerIntervals;
    [SerializeField] private bool isAllEnemiesSpawned = false; 
    private bool isSpawnerTimerFinished = true; 
    [SerializeField] private int iterations; 
    [SerializeField] GameObject debugDudes; 
    void Start()
    {
         
    }

    void OnEnable() 
    {
        roundCounter = SceneMaster.instance.player.GetComponentInChildren<UiData>().roundCounterText;
        currentWaveNumber = 1;
        roundCounter.text = "1";
        print("Nala");   
        enemiesInstantiated = new GameObject[totalWaveEnemies];
        for (int i = 0; i < totalWaveEnemies; i++)
        {
            GameObject dude = Instantiate(enemiesToInstantiate[Random.Range(1, enemiesToInstantiate.Length) - 1], ragdollParentObject.transform);
            dude.GetComponent<ActiveRagdoll>().waveModeEvent.AddListener(HandleEnemy);
            enemiesInstantiated[i] = dude; 
            dude.SetActive(false);
            //dude.GetComponentInChildren<SkinnedMeshRenderer>().enabled = false;  
            print("awww simba"); 
        }
        roundIntervalTimer = new Timer(timeBetweenRounds); 
        roundIntervalTimer.Elapsed += OnBetweenRoundTimerFinish;  
        spawnerIntervals = new Timer(spawnTimeRangeMax);
        spawnerIntervals.Elapsed += OnEnemyInterval;   
        WaveStart();
    }

    
    void Update()
    {
    
    }

    void HandleEnemy()
    { 
        currentWaveEnemies--; 
        if (currentWaveEnemies <= 0)
        {
            StartCoroutine(WaveTransition()); 
        }
    }

    IEnumerator WaveTransition()
    {
        isTransitioningWave = true; 
        currentWaveNumber++; 
        yield return new WaitForSeconds(timeBetweenRounds); 
        roundCounter.text = currentWaveNumber.ToString();   
        for (int i = 0; i < enemiesInstantiated.Length; i++)
        {
            GameObject dude = enemiesInstantiated[i];
            dude.SetActive(false);
            //dude.GetComponentInChildren<SkinnedMeshRenderer>().enabled = false; 
            ParticleSystem deathParts = Instantiate(deathParticleEffectPrefab, dude.transform.position, dude.transform.rotation).GetComponent<ParticleSystem>(); 
            deathParts.Play(); 
        }
        isTransitioningWave = false;
        WaveStart(); 
    }

    void SpawningEnemies()
    {
       
    }

    void ModeEnd()
    {

    }

    void WaveStart()
    {
       iterations = 0; 
       isAllEnemiesSpawned = false; 
       currentWaveEnemies = totalWaveEnemies; 
       StartCoroutine(DudeSpawner());   
       //if (isSpawnerTimerFinished)
       //{
            //DudeSpawner(iterations);  
            //if (iterations >= totalWaveEnemies)
            //{
                //isAllEnemiesSpawned = true; 
            //}
            //iterations++; 
            //spawnerIntervals.Interval = Random.Range(spawnTimeRangeMin * 1000, spawnTimeRangeMax * 1000);
            //spawnerIntervals.Start(); 
            //isSpawnerTimerFinished = false; 
       //}
    }

    IEnumerator DudeSpawner()
    {
        debugDudes = enemiesInstantiated[iterations].gameObject;
        iterations++; 
        print(iterations); 
        ActiveRagdoll dudeScrippy = debugDudes.GetComponent<ActiveRagdoll>();
        debugDudes.transform.position = spawnPoints[Random.Range(0, spawnPoints.Length - 1)].position;
        dudeScrippy.theOverallAnimatedRig.transform.localPosition = new Vector3(0, 0, 0);   
        //if(!dudeScrippy.isAlive)
        //{
            //dudeScrippy.RagdollRevive(); 
        //} 
        debugDudes.SetActive(true);
        //debugDudes.GetComponentInChildren<SkinnedMeshRenderer>().enabled = true;
        print("WTF");
        if (iterations >= totalWaveEnemies)
        {
            isAllEnemiesSpawned = true; 
        } 
        yield return new WaitForSeconds(Random.Range(spawnTimeRangeMin, spawnTimeRangeMax));
        if (!isAllEnemiesSpawned)
        {
            StartCoroutine(DudeSpawner()); 
            //spawnerIntervals.Interval = Random.Range(spawnTimeRangeMin * 1000, spawnTimeRangeMax * 1000);
            //spawnerIntervals.Start();
        }
    }
    private void OnBetweenRoundTimerFinish(System.Object source, ElapsedEventArgs e)
    {
        isTransitioningWave = false; 
        WaveStart(); 
    }

    private void OnEnemyInterval(System.Object source, ElapsedEventArgs e)
    {
        isSpawnerTimerFinished = true; 
        DudeSpawner(); 
        print("Timer finished"); 
    }
}
