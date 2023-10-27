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
        int z = 0;
        int xr = 0;
        float x = 0;
        
        
        if (Input.GetKey(KeyCode.A))
        {
            for(int i = 0;i<20;i++ )
            {
                z = - i;
            }
            x = -0.2f;
        }
        if (Input.GetKey(KeyCode.D))
        {
            for (int i = 0; i < 20; i++)
            {
                z = i;
            }
            x = 0.2f;
        }
        if (Input.GetKey(KeyCode.W))
        {
            for (int i = 0; i < 20; i++)
            {
                xr = - i;
            }

        }
        if (Input.GetKey(KeyCode.S))
        {
            for (int i = 0; i < 20; i++)
            {
                xr = i;
            }

        }
        transform.localEulerAngles = new Vector3(xr, 180, z);
        transform.localPosition = new Vector3(x,2 ,-2.3f);
        //this.transform.Rotate(0, 0, z);
    }
}
