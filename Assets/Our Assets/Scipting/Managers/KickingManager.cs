using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using UnityEngine.SceneManagement;

public class KickingManager : MonoBehaviour
{

    //management variables
    private bool kicking = false;
    private int kicksLeft = 5;



    //event variables
    public delegate void OnKickTV();
    public static OnKickTV onKickTV;

    public delegate void OnRecoverFromKick();
    public static OnRecoverFromKick onRecoverFromKick;


    //kicking variables
    [SerializeField]
    private GameObject foot;
    [SerializeField]
    private GameObject tvStatic;
    public float kickTime = 1f;

    //UI variables
    [SerializeField]
    private TextMeshProUGUI textmesh;

    //Audio
    [SerializeField]
    private AudioSource kickHero;
    [SerializeField]
    private AudioSource gameOver;


    //other entities variables
    private LocationsManager locationsManager;
    public GameObject gameOverText; // TEMP VARIABLE



    // Start is called before the first frame update
    void Start()
    {
        foot.SetActive(false);
        tvStatic.SetActive(false);
        textmesh.text = "Kicks left: " + kicksLeft;

        locationsManager = GameObject.FindGameObjectWithTag("SpawningManager")?.GetComponent<LocationsManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!kicking && Input.GetKeyDown(KeyCode.K))
        {

            locationsManager.ResetLocations();
            onKickTV.Invoke();
            kickHero.Play();
        }

        if(Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadSceneAsync(0);
        }
    }

    
    private void OnEnable()
    {
        onKickTV += KickTV;
        onRecoverFromKick += RecoverFromKickingTV;
    }

    private void OnDisable()
    {
        onKickTV -= KickTV;
        onRecoverFromKick -= RecoverFromKickingTV;
    }
    

    private void KickTV()
    {

        StartCoroutine(KickingDuration());

        foot.SetActive(true);
        tvStatic.SetActive(true);
        
    }

    public void RecoverFromKickingTV()
    {
        kicking = false;

        kicksLeft--;
        textmesh.text = "Kicks left: " + kicksLeft;
    }

    IEnumerator KickingDuration()
    {
        kicking = true;
        yield return new WaitForSeconds(kickTime);



        if(kicksLeft > 0)
        {
            
            tvStatic.SetActive(false);
            onRecoverFromKick.Invoke();
        }
        else
        {
            gameOver.Play();
            gameOverText.SetActive(true);
            
        }

        foot.SetActive(false);
    }
}
