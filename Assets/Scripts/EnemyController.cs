using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class EnemyController : MonoBehaviour
{
    public Element element = Element.NONE;
    public bool flying = false;
    public bool heavy = false;

    public float health = 100f;
    public float speed = 1f;
    public bool isActive = false;
    public bool isStunned = false;
    public float lastStunnedTime = 0f;
    public float stunDuration = 1f;
    public ParticleSystem stunParticles;
    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Update()
    {

        if (isStunned && lastStunnedTime + stunDuration < Time.time)
        {
            OnStunOver();
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        animator.SetBool("isActive", isActive);
        if (health <= 0f)
        {
            EnemyManager.instance.RemoveDeadEnemy(gameObject);
            Destroy(gameObject);
        }

        if (isActive && !isStunned)
        {
            Transform playerTrans = Player.instance.headCollider.transform;
            Vector3 playerPos = playerTrans.position;
            Vector3 targetPos = new Vector3(playerPos.x, transform.position.y, playerPos.z);
            transform.LookAt(targetPos);
            transform.position += transform.TransformVector(Vector3.forward).normalized * speed * Time.fixedDeltaTime;
        }
    }

    float CalculateDamage(AttackProps attackType)
    {
        float elementModifier = (attackType.element == element) ? 0.5f : 1f;
        float armorModifier = 1f;
        if (heavy)
        {
            if (attackType.isPiercing)
            {
                armorModifier = 1f;
            }
            else
            {
                armorModifier = .5f;
            }
        }
        return attackType.baseDamage * elementModifier * armorModifier;
    }


    public void OnTriggerEnter(Collider other)
    {
        AttackProps attackProps = other.gameObject.GetComponent<AttackProps>();
        Debug.Log("hit");
        if (attackProps)
        {
            health -= CalculateDamage(attackProps);
            if (!isStunned && attackProps.isStunning)
            {
                OnStunStart();
                attackProps.Proliferate(this);
            }
        }
    }

    public void OnStunStart()
    {
        isStunned = true;
        lastStunnedTime = Time.time;
        stunParticles.Play();
        
    }

    public void OnStunOver()
    {
        isStunned = false;
        stunParticles.Stop();
    }

}
