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
    [SerializeField]
    private float unblurSpeed = .5f;
    private DepthOfField depth;
    [SerializeField]
    private Vector2 blurLerpRange = new Vector2(0, 10);

    //color grading variables
    [SerializeField]
    private float colorSpeed = 5f;
    private ColorGrading color;
    [SerializeField]
    private Vector2 colorLerpRange = new Vector2(-100, 0);
    [SerializeField]
    private float colorPostSpeed = 1;
    [SerializeField]
    private Vector2 colorPostLerpRange = new Vector2(-100, 0);




    // Start is called before the first frame update
    void Start()
    {
        //set the distortion variables
        volume = GetComponent<PostProcessVolume>();
        volume.profile.TryGetSettings<Grain>(out grain);
        volume.profile.TryGetSettings<LensDistortion>(out lens);
        volume.profile.TryGetSettings<DepthOfField>(out depth);
        volume.profile.TryGetSettings<ColorGrading>(out color);

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
        color.active = true;

        depth.focusDistance.value = blurLerpRange.x;

        //randomcolor shifting
        colorLerpRange.x *= Random.Range(0,100) < 50 ? 1 : -1;
        color.saturation.value = colorLerpRange.x;

        //contrast - testing code
        colorLerpRange.x *= Random.Range(0, 100) < 50 ? 1 : -1;
        color.contrast.value = colorLerpRange.x / 2;

        //color post proccessing
        color.postExposure.value = colorPostLerpRange.y;
    }

    private void StopDistortingScreen()
    {
        distorting = false;
        grain.active = false;
        lens.active = false;
        depth.active = false;
        color.active = false;
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
        depth.focusDistance.value = Mathf.Lerp(blurLerpRange.x, blurLerpRange.y, unblurSpeed * Time.deltaTime);

        color.saturation.value = Mathf.Lerp(color.saturation.value, colorLerpRange.y, colorSpeed * Time.deltaTime);
        color.contrast.value = Mathf.Lerp(color.saturation.value, colorLerpRange.y, colorSpeed * Time.deltaTime);

        color.postExposure.value = Mathf.Lerp(color.postExposure, colorPostLerpRange.x, colorSpeed * Time.deltaTime);
    }
}
