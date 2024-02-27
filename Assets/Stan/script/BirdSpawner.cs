using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BirdSpawner : MonoBehaviour
{
    [System.Serializable]
    public class BirdSpawnInfo
    {
        public GameObject birdPrefab;
        public int rarityPercentage;
        public List<string> allowedSpawnZones;
    }

    public Transform[] spawnZones;
    public BirdSpawnInfo[] birdSpawnInfoList;
    public float spawnInterval = 5f;
    public int maxBirds = 10;

    private int totalRarity;
    private int currentBirds = 0; 

    private BirdPhotoController birdPhotoController; 

    private void Start()
    {
        birdPhotoController = FindObjectOfType<BirdPhotoController>(); // Find BirdPhotoController in the scene

        foreach (var birdSpawnInfo in birdSpawnInfoList)
        {
            totalRarity += birdSpawnInfo.rarityPercentage;
        }

        StartCoroutine(SpawnBirds());
    }

    private IEnumerator SpawnBirds()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

            if (currentBirds >= maxBirds) // Check if the maximum number of birds has been reached
            {
                continue;
            }

            // Shuffle the spawn zones to randomize the selection
            Shuffle(spawnZones);

            // Determine the number of spawn zones to activate at this interval
            int activeSpawnZones = Mathf.Min(Random.Range(1, 5), spawnZones.Length); 

            for (int i = 0; i < activeSpawnZones; i++)
            {
                Transform spawnZone = spawnZones[i];

                int randomValue = Random.Range(0, totalRarity);
                int cumulativeRarity = 0;
                GameObject birdPrefabToSpawn = null;

                foreach (var birdSpawnInfo in birdSpawnInfoList)
                {
                    cumulativeRarity += birdSpawnInfo.rarityPercentage;

                    if (randomValue < cumulativeRarity && birdSpawnInfo.allowedSpawnZones.Contains(spawnZone.name))
                    {
                        birdPrefabToSpawn = birdSpawnInfo.birdPrefab;
                        break;
                    }
                }

                if (birdPrefabToSpawn != null)
                {
                    GameObject spawnedBird = Instantiate(birdPrefabToSpawn, spawnZone.position, Quaternion.identity);
                    spawnedBird.transform.parent = transform;
                    currentBirds++; 

                    BirdDetector birdDetector = spawnedBird.GetComponent<BirdDetector>();
                    if (birdDetector != null && birdPhotoController != null)
                    {
                        birdPhotoController.AddBirdDetector(birdDetector);
                    }
                }
            }
        }
    }

    private void Shuffle(Transform[] array)
    {
        for (int i = array.Length - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            Transform temp = array[i];
            array[i] = array[randomIndex];
            array[randomIndex] = temp;
        }
    }

    public void DecreaseBirdCount()
    {
        currentBirds--;
    }
}
