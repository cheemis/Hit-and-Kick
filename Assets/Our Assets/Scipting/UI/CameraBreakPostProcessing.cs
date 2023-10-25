using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class CameraBreakPostProcessing : MonoBehaviour
{

    //management variables
    [SerializeField]
    private float distortionSpeed = 50;
    private bool distorting = false;


    //post proccessing variables
    private PostProcessVolume volume;

    //grain variables
    private Grain grain;
    [SerializeField]
    private Vector2 grainDistorionRange = new Vector2(0, 1);
    private float grainTarget = 0;

    //lens distortion variables
    private LensDistortion lens;
    [SerializeField]
    private Vector2 lensDistorionRange = new Vector2(10, 80);
    private float lensTarget = 0;

    private Vector2 multRange = new Vector2(0, 1);
    private float xMultTarget = 0;
    private float yMultTarget = 0;

    private Vector2 centerRange = new Vector2(-1, 1);
    private float centerXTarget = 0;
    private float centerYTarget = 0;

    //blur variables
    private DepthOfField depth;
    private Vector2 lerpRange = new Vector2(0, 10);




    // Start is called before the first frame update
    void Start()
    {
        //set the distortion variables
        volume = GetComponent<PostProcessVolume>();
        volume.profile.TryGetSettings<Grain>(out grain);
        volume.profile.TryGetSettings<LensDistortion>(out lens);
        volume.profile.TryGetSettings<DepthOfField>(out depth);

        //make screen clean
        StopDistortingScreen();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if(distorting)
        {
            DistortScreen();
            UnBlurScreen();
        }
        
    }

    private void OnEnable()
    {
        KickingManager.onKickTV += StartDistortingScreen;
        KickingManager.onRecoverFromKick += StopDistortingScreen;
    }

    private void OnDisable()
    {
        KickingManager.onKickTV -= StartDistortingScreen;
        KickingManager.onRecoverFromKick -= StopDistortingScreen;
    }


    private void StartDistortingScreen()
    {
        distorting = true;
        grain.active = true;
        lens.active = true;
        depth.active = true;

        depth.focusDistance.value = lerpRange.x;
    }

    private void StopDistortingScreen()
    {
        distorting = false;
        grain.active = false;
        lens.active = false;
        depth.active = false;
    }










    private void DistortScreen()
    {
        lens.intensity.value = Mathf.Lerp(lens.intensity.value, lensTarget, Time.deltaTime * distortionSpeed);
        if(Mathf.Abs(lens.intensity.value - lensTarget) < 10) {lensTarget = Random.Range(lensDistorionRange.x, lensDistorionRange.y);}



        lens.intensityX.value = Mathf.Lerp(lens.intensityX.value, xMultTarget, Time.deltaTime * distortionSpeed);
        if (Mathf.Abs(lens.intensityX.value - xMultTarget) < .1) { xMultTarget = Random.Range(multRange.x, multRange.y); };

        lens.intensityY.value = Mathf.Lerp(lens.intensityX.value, xMultTarget, Time.deltaTime * distortionSpeed);
        if (Mathf.Abs(lens.intensityY.value - yMultTarget) < .1) { yMultTarget = Random.Range(multRange.x, multRange.y); };



        lens.centerX.value = Mathf.Lerp(lens.centerX.value, centerXTarget, Time.deltaTime * distortionSpeed);
        if (Mathf.Abs(lens.centerX.value - centerXTarget) < .1) { centerXTarget = Random.Range(centerRange.x, centerRange.y); };

        lens.centerY.value = Mathf.Lerp(lens.centerY.value, centerYTarget, Time.deltaTime * distortionSpeed);
        if (Mathf.Abs(lens.centerY.value - centerYTarget) < .1) { centerYTarget = Random.Range(centerRange.x, centerRange.y); };
    }

    private void UnBlurScreen()
    {
        depth.focusDistance.value += Mathf.Lerp(lerpRange.x, lerpRange.y, Time.deltaTime);
    }
}
