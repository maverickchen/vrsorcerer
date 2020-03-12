using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceThePlayer : MonoBehaviour
{
    public GameObject player;
    public GameObject focusedObj;
    public GameObject reticle;
    public float offset;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void ShowReticle(bool show)
    {
        reticle.SetActive(show);
    }


    // Update is called once per frame
    void LateUpdate()
    {
        bool shouldShow = (focusedObj != null && focusedObj.activeSelf);
        ShowReticle(shouldShow);
        if (shouldShow && player)
        {
            transform.LookAt(player.transform, Vector3.up);
            transform.position = focusedObj.transform.position + transform.TransformVector(Vector3.forward) * offset;
        }
    }
}
