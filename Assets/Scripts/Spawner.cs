// File: Spawner.cs
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public List<GameObject> randomPoints; // List of points in the scene
    public List<GameObject> randomObjects; // List of prefab objects to spawn
    public float spawnFrequency = 1.0f; // Time interval between spawns in seconds
    public float travelTime = 2.0f; // Time it takes for the object to travel to the target point

    private void Start()
    {
        StartCoroutine(SpawnObjects());
    }

    private IEnumerator SpawnObjects()
    {
        while (true)
        {
            // Wait for the next spawn
            yield return new WaitForSeconds(spawnFrequency);

            // Choose a random spawn point and a random object
            GameObject spawnPoint = randomPoints[Random.Range(0, randomPoints.Count)];
            GameObject prefabToSpawn = randomObjects[Random.Range(0, randomObjects.Count)];

            // Instantiate the prefab at the spawn point
            GameObject spawnedObject = Instantiate(prefabToSpawn, spawnPoint.transform.position, Quaternion.identity);

            // Choose a different random point for the object to move to
            List<GameObject> possibleTargets = new List<GameObject>(randomPoints);
            possibleTargets.Remove(spawnPoint);
            GameObject targetPoint = possibleTargets[Random.Range(0, possibleTargets.Count)];

            // Start the movement of the object towards the target point
            StartCoroutine(MoveObject(spawnedObject, targetPoint.transform.position));
        }
    }

    private IEnumerator MoveObject(GameObject obj, Vector3 targetPosition)
    {
        Vector3 startPosition = obj.transform.position;
        float elapsedTime = 0f;
        obj.transform.localScale = new Vector3(0.12f, 0.12f, 1f);

        while (elapsedTime < travelTime)
        {
            // Move the object towards the target point over time
            obj.transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / travelTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the object reaches the exact target position
        obj.transform.position = targetPosition;
        Destroy(obj);
    }
}
