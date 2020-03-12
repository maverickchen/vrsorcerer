using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager instance;
    public List<GameObject> enemies = new List<GameObject>();
    public float timeBetweenSpawns = 3f;
    public float lastSpawnedTime = 0f;
    public int maxEnemyCount;
    public List<GameObject> enemyPrefabs;
    public List<Vector3> spawnPositions = new List<Vector3>()
    {
        new Vector3(-3.5f, 2.5f, 27.5f),
        new Vector3(0f, 2.5f, 27.5f),
        new Vector3(3.5f, 2.5f, 27.5f),
    };

    public GameObject startScreenObjects;

    public bool gameInProgress = false;
    public AudioClip gong;
    public AudioSource audioSource;

    public void StartGame()
    {
        gameInProgress = true;
        startScreenObjects.SetActive(false);
        audioSource.clip = gong;
        audioSource.Play();
    }

    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        } else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (gameInProgress)
        {
            if (enemies.Count < maxEnemyCount && lastSpawnedTime + timeBetweenSpawns < Time.time)
            {
                int spawnIndex = Random.Range(0, spawnPositions.Count);
                GameObject enemyPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Count)];
                GameObject newEnemy = Instantiate(enemyPrefab, spawnPositions[spawnIndex], Quaternion.identity);
                newEnemy.GetComponent<EnemyController>().isActive = true;
                enemies.Add(newEnemy);
                lastSpawnedTime = Time.time;
            }
            int i = 0;
            while (i < enemies.Count)
            {
                if (enemies[i] == null)
                {
                    enemies.RemoveAt(i);
                }
                else
                {
                    i++;
                }
            }
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        gameInProgress = true;
    }

    public void RemoveDeadEnemy(GameObject gameObject)
    {
        enemies.Remove(gameObject);
    }
}
