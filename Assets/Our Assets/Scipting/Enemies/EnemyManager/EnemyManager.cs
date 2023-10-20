using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{

    //Enemy Management Variables
    private int numEnemies = 0;
    [SerializeField]
    private float maxEnemies = 5;
    public float enemyIncrease = .25f;


    //spawning variables
    public GameObject enemyPrefab;
    public Transform playerTransform;
    public Vector3[] spawningRange = new Vector3[2]; //x is spawn side, y & z are the range in which they can spawn


    //other entities variables
    [SerializeField]
    private LocationsManager locationManager;




    // Start is called before the first frame update
    void Awake()
    {
        locationManager = GameObject.FindGameObjectWithTag("SpawningManager")?.GetComponent<LocationsManager>();

        SpawnEnemies();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DespawnEnemy()
    {
        numEnemies--;
        maxEnemies += enemyIncrease;
        Debug.Log(numEnemies + " : " + maxEnemies);
        SpawnEnemies();
    }

    private void SpawnEnemies()
    {
        
        while (numEnemies < (int)maxEnemies)
        {
            //choose a spawn location
            Vector3 spawnSide = spawningRange[Random.Range(0, spawningRange.Length)];
            Vector3 spawnPosition = new Vector3(spawnSide.x, 0, Random.Range(spawnSide.y, spawnSide.z));

            //instantiate new enemy and set components
            GameObject newEnemy = Instantiate(enemyPrefab, spawnPosition, enemyPrefab.transform.rotation);
            newEnemy.GetComponent<Grunt>().InstantiateGrunt(this, playerTransform, locationManager);

            //increase local counter for enemies in game
            numEnemies++;
        }
    }


}
