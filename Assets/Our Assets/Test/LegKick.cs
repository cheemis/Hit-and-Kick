using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegKick : MonoBehaviour
{
    //private bool kicking = false;
    public AnimationClip LEG;
    Animation anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animation>();
    }

    // Update is called once per frame
    void Update()
    {


    }
    private void OnEnable()
    {
        KickingManager.onKickTV += KickAni;
        //KickingManager.onRecoverFromKick += RecoverFromKickingTV;
    }

    private void OnDisable()
    {
        KickingManager.onKickTV -= KickAni;
        //KickingManager.onRecoverFromKick -= RecoverFromKickingTV;
    }

    private void KickAni()
    {
        anim.clip = LEG;
        anim.Play();
    }

}
