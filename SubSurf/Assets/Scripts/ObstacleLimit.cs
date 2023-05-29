using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleLimit : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {
            Destroy(other.gameObject);
        }
    }
}
