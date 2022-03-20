using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceShot : MonoBehaviour
{
    public float forceApplied;
    private Vector3 posDiff;
    public Collider theCollider;
    private void OnTriggerEnter(Collider other)
    {
        
        if (other.attachedRigidbody != null)
        {
            posDiff = other.transform.position - theCollider.transform.position;
            other.attachedRigidbody.AddForce(posDiff.normalized * forceApplied, ForceMode.Impulse);
        }
    }
}
