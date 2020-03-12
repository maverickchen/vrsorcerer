using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AttackProps : MonoBehaviour
{
    public Element element = Element.NONE;
    public float baseDamage = 20f;
    public bool isPiercing = false;
    public bool isStunning = false;
    private Collider[] colliders;
    public GameObject lightningArcPrefab;

    public void FollowUp()
    {

    }

    public void Proliferate(EnemyController enemyController)
    {
        float arcRadius = 5f;
        int TARGETABLE_LAYER = 9;
        int layerMask = 1 << TARGETABLE_LAYER;
        
        colliders = Physics.OverlapSphere(enemyController.gameObject.transform.position, arcRadius, layerMask);
        for (int i = 0; i < colliders.Length; i++)
        {
            ElecArc arc = Instantiate(lightningArcPrefab, Vector3.zero, Quaternion.identity).GetComponent<ElecArc>();
            arc.SetEndpoints(transform.position, colliders[i].transform.position);
            EnemyController enemy = colliders[i].gameObject.GetComponent<EnemyController>();
            if (enemy) enemy.health -= 20f;
        }
    }
}
