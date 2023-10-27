using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TV : MonoBehaviour
{

    public GameObject[] Crack;
    public bool isKicked = false;
    int i;
    public bool TVkicked;
 

    // Start is called before the first frame update
    void Start()
    {
        //Find Cracks
        Crack = GameObject.FindGameObjectsWithTag("Crack");
        //Set Cracks invisible
        for (i = 0; i < Crack.Length; i++)
        {
            isKicked = Crack[i].activeSelf;
            isKicked = false;
            Crack[i].SetActive(isKicked);
        }
        i = 0;
    }

    // Update is called once per frame
    void Update()
    {

        //print(TVkicked);
    }
   
    void OnTriggerEnter(Collider collision)
    {
        //If got kicked
        if (collision.tag == "Leg")
        {
            TVkicked = true;
            //Count kick times
            if (i>=0 && i < Crack.Length)
            {
                isKicked = Crack[i].activeSelf;
                isKicked = true;
                
                Crack[i].SetActive(isKicked);
                isKicked=false;
                
                i++;
                
            }
            
        }
    }
    void OnTriggerExit(Collider collision)
    {
        TVkicked = false;
    }
}
