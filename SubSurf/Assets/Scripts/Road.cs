using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof (Rigidbody))]
public class Road : MonoBehaviour
{
    float life = -14;
    Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = -transform.forward * RoadManager.Instance.moveSpeed;
        //rb.MovePosition(transform.position - transform.forward * RoadManager.Instance.moveSpeed);
    }

    private void Update()
    {
        if (RoadManager.Instance.gameFinished)
        {
            rb.velocity = Vector3.zero;
            return;
        }
        if (rb.velocity != -transform.forward * RoadManager.Instance.moveSpeed)
        {
            rb.velocity = -transform.forward * RoadManager.Instance.moveSpeed;
        }
        life += Time.deltaTime;
        if (life > RoadManager.Instance.timeForNext)
        {
            Destroy(gameObject);
        }
    }
}
