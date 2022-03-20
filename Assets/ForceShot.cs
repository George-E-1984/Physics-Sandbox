using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceShot : MonoBehaviour
{
    public float forceApplied;
    private Vector3 posDiff;
    public BoxCollider boxCollider;
    private void OnTriggerEnter(Collider other)
    {
        
        if (other.attachedRigidbody != null)
        {
            posDiff = other.transform.position - boxCollider.transform.position;
            other.attachedRigidbody.AddForce(posDiff.normalized * forceApplied, ForceMode.Impulse);
        }
    }
}
