using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleAnimation : MonoBehaviour
{
    public float speed = 4f;
    public float amplitude = .15f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float scale = 1.1f + amplitude * Mathf.Sin(speed * Time.time);
        transform.localScale = new Vector3(scale, scale, scale);
    }
}
