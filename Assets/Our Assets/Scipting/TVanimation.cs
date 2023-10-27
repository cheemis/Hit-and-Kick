using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TVanimation : MonoBehaviour
{
    public GameObject Television;
    public AnimationClip TV;
    Animation anim;
    public bool kicked;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animation>();
    }

    // Update is called once per frame
    void Update()
    {
        //kicked = true;
        kicked = Television.GetComponent<TV>().TVkicked;
        //print(kicked);
        if (kicked == true)
        {
            anim.clip = TV;
            anim.Play();
            kicked = false;
          
        }
        
    }

}
