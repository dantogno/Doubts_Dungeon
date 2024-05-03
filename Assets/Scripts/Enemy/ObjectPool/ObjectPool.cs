using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool Instance { get; private set; }

    public GameObject smallEnemyPrefab;
    public GameObject largeEnemyPrefab;

    public int minPoolSize = 10;
    public int maxPoolSize = 30;
    public float checkInterval = 1f;

    private List<GameObject> smallEnemyPool = new List<GameObject>();
    private List<GameObject> largeEnemyPool = new List<GameObject>();
    private int activeObjectsCount = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject); // Ensures only one instance exists
        }
    }

    void Start()
    {
        // Initialize pools with minimum sizes
        InitializePool(smallEnemyPrefab, minPoolSize);
        InitializePool(largeEnemyPrefab, minPoolSize);

        // Start monitoring pool sizes and adjusting dynamically
        StartCoroutine(MonitorPoolSize());
    }

    private void InitializePool(GameObject prefab, int size)
    {
        List<GameObject> pool = prefab == smallEnemyPrefab ? smallEnemyPool : largeEnemyPool;

        for (int i = 0; i < size; i++)
        {
            GameObject obj = Instantiate(prefab);
            obj.SetActive(false);
            pool.Add(obj);
        }
    }

    private IEnumerator MonitorPoolSize()
    {
        while (true)
        {
            yield return new WaitForSeconds(checkInterval);

            // Calculate demand based on active objects count
            float demandRatio = (float)activeObjectsCount / (smallEnemyPool.Count + largeEnemyPool.Count);

            if (demandRatio > 0.8f && (smallEnemyPool.Count + largeEnemyPool.Count) < maxPoolSize)
            {
                // Increase pool size
                int increaseAmount = Mathf.Min(maxPoolSize - (smallEnemyPool.Count + largeEnemyPool.Count), (smallEnemyPool.Count + largeEnemyPool.Count));
                for (int i = 0; i < increaseAmount; i++)
                {
                    GameObject prefab = Random.value < 0.5f ? smallEnemyPrefab : largeEnemyPrefab;
                    List<GameObject> pool = prefab == smallEnemyPrefab ? smallEnemyPool : largeEnemyPool;

                    GameObject obj = Instantiate(prefab);
                    obj.SetActive(false);
                    pool.Add(obj);
                }
            }
            else if (demandRatio < 0.2f && (smallEnemyPool.Count + largeEnemyPool.Count) > minPoolSize)
            {
                // Decrease pool size
                int decreaseAmount = Mathf.Min((smallEnemyPool.Count + largeEnemyPool.Count) - minPoolSize, (smallEnemyPool.Count + largeEnemyPool.Count));
                for (int i = 0; i < decreaseAmount; i++)
                {
                    GameObject obj = null;
                    if (smallEnemyPool.Count > minPoolSize)
                    {
                        obj = smallEnemyPool[smallEnemyPool.Count - 1];
                        smallEnemyPool.RemoveAt(smallEnemyPool.Count - 1);
                    }
                    else if (largeEnemyPool.Count > minPoolSize)
                    {
                        obj = largeEnemyPool[largeEnemyPool.Count - 1];
                        largeEnemyPool.RemoveAt(largeEnemyPool.Count - 1);
                    }

                    if (obj != null)
                        Destroy(obj);
                }
            }
        }
    }

    public GameObject GetObjectFromPool(GameObject prefab)
    {
        List<GameObject> pool = prefab == smallEnemyPrefab ? smallEnemyPool : largeEnemyPool;

        foreach (GameObject obj in pool)
        {
            if (!obj.activeInHierarchy)
            {
                obj.SetActive(true);
                activeObjectsCount++;
                return obj;
            }
        }

        // If no available object, instantiate a new one (optional)
        GameObject newObj = Instantiate(prefab);
        pool.Add(newObj);
        activeObjectsCount++;
        return newObj;
    }

    public void ReturnObjectToPool(GameObject obj)
    {
        obj.SetActive(false);
        activeObjectsCount--;
    }
}
