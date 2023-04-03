using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformSpawner : MonoBehaviour
{
    public GameObject platformPrefab;
   
    public int numberOfPlatforms = 1000;

    public float levelWidth = 5f;
    public float minY = 2f;
    public float maxY = 3.3f;
     
    void Start()
    {
        Vector3 spawnPosition = new Vector3();
       
        for (int i = 0; i < numberOfPlatforms; i++)
        {   
            spawnPosition.y += Random.Range(minY, maxY);
            spawnPosition.x += Random.Range(-levelWidth/2, levelWidth);
            Instantiate(platformPrefab, spawnPosition, Quaternion.identity);
        }
        
    }
    
    void Update()
    {
      
    }
}
