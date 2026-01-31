using UnityEngine;
using System.Collections;

public class BallonSpawner : MonoBehaviour
{
    public GameObject BallonPrefab;
    public float spawnInterval = 0.5f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (BallonPrefab == null)
        {
            Debug.LogError("BalloonSpawner: No balloonPrefab assigned!");
            return;
        }

        StartCoroutine(spawnLoop());
    }


    IEnumerator spawnLoop()
    {
        while(true)
        {
            spawnBallon();

            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void spawnBallon()
    {
         Vector3 spawnPosition = new Vector3 (
                    Random.Range(-60f, 60f),
                    Random.Range(0f, 0f),
                    Random.Range(-60f, 60f)
                );

        Instantiate(BallonPrefab, spawnPosition, Quaternion.identity);
    }

//     // Update is called once per frame
//     void Update()
//     {
        
//     }
}
