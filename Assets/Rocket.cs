using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Rocket : MonoBehaviour
{
    private GameObject explosionEffect; 
    public float forceToExplode; 
    public AudioClip[] explodeSound; 
    public GameObject explodeCollider; 
    public Rigidbody rocketRigidbody; 
    void Start() 
    {
         
    }
    public IEnumerator Explode()
    {
        explosionEffect = ObjectPooler.Instance.SpawnFromPool("Explosion", this.transform.position, new Quaternion(0, 0, 0, 0));   
        explosionEffect.GetComponent<ParticleSystem>().Play(); 
        explosionEffect.GetComponent<AudioSource>().PlayOneShot(explodeSound[Random.Range(0, explodeSound.Length)]); 
        explodeCollider.SetActive(true); 
        yield return new WaitForEndOfFrame(); 
        explodeCollider.SetActive(false); 
        rocketRigidbody.Sleep(); 
        gameObject.SetActive(false); 
        print("Hit"); 
    }
    void OnCollisionEnter(Collision other) 
    {
        if (!other.collider.isTrigger && other.relativeVelocity.magnitude >= forceToExplode)
        {
            StartCoroutine(Explode()); 
        }
        
    }
}
