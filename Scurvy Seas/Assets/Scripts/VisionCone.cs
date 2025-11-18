using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class VisionCone : MonoBehaviour
{
    [SerializeField] private LayerMask collisionLayers;
    [SerializeField] private LayerMask layersToHit;

    private List<Transform> transformsInCone = new List<Transform>();

    private void OnTriggerEnter(Collider other)
    {
        //if (other.gameObject.)
    }

    private void OnTriggerExit(Collider other)
    {
        
    }


}
