using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElecArc : MonoBehaviour
{
    private float seed;
    public Vector3 start;
    public Vector3 end;
    public float noiseScale = .2f;
    [SerializeField]
    private LineRenderer line;
    public int numVertices = 20;
    public float lifetime = 3f;
    public float startTime = 0f;

    // Start is called before the first frame update
    void Awake()
    {
        Vector3[] positions = new Vector3[numVertices];

        line.SetPositions(positions);
        SetEndpoints(start, end);
        line.enabled = false;
        seed = Random.value * 20f;
    }

    // Update is called once per frame
    void Update()
    {
        if (line.enabled)
        {
            if ((startTime + lifetime) < Time.time)
            {
                line.enabled = false;
            }
            else
            {
                for (int i = 1; i < line.positionCount - 1; i++)
                {
                    float ratio = i / (float)line.positionCount;
                    Vector3 linearPos = Vector3.Lerp(start, end, ratio);
                    float newX = linearPos.x + noiseScale * Mathf.Sin(i * Time.time + i * seed);
                    float newY = linearPos.y + noiseScale * Mathf.Sin(i * Time.time + i * seed);
                    float newZ = linearPos.z + noiseScale * Mathf.Sin(i * Time.time + i * seed);
                    Vector3 noisedPos = new Vector3(newX, newY, newZ);

                    line.SetPosition(i, noisedPos);
                }
            }
        }
    }

    public void FillMidpoints()
    {
        // fill out the midpoints
        for (int i = 1; i < line.positionCount - 1; i++)
        {
            float ratio = i / (float)line.positionCount;
            Vector3 linearPos = Vector3.Lerp(start, end, ratio);
            float newX = linearPos.x + noiseScale * Mathf.Sin(i * Time.time + i * 2);
            float newY = linearPos.y + noiseScale * Mathf.Sin(i * Time.time + i * 3);
            float newZ = linearPos.z + noiseScale * Mathf.Sin(i * Time.time + i * 4);
            Vector3 noisedPos = new Vector3(newX, newY, newZ);

            line.SetPosition(i, noisedPos);
        }
    }

    public void SetEndpoints(Vector3 newStart, Vector3 newEnd)
    {
        start = newStart;
        end = newEnd;

        line.SetPosition(0, start);
        line.SetPosition(line.positionCount - 1, end);

        FillMidpoints();
        line.enabled = true;
        startTime = Time.time;
    }
}
