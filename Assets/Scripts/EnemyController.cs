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
    private Rigidbody rb;
    private float distToBottom;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        distToBottom = GetComponent<Collider>().bounds.extents.y;
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
            OnDeath();
        }

        if (isActive)
        {
            if (!isStunned)
            {
                if (!IsGrounded())
                {
                    transform.position -= Vector3.up*Time.fixedDeltaTime;
                }
                else
                {
                    Transform playerTrans = Player.instance.headCollider.transform;
                    Vector3 playerPos = playerTrans.position;
                    Vector3 targetPos = new Vector3(playerPos.x, transform.position.y, playerPos.z);
                    transform.LookAt(targetPos);
                    transform.position += transform.TransformVector(Vector3.forward).normalized * speed * Time.fixedDeltaTime;
                }
            }
            else
            {
                transform.localRotation = Quaternion.Euler(0f, transform.localRotation.eulerAngles.y, 0f);
            }
        }
    }

    bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, distToBottom + 0.1f);
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

    void OnDeath()
    {
        EnemyManager.instance.RemoveDeadEnemy(gameObject);
        isActive = false;
        rb.isKinematic = false;
        Vector3 dir = (transform.position - Player.instance.transform.position).normalized;
        dir.y = 2f;
        rb.AddForceAtPosition(dir.normalized * 5f, transform.TransformVector(Vector3.up), ForceMode.Impulse);
        // rb.AddForce((Player.instance.bodyDirectionGuess + .5f*Vector3.up).normalized * 5f, ForceMode.Impulse);
        Destroy(gameObject, 2f);
    }

    public void OnStunStart()
    {
        isStunned = true;
        lastStunnedTime = Time.time;
        stunParticles.Play();
        animator.SetBool("isStunned", true);
    }

    public void OnStunOver()
    {
        isStunned = false;
        stunParticles.Stop();
        animator.SetBool("isStunned", false);
    }

}
