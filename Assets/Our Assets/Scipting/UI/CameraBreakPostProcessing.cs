using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class CameraBreakPostProcessing : MonoBehaviour
{

    //post proccessing variables
    [SerializeField]
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




    // Start is called before the first frame update
    void Start()
    {
        volume = GetComponent<PostProcessVolume>();

        volume.profile.TryGetSettings<Grain>(out grain);
        volume.profile.TryGetSettings<LensDistortion>(out lens);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        DistortScreen();
    }


    private void DistortScreen()
    {
        lens.intensity.value = Mathf.Lerp(lens.intensity.value, lensTarget, Time.deltaTime * 50);
        if(Mathf.Abs(lens.intensity.value - lensTarget) < 10) {lensTarget = Random.Range(lensDistorionRange.x, lensDistorionRange.y);}

        lens.intensityX.value = Mathf.Lerp(lens.intensityX.value, xMultTarget, Time.deltaTime * 50);
        if (Mathf.Abs(lens.intensityX.value - xMultTarget) < .1) { xMultTarget = Random.Range(multRange.x, multRange.y); };

    }
}
