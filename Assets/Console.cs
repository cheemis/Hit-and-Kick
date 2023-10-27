using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Console : MonoBehaviour
{
    public GameObject Con;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //int z = 0;
        
        if (Input.GetKey(KeyCode.A))
        {
            /*if(z <= 0 && z > -10)
            {
                z = -2;
            }*/
            transform.localEulerAngles = new Vector3(5, 180, -10);
        }
        //this.transform.Rotate(0, 0, z);
    }
}
