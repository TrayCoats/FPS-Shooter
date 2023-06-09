using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerEnemy : MonoBehaviour
{
    [SerializeField] GameObject prefab;
    [SerializeField] int intervalTime;
    [SerializeField] Transform[] spawnPos;
    [SerializeField] int prefabMaxNum;


    public List<GameObject> prefabList = new List<GameObject>();

    int prefabsSpawnCount;
    bool playerInRange;
    bool isSpawning;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (playerInRange && !isSpawning && prefabsSpawnCount < prefabMaxNum)
        {
            StartCoroutine(spawn());
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }
    IEnumerator spawn()
    {
        isSpawning = true;
        GameObject prefabClone = Instantiate(prefab, spawnPos[Random.Range(0, spawnPos.Length)].position, prefab.transform.rotation);

        prefabList.Add(prefabClone);

        prefabsSpawnCount++;
        yield return new WaitForSeconds(intervalTime);
        isSpawning = false;
    }
}
