using System.Collections;
using System.Collections.Generic;
//using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //This script is moves the hero within the game


    //managing variables
    private bool playerCanMove = true;


    //movement variables
    private CharacterController controller;
    public float horizontalSpeed = 10f;
    public float verticalSpeed = 5f;


    //hitting varaibles
    private bool hitting = false;
    private bool kicking = false;
    public float hitTime = 2f;
    public float kickTime = 2f;


    //visual variables
    [SerializeField]
    private Transform heroTransform;
    [SerializeField]
    private GameObject hurtBox;


    //animation variables



    //other entities variables
    private LocationsManager locationsManager;
    public GameObject gameOverText; // TEMP VARIABLE


    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        hurtBox.SetActive(false);

        locationsManager = GameObject.FindGameObjectWithTag("SpawningManager")?.GetComponent<LocationsManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(playerCanMove)
        {
            PlayerControls();
        }
        
    }

    private void OnEnable()
    {
        KickingManager.onKickTV += OnKick;
        KickingManager.onRecoverFromKick += OnRecoverFromKick;
    }

    private void OnDisable()
    {
        KickingManager.onKickTV -= OnKick;
        KickingManager.onRecoverFromKick -= OnRecoverFromKick;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            gameOverText.SetActive(true);
            playerCanMove = false;
        }
    }

    //this container function recieves and reacts to all player input
    private void PlayerControls()
    {
        //check to see if the player threw a punch
        Hit();

        Kick();

        //if the player is not punching, then allow them to move
        if(!hitting && !kicking)
        {
            MoveHero();
            TurnHero(Input.GetKey(KeyCode.D), Input.GetKey(KeyCode.A));
        }
        
    }

    private void MoveHero()
    {
        float upwardSpeed = ((Input.GetKey(KeyCode.W) ? 1 : 0) +
                            (Input.GetKey(KeyCode.S) ? -1 : 0)) *
                            horizontalSpeed * Time.deltaTime;

        float rightwardSpeed = ((Input.GetKey(KeyCode.D) ? 1 : 0) +
                               (Input.GetKey(KeyCode.A) ? -1 : 0)) *
                               horizontalSpeed * Time.deltaTime;

        controller.Move(new Vector3(rightwardSpeed, 0, upwardSpeed));
    }

    private void TurnHero(bool facingRight, bool facingLeft)
    {
        if(facingRight)
        {
            heroTransform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (facingLeft)
        {
            heroTransform.rotation = Quaternion.Euler(0, 180, 0);
        }
    }

    private void Hit()
    {
        if(!hitting && Input.GetKeyDown(KeyCode.Comma))
        {
            StartCoroutine(HittingDuration());
        }
    }

    private void Kick()
    {
        if (!kicking && Input.GetKeyDown(KeyCode.Period))
        {
            StartCoroutine(KickingDuration());
        }
    }

    private void OnKick()
    {
        //get a random spawn location
        Vector3 spawnLocation = locationsManager == null ? Vector3.zero : locationsManager.GetRandomLocation();

        //teleport player to new location
        controller.enabled = false;
        transform.position = new Vector3(spawnLocation.x, transform.position.y, spawnLocation.z);
        controller.enabled = true;

        //stop player from moving
        playerCanMove = false;
    }

    private void OnRecoverFromKick()
    {
        playerCanMove = true;
    }



    IEnumerator HittingDuration()
    {
        hitting = true;
        hurtBox.SetActive(true);
        yield return new WaitForSeconds(hitTime);
        hurtBox.SetActive(false);
        hitting = false;
    }

    IEnumerator KickingDuration()
    {
        kicking = true;
        Vector3 oldPosition = hurtBox.transform.localPosition;
        hurtBox.transform.localPosition = new Vector3(1.573f, -0.5f, 0);  //hardcode
        hurtBox.SetActive(true);
        yield return new WaitForSeconds(kickTime);
        hurtBox.SetActive(false);
        hurtBox.transform.localPosition = oldPosition;
        kicking = false;
    }
}
