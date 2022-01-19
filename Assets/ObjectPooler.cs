using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    [System.Serializable]
    public class Pool
    {
        public GameObject prefab; 
        public string tag; 
        public int size; 
    }

    #region Singleton
    public static ObjectPooler Instance;
    private void Awake() 
    {
        Instance = this; 
    }

    #endregion
    public List<Pool> pools;
    public Dictionary<string, Queue<GameObject>> poolDicionary; 

    void Start()
    {
        poolDicionary = new Dictionary<string, Queue<GameObject>>();

        foreach (Pool pool in pools)
        {
            Queue<GameObject> objectpool = new Queue<GameObject>();
            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false); 
                objectpool.Enqueue(obj); 
            }

            poolDicionary.Add(pool.tag, objectpool); 
        }

    
    }
        public GameObject SpawnFromPool (string tag, Vector3 position, Quaternion rotation)
        {
           if (!poolDicionary.ContainsKey(tag))
           {
               Debug.LogWarning("Pool with tag " + tag + " doesn't excist.");
               return null; 
           }
           GameObject objectToSpawn = poolDicionary[tag].Dequeue();

           objectToSpawn.SetActive(true); 
           objectToSpawn.transform.position = position;
           objectToSpawn.transform.rotation = rotation; 

           poolDicionary[tag].Enqueue(objectToSpawn); 

           return objectToSpawn;
        }
 

}
