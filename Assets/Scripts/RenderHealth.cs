using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderHealth : MonoBehaviour
{
    private float maxHealth;
    [SerializeField]
    private EnemyController playerData;
    [SerializeField]
    private GameObject healthMeter;
    [SerializeField]
    private GameObject healthContainer;
    float scaleY;
    float scaleZ;

    private void Awake()
    {
        maxHealth = playerData.health;
        scaleY = healthMeter.transform.localScale.y;
        scaleZ = healthMeter.transform.localScale.z;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float ratio = Mathf.Clamp01(playerData.health / maxHealth);
        if (ratio < 1 && !healthContainer.activeSelf)
        {
            healthContainer.SetActive(true);
        }
        healthMeter.transform.localScale = new Vector3(ratio, scaleY, scaleZ);
    }
}
