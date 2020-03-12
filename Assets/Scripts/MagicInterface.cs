using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class MagicInterface : MonoBehaviour
{
    public GameObject targetingReticle;
    private Stack<GameObject> shardStack;
    public GameObject shardPrefab;
    public GameObject targetedObject;
    public AudioSource audioSource;
    public AudioClip targetFoundClip;

    private GameObject currentShard;

    public GameObject magicType;
    public ParticleSystem runeParticles;

    // Start is called before the first frame update
    void Start()
    {
        shardStack = new Stack<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        foreach (Hand hand in Player.instance.hands)
        {
            if (hand.GetGrabStarting() != GrabTypes.None)
            {
                if (magicType)
                {
                    magicType.GetComponent<ListenOnHover>().Prepare();
                    //GameObject newShard = Instantiate(shardPrefab, transform.position + Vector3.up * 2, transform.rotation);
                    //currentShard = newShard;
                    //newShard.GetComponent<ShardArc>().target = targetedObject;
                    //shardStack.Push(newShard);
                }
            }
            else if (hand.GetGrabEnding() != GrabTypes.None)
            {
                if (magicType)
                {
                    magicType.GetComponent<ListenOnHover>().Execute();
                    runeParticles.Stop();
                    runeParticles.Play();
                }
                
                //Shoot();
            }
        }
    }

    public void Shoot()
    {
        shardStack.Pop().GetComponent<ShardArc>().Launch(20f);
        currentShard = null;
    }

    void FixedUpdate()
    {
        RaycastHit hit = new RaycastHit();
        int TARGETABLE_LAYER = 9;
        float radius = .25f;
        float maxDist = 15f;
        int layerMask = 1 << TARGETABLE_LAYER;
        Vector3 startPos = Player.instance.headCollider.transform.position;
        Vector3 dir = Player.instance.headCollider.transform.forward;
        Physics.SphereCast(startPos, radius, dir, out hit, maxDist, layerMask);
        Debug.DrawRay(startPos, dir);
        if (hit.collider && hit.collider.gameObject != targetedObject)
        {
            targetedObject = hit.collider.gameObject;
            //if (currentShard)
            //{
            //    currentShard.GetComponent<ShardArc>().target = targetedObject;
            //}
            targetingReticle.GetComponent<FaceThePlayer>().focusedObj = targetedObject;
            if (magicType)
            {
                magicType.GetComponent<ListenOnHover>().SetTarget(targetedObject);
            }
            audioSource.clip = targetFoundClip;
            audioSource.Play();
        }
        
    }
}
