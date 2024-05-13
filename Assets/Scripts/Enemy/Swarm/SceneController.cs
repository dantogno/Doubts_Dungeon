using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneController : MonoBehaviour
{
    public BoidController boidPrefab;
    public int spawnBoids = 100;
    private List<BoidController> _boids;

    private void Start()
    {
        _boids = new List<BoidController>();

        for (int i = 0; i < spawnBoids; i++)
        {
            SpawnBoid(boidPrefab.gameObject);
        }
    }

    private void Update()
    {
        foreach (BoidController boid in _boids)
        {
            boid.SimulateMovement(_boids, Time.deltaTime);
        }
    }

    private void SpawnBoid(GameObject prefab)
    {
        var boidInstance = Instantiate(prefab);
        boidInstance.transform.localPosition += new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10));
        boidInstance.transform.localRotation = Quaternion.Euler(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360));
        var boidController = boidInstance.GetComponent<BoidController>();

        // Assign random SwarmIndex
        boidController.SwarmIndex = Random.Range(0, 3);

        _boids.Add(boidController);
    }
}
