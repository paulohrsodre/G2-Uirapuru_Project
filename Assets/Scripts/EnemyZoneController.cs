using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyZoneController : MonoBehaviour
{
    public GameObject[] enemyPrefabs;
    public Transform[] spawnPoints;

    private List<GameObject> spawnedEnemies = new List<GameObject>();
    private bool playerInside = false;

    private void RespawnEnemies()
    {
        foreach (GameObject enemy in spawnedEnemies)
        {
            if (enemy != null)
            {
                Destroy(enemy);
            }
        }

        spawnedEnemies.Clear();

        for (int i = 0; i < enemyPrefabs.Length && i < spawnPoints.Length; i++)
        {
            GameObject newEnemy = Instantiate(enemyPrefabs[i], spawnPoints[i].position, Quaternion.identity);
            spawnedEnemies.Add(newEnemy);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerInside = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerInside = false;
            RespawnEnemies();
        }
    }
}
