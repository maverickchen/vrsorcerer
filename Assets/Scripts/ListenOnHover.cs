using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class ListenOnHover : MonoBehaviour
{
    bool hovering = false;

    public MagicInterface magic;
    public GameObject shardPrefab;

    public AudioClip hoverStartSound;
    public AudioClip hoverEndSound;
    public AudioSource audioSource;
    private GameObject currentShard;
    private Stack<GameObject> shardStack;
    private GameObject targetedObject;
    private Interactable interactable;
    [SerializeField]
    private Material mat;

    // Start is called before the first frame update
    void Start()
    {
        shardStack = new Stack<GameObject>();
        interactable = GetComponent<Interactable>();
    }

    // Update is called once per frame
    void Update()
    {
        if (hovering)
        {
        }
    }

    public void SetTarget(GameObject newTarget)
    {
        targetedObject = newTarget;
        if (currentShard)
        {
            currentShard.GetComponent<ShardArc>().target = newTarget;
        }
        
    }

    public void Prepare()
    {
        GameObject newShard = Instantiate(shardPrefab, transform.position + Vector3.up * 3f, transform.rotation);
        currentShard = newShard;
        newShard.GetComponent<ShardArc>().target = magic.targetedObject;
        shardStack.Push(newShard);
    }

    public void Execute()
    {
        if (currentShard)
        {
            shardStack.Pop().GetComponent<ShardArc>().Launch(20f);
            currentShard = null;
        }
    }

    public void OnHover()
    {
        hovering = true;
        foreach (Hand hand in Player.instance.hands)
        {
            Debug.Log(hand.hoveringInteractable);
            if (hand.hoveringInteractable == interactable)
            {
                Debug.Log("Hovering");
                ushort length = 10000;
                hand.TriggerHapticPulse(length);
            }
        }
        magic.magicType = gameObject;
        audioSource.clip = hoverStartSound;
        audioSource.Play();
    }

    public void OnHoverExit()
    {
        hovering = false;
        magic.magicType = null;
        currentShard = null;
        while (shardStack.Count > 0)
        {
            shardStack.Pop().GetComponent<ShardArc>().Deactivate();
        }
        shardStack = new Stack<GameObject>();
        audioSource.clip = hoverEndSound;
        audioSource.Play();
    }
}
