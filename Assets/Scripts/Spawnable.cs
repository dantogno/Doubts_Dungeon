using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawnable : MonoBehaviour
{
    public List<GameObject> Spawns;
    public int minSpawnNum;
    public int AmountToChangeSpawns;

    private void Start()
    {
        RandomizeSpawnPoints();
    }

    public void RandomizeSpawnPoints()
    {
        for (int i = 0; i < AmountToChangeSpawns; i++)
        {
            int inty = Random.Range(minSpawnNum, Spawns.Count);
          
            Spawns[inty].SetActive(false);
        }
    }
}
