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

    //global variables
    public static bool gameOver = false;

    //event variables
    public delegate void OnKickTV();
    public static OnKickTV onKickTV;

    public delegate void OnRecoverFromKick();
    public static OnRecoverFromKick onRecoverFromKick;

    //kicking variables
    [SerializeField]
    private GameObject tvStatic;
    public float kickTime = 1f;

    //UI variables
    //[SerializeField]
    //private TextMeshProUGUI textmesh;

    //Audio
    //[SerializeField]
    private AudioSource kickHero;
    //[SerializeField]
    //private AudioSource gameOver;


    //other entities variables
    private LocationsManager locationsManager;
    public GameObject gameOverText; // TEMP VARIABLE
    [SerializeField]
    private MusicManager musicManager;
    [SerializeField]
    private ScoreBoard scoreBoard;

    public Animator leg;



    // Start is called before the first frame update
    void Start()
    {
        //foot.SetActive(false);
        tvStatic.SetActive(false);

        locationsManager = GameObject.FindGameObjectWithTag("SpawningManager")?.GetComponent<LocationsManager>();
        kickHero = GetComponent<AudioSource>();

        gameOver = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(!gameOver && !kicking && Input.GetKeyDown(KeyCode.K))
        {
            kicking = true;
            locationsManager.ResetLocations();
            leg.SetTrigger("kick");
            StartCoroutine(WaitforKick());
        }

        else if (gameOver)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadSceneAsync(0);
            }
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

        //foot.SetActive(true);
        tvStatic.SetActive(true);
        
    }

    public void RecoverFromKickingTV()
    {
        kicking = false;

        kicksLeft--;
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
            EndGame();
        }

        //foot.SetActive(false);
    }

    public void EndGame()
    {
        gameOver = true;
        musicManager.GameOverMusic();
        gameOverText.SetActive(true);
        if (scoreBoard != null)
        {
            string gameOverString = //send help
                "Game Over" +
                "\r\n" +
                "<color=white>Score: </color><color=green>" + scoreBoard.currentScore + "</color>" +
                "\r\n" +
                "<size=42>Press 'R' to restart\r\n</size>";


            gameOverText.GetComponent<TextMeshProUGUI>().text = gameOverString;
        }
    }
    IEnumerator WaitforKick()
    {
        yield return new WaitForSeconds(kickTime);
        onKickTV.Invoke();
        kickHero.Play();
    }
}
