using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnObj : MonoBehaviour
{
    public GameObject[] objs;
    public List<Transform> spawnPoints;
    public Button Button;

    private bool isSpawning = false;

    private void Start()
    {
        Button.onClick.AddListener(ToggleSpawn);
    }

    private void Update()
    {
        if (isSpawning)
        {
            SpawnObjects();
        }
    }

    private void SpawnObjects()
    {
        if (spawnPoints.Count == 0)
        {
            Debug.LogWarning("No more spawn points available!");
            return;
        }

        for (int i = 0; i < objs.Length; i++)
        {
            int randomIndex = Random.Range(0, spawnPoints.Count);
            Transform spawnPoint = spawnPoints[randomIndex];

            Instantiate(objs[i], spawnPoint.position, Quaternion.identity);

            spawnPoints.RemoveAt(randomIndex);
        }

        isSpawning = false;
    }

    public void ToggleSpawn()
    {
        isSpawning = !isSpawning;
    }
}
