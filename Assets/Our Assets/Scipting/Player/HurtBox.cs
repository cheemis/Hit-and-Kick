using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtBox : MonoBehaviour
{

    //public bool activate; // -- "you dont need this, just use gameObject.activeSelf
    public int hitCounter;


    // Start is called before the first frame update
    void Start()
    {
        //activate = true;
        hitCounter = 2;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //used to detect the enermies
    private void OnTriggerEnter(Collider other)
    {
        if (!/*activate*/gameObject.activeSelf) //this is redundant
        {
            return;
        }
        if (hitCounter < 1)
        {
            return;
        }
        if (other.gameObject.tag == "EnemyBody")
        {
            Grunt grunt = other.gameObject.GetComponent<Grunt>();
            if (!grunt.isAlreadyPunched())
            {
                Debug.Log("the counter is: " + hitCounter);
                //call death
                grunt.Death();
                hitCounter--;
            }
        }
    }

}
