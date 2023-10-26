using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class CameraBreakMenu : MonoBehaviour
{
    //management variables
    [SerializeField]
    private float distortionSpeed = 50;


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
    private Vector2 lensDistorionRange = new Vector2(10, 30);
    private float lensTarget = 0;

    private Vector2 multRange = new Vector2(0, .5f);
    private float xMultTarget = 0;
    private float yMultTarget = 0;

    private Vector2 centerRange = new Vector2(-.25f, .25f);
    private float centerXTarget = 0;
    private float centerYTarget = 0;

    //blur variables
    [SerializeField]
    private float unblurSpeed = .5f;
    private DepthOfField depth;
    [SerializeField]
    private Vector2 blurLerpRange = new Vector2(0, 5);
    private float blurTarget = 0;

    //color grading variables
    [SerializeField]
    private float colorSpeed = 5f;
    private ColorGrading color;
    [SerializeField]
    private Vector2 colorLerpRange = new Vector2(-50, 0);
    private float colorTarget = 0;
    private float colorContrastTarget = 0;
    [SerializeField]
    private float colorPostSpeed = 1;
    [SerializeField]
    private Vector2 colorPostLerpRange = new Vector2(-50, 0);
    private float colorPostTarget = 0;




    // Start is called before the first frame update
    void Start()
    {
        //set the distortion variables
        volume = GetComponent<PostProcessVolume>();
        volume.enabled = true;

        volume.profile.TryGetSettings<Grain>(out grain);
        volume.profile.TryGetSettings<LensDistortion>(out lens);
        volume.profile.TryGetSettings<DepthOfField>(out depth);
        volume.profile.TryGetSettings<ColorGrading>(out color);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        DistortScreen();

        UnBlurScreen();

    }



    private void DistortScreen()
    {
        lens.intensity.value = Mathf.Lerp(lens.intensity.value, lensTarget, Time.deltaTime * distortionSpeed);
        if (Mathf.Abs(lens.intensity.value - lensTarget) < 10) { lensTarget = Random.Range(lensDistorionRange.x, lensDistorionRange.y); }



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
        //change blur
        depth.focusDistance.value = Mathf.Lerp(depth.focusDistance.value, blurTarget, unblurSpeed * Time.deltaTime);
        if(Mathf.Abs(depth.focusDistance.value - blurTarget) < .5f) { blurTarget = Random.Range(blurLerpRange.x, blurLerpRange.y); }

        //change saturation
        color.saturation.value = Mathf.Lerp(color.saturation.value, colorTarget, colorSpeed * Time.deltaTime);
        if(Mathf.Abs(color.saturation.value - blurTarget) < .5f) { colorTarget = Random.Range(colorLerpRange.x, colorLerpRange.y); }

        //change contrast
        color.contrast.value = Mathf.Lerp(color.saturation.value, colorContrastTarget, colorSpeed * Time.deltaTime);
        if(Mathf.Abs(color.contrast.value - colorContrastTarget) < .5f) { colorContrastTarget = Random.Range(colorLerpRange.x, colorLerpRange.y); }

        //change exposure
        color.postExposure.value = Mathf.Lerp(color.postExposure, colorPostTarget, colorSpeed * Time.deltaTime);
        if(Mathf.Abs(color.postExposure.value - colorPostTarget) < .5f) { colorPostTarget = Random.Range(colorPostLerpRange.x, colorPostLerpRange.y); }
    }
}
