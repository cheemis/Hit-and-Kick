using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationsManager : MonoBehaviour
{
    //spawn location variables
    private int numSpawnLocations;
    [SerializeField]
    private int[] locationIndexes;
    [SerializeField]
    private int currentLocation = 0;


    // Start is called before the first frame update
    void Start()
    {
        numSpawnLocations = transform.childCount;
        locationIndexes = new int[numSpawnLocations];
        for (int i = 0; i < numSpawnLocations; i++) locationIndexes[i] = i; //for randomized picking of location
        RandomizeLocationIndexes();
    }

    public void ResetLocations()
    {
        currentLocation = 0;
        RandomizeLocationIndexes();

    }

    private void RandomizeLocationIndexes()
    {
        for (int i = 0; i < numSpawnLocations;  ++i)
        {
            int randomIndex = Random.Range(i, numSpawnLocations);
            int temp = locationIndexes[randomIndex];

            locationIndexes[randomIndex] = locationIndexes[i];
            locationIndexes[i] = temp;
        }
        Debug.Log("randomized spawn locations!");
    }

    public Vector3 GetRandomLocation()
    {
        Vector3 returnLocation = Vector3.zero; //base case: no more spawn locations

        if(currentLocation < numSpawnLocations)
        {
            returnLocation = transform.GetChild(locationIndexes[currentLocation]).position;
            currentLocation++;
        }

        return returnLocation;
    }
}
