using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnTrigger : MonoBehaviour
{
    public void OnCollisionEnter(Collision collision)
    {
        Debug.Log("collision");
    }

    public void OnTriggerEnter(Collider other)
    {
        Debug.Log("hit");
        EnemyManager.instance.StartGame();
    }
}
