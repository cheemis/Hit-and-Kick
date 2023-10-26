using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TVanimation : MonoBehaviour
{
    public GameObject Television;
    public AnimationClip TV;
    Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnTriggerEnter(Collider collision)
    {
        //If got kicked

        bool kicked = Television.GetComponent<TV>().isKicked;
        if (kicked == true)
        {
            anim.Play("TV");
        }
            
        
    }
}
