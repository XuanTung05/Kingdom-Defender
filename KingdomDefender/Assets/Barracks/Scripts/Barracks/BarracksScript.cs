using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarracksScript : MonoBehaviour
{
    public GameObject soldierPrefab;
    public Transform[] spawnPoints;
    public int soldierCount = 3;

    private List<GameObject> activeSoldiers = new List<GameObject>();

    private void Start()
    {
        StartCoroutine(SpawnSoldiers());
    }

    private IEnumerator SpawnSoldiers()
    {
        while (true)
        {
            // If no active soldiers are left, start spawning again
            if (activeSoldiers.Count == 0)
            {
                for (int i = 0; i < soldierCount; i++)
                {
                    Transform spawnPoint = spawnPoints[i % spawnPoints.Length];
                    GameObject soldier = Instantiate(soldierPrefab, spawnPoint.position, spawnPoint.rotation);
                    SoldierScript soldierScript = soldier.GetComponent<SoldierScript>();
                    soldierScript.SetSpawnPoint(spawnPoint.position);

                    // Add the soldier to the active soldiers list
                    activeSoldiers.Add(soldier);

                    // Listen for when the soldier is destroyed or deactivated
                    soldier.GetComponent<SoldierScript>().OnSoldierDeath += () => OnSoldierDeath(soldier);

                    yield return new WaitForSeconds(2f);
                }
            }

            // Check every 1 second if all soldiers are gone
            yield return new WaitForSeconds(45f);
        }
    }

    private void OnSoldierDeath(GameObject soldier)
    {
        // Remove the soldier from the active soldiers list when it is destroyed or deactivated
        if (activeSoldiers.Contains(soldier))
        {
            activeSoldiers.Remove(soldier);
        }
    }
}
