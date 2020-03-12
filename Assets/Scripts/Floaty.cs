using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floaty : MonoBehaviour
{

    public float speed = .4f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.Rotate(speed*Mathf.Sin(.01f*Time.time+.2f), speed * Mathf.Sin(10 + Time.time * .05f), speed * Mathf.Sin(4*Time.time * .03f));
    }
}
