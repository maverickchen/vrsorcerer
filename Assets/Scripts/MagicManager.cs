using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class MagicManager : MonoBehaviour
{
    public GameObject targetingReticle;
    private Stack<GameObject> shardStack;
    public GameObject shardPrefab;
    public GameObject targetedObject;
    public AudioSource audioSource;
    public AudioClip targetFoundClip;

    public ParticleSystem runeParticles;

    private Dictionary<Hand, ISpell> activeMagicMap = new Dictionary<Hand, ISpell>();

    // Start is called before the first frame update
    void Start()
    {
        shardStack = new Stack<GameObject>();
    }

    public ISpell GetSpellFromHand(Hand hand)
    {
        ISpell spell;
        if (activeMagicMap.TryGetValue(hand, out spell))
        {
            return spell;
        }
        return null;
    }

    // Update is called once per frame
    void Update()
    {
        foreach (Hand hand in Player.instance.hands)
        {
            try
            {
                ISpell spell = hand.hoveringInteractable.gameObject.GetComponent<ISpell>();
                if (hand.GetGrabStarting() != GrabTypes.None)
                {
                    spell.Prepare();
                }
                else if (hand.GetGrabEnding() != GrabTypes.None)
                {
                    spell.Execute();
                    runeParticles.Stop();
                    runeParticles.Play();
                }
            }
            catch (System.Exception)
            {
                Debug.Log("no spell active");
            }
            
        }
    }

    void FixedUpdate()
    {
        RaycastHit hit = new RaycastHit();
        int TARGETABLE_LAYER = 9;
        float radius = .25f;
        float maxDist = 30f;
        int layerMask = 1 << TARGETABLE_LAYER;
        Vector3 startPos = Player.instance.headCollider.transform.position;
        Vector3 dir = Player.instance.headCollider.transform.forward;
        Physics.SphereCast(startPos, radius, dir, out hit, maxDist, layerMask);
        Debug.DrawRay(startPos, dir);
        if (hit.collider && hit.collider.gameObject != targetedObject)
        {
            targetedObject = hit.collider.gameObject;
            targetingReticle.GetComponent<FaceThePlayer>().focusedObj = targetedObject;
            foreach (Hand hand in Player.instance.hands)
            {
                try
                {
                    ISpell spell = hand.hoveringInteractable.gameObject.GetComponent<ISpell>();
                    spell.SetTarget(targetedObject);
                }
                catch (System.Exception)
                {
                    Debug.Log("No current spell");
                }
            }
            audioSource.clip = targetFoundClip;
            audioSource.Play();
        }
    }
}
