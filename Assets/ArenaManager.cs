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
    public int totalInstaniatedEnemies;
    public int maxEnemiesAtOnce; 
    public int maxEnemiesPerRound; 
    public int enemyAmountIncreaseInterval;    
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
    [SerializeField] private int totalRagdollSpawnerIterations;
    [SerializeField] private int currentRagdollSpawnerIterations;

    [SerializeField] GameObject activeRagdoll;  
    void Start()
    {
        SceneMaster.instance.player.GetComponent<PlayerManager>().deathEvent.AddListener(ModeEnd);  
        enemiesInstantiated = new GameObject[totalInstaniatedEnemies];
        for (int i = 0; i < totalInstaniatedEnemies; i++)
        {
            GameObject dude = Instantiate(enemiesToInstantiate[Random.Range(1, enemiesToInstantiate.Length) - 1], ragdollParentObject.transform);
            dude.SetActive(false); 
            ActiveRagdoll activeRagdollScript = dude.GetComponent<ActiveRagdoll>(); 
            activeRagdollScript.ragdollMeshRend.enabled = false;
            activeRagdollScript.onDieEvent.AddListener(HandleEnemy);
            enemiesInstantiated[i] = dude; 
            //activeRagdollScript.RagdollDeath(); 
            //dude.GetComponentInChildren<SkinnedMeshRenderer>().enabled = false;  
            print("awww simba"); 
        }      
    }
    
    void OnEnable() 
    {
        roundCounter = SceneMaster.instance.player.GetComponentInChildren<UiData>().roundCounterText;
        currentWaveNumber = 1;
        roundCounter.text = "1";
    }

    
    void Update()
    {
    
    }

    void HandleEnemy()
    { 
        currentWaveEnemies--; 
        if (totalRagdollSpawnerIterations >= maxEnemiesPerRound && currentWaveEnemies <= 0)
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
        if (currentWaveNumber <= 100)
        {
            int foreachIteration = 0; 
            foreach (GameObject ragdoll in enemiesToInstantiate)
            {
                ActiveRagdoll activeRagdollScript = enemiesToInstantiate[foreachIteration].GetComponent<ActiveRagdoll>(); 
                activeRagdollScript.activeRagdollObject.maxRagdollHealth += 2;  
                activeRagdollScript.activeRagdollObject.attackDamage += 1; 
                if (maxEnemiesPerRound < 100)
                {
                    maxEnemiesPerRound += enemyAmountIncreaseInterval;
                }    
                foreachIteration++; 
            }
        }  
        for (int i = 0; i < enemiesInstantiated.Length; i++)
        {
            GameObject dude = enemiesInstantiated[i];
            //dude.SetActive(false);
            dude.GetComponentInChildren<SkinnedMeshRenderer>().enabled = false; 
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
        print("Mode finished"); 
    }

    void WaveStart()
    {
       totalRagdollSpawnerIterations = 0; 
       currentRagdollSpawnerIterations = 0; 
       isAllEnemiesSpawned = false; 
       //currentWaveEnemies = maxEnemiesAtOnce; 
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
        if (currentRagdollSpawnerIterations < maxEnemiesAtOnce)
        {
            activeRagdoll = enemiesInstantiated[totalRagdollSpawnerIterations].gameObject;
            currentRagdollSpawnerIterations++; 
            totalRagdollSpawnerIterations++; 
            currentWaveEnemies++; 
            print(currentRagdollSpawnerIterations); 
            ActiveRagdoll activeRagdollScript = activeRagdoll.GetComponent<ActiveRagdoll>();
            //dudeScrippy.theOverallAnimatedRig.transform.localPosition = new Vector3(0, 0, 0);    
            activeRagdollScript.theOverallAnimatedRig.transform.localPosition = new Vector3(0, 0, 0); 
            activeRagdollScript.theOverallPhysicsRig.transform.localPosition = new Vector3(0, 0, 0); 
            activeRagdollScript.rootAnimatedObj.transform.localPosition = new Vector3(0, 0, 0); 
            activeRagdollScript.rootAnimatedObj.transform.rotation = new Quaternion(0, 0, 0, 0); 
            activeRagdollScript.rootPhysicsObj.transform.rotation = new Quaternion(0, 0, 0, 0); 
            activeRagdollScript.rootPhysicsObj.transform.localPosition = new Vector3(0, 0, 0); 
            if (!activeRagdollScript.isAlive)
            {
                activeRagdollScript.RagdollRevive(); 
            }
            else 
            {
                activeRagdoll.SetActive(true); 
            }
            //activeRagdoll.SetActive(true);
            activeRagdoll.transform.position = spawnPoints[Random.Range(0, spawnPoints.Length - 1)].position;
            activeRagdollScript.ragdollMeshRend.enabled = true;  
            activeRagdollScript.ragdollMeshAnimator.enabled = true;
            //debugDudes.GetComponentInChildren<SkinnedMeshRenderer>().enabled = true;
            print("WTF");
        }
        else if (currentWaveEnemies == 0)
        {
            currentWaveEnemies = 0; 
            currentRagdollSpawnerIterations = 0; 
        }
        if (totalRagdollSpawnerIterations >= maxEnemiesPerRound)
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
}
