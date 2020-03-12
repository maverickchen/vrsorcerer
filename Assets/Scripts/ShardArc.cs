using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShardArc : MonoBehaviour
{
    [SerializeField]
    private GameObject spikeObj;
    private Collider spikeCollider;
    public GameObject target;
    public float speed = 5f;
    public bool standingBy = true;
    public ParticleSystem shatterParticles;
    public ParticleSystem deactivateParticles;
    public AudioClip spawnSound;
    public AudioClip launchSound;
    public AudioClip hitSound;
    public AudioClip deactivateSound;
    public AudioSource source;

    // Start is called before the first frame update
    void Start()
    {
        spikeCollider = spikeObj.GetComponent<Collider>();
    }

    void Awake()
    {
        OnSpikeActive();
    }

    void OnSpikeActive()
    {
        source.clip = spawnSound;
        source.Play();
        spikeObj.SetActive(true);
    }

    void SetTarget(GameObject gameObject)
    {
        target = gameObject;
    }

    public void Launch(float velocity)
    {
        if (target != null && target.activeSelf)
        {
            standingBy = false;
            // speed = velocity;
            transform.LookAt(target.transform);
            source.clip = launchSound;
            source.Play();
        }
        else
        {
            Debug.Log("Icicle target null/unavailable at Launch time; destroying self");
            Deactivate();
        }
    }

    public void Deactivate()
    {
        source.clip = deactivateSound;
        source.Play();
        deactivateParticles.Play();
        Destroy(spikeObj);
        StartCoroutine(WaitForClipEnd());
    }

    private IEnumerator WaitForClipEnd()
    {
        while (source.isPlaying)
        {
            yield return null;
        }
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (target)
        {
            transform.LookAt(target.transform);
        }
        if (!standingBy)
        {
            transform.position += transform.TransformVector(Vector3.forward) * speed * Time.deltaTime;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        OnCollide();
    }

    private void OnCollide()
    {
        standingBy = true;
        shatterParticles.Play();
        spikeObj.SetActive(false);
        source.clip = hitSound;
        source.Play();
        GetComponent<MeshCollider>().enabled = false;
    }

    void OnTriggerEnter(Collider other)
    {
        standingBy = true;
        shatterParticles.Play();
        spikeObj.SetActive(false);
        source.clip = hitSound;
        source.Play();
        GetComponent<MeshCollider>().enabled = false;
    }
}
