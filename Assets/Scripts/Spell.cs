using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class Spell : MonoBehaviour, ISpell
{
    bool hovering = false;

    public MagicManager magicManager;
    public GameObject shardPrefab;

    public AudioClip hoverStartSound;
    public AudioClip hoverEndSound;
    public AudioSource audioSource;
    private GameObject currentShard;
    private Stack<GameObject> shardStack;
    private GameObject targetedObject;
    private Interactable interactable;

    // Start is called before the first frame update
    void Start()
    {
        shardStack = new Stack<GameObject>();
        interactable = GetComponent<Interactable>();
    }

    // Update is called once per frame
    void Update()
    {
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
        GameObject newShard = Instantiate(shardPrefab, transform.position + Vector3.up*2f + Player.instance.bodyDirectionGuess.normalized, transform.rotation);
        currentShard = newShard;
        newShard.GetComponent<ShardArc>().target = magicManager.targetedObject;
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
        audioSource.clip = hoverStartSound;
        audioSource.Play();
    }

    public void OnHoverExit()
    {
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
