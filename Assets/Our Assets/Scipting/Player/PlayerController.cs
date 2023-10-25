using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
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

    [Space(20)]
    //audio variables
    [SerializeField]
    private AudioClip[] punchSfx;
    [SerializeField]
    private AudioSource fightAudioSource;
    [SerializeField]
    private AudioSource kickComputer;
    [SerializeField]
    private AudioSource gameOver;

    public enum playerAction
    {
        noAction,
        hit,
        kick,
    }

    playerAction[] actionList = new playerAction[3];


    //hitting varaibles
    private bool hitting = false;
    private bool kicking = false;
    public float hitTime = 2f;
    public float kickTime = 2f;

    private bool actioned = false;

    //hurtbox variables
    [SerializeField]
    private HurtBox punchingHurtBox;


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
            gameOver.Play();
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
        // Tom thoughts: we may need to release this constraint once we want to run-hit
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
        //audio 
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
        if (actioned)
        {
            return;
        }
        if(!hitting && Input.GetKeyDown(KeyCode.Comma))
        {
            StartCoroutine(HittingDuration());
        }
    }

    private void Kick()
    {
        if (actioned)
        {
            return;
        }
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
        //audio
        kickComputer.Play();
        //stop player from moving
        playerCanMove = false;
    }

    private void OnRecoverFromKick()
    {
        playerCanMove = true;
    }



    IEnumerator HittingDuration()
    {
        actioned = true;
        hitting = true;
        
        for (int i = 0; i < actionList.Length - 1; i++)
        {
            actionList[i] = actionList[i + 1];
        }
        actionList[actionList.Length - 1] = playerAction.hit;

        hurtBox.SetActive(true);

        /*
        //Adding a raycaset to detect the first 3 enermies.
        int enermyLayer = 7;
        RaycastHit[] hits = Physics.SphereCastAll(heroTransform.position, 5, transform.forward, 1.573f, enermyLayer);
        foreach (var hit in hits)
        {
            if (hit.collider.gameObject.layer == enermyLayer)
            {
                Debug.Log(hit.collider.gameObject.name);
            }            
        }
        */

        HurtBox box = hurtBox.GetComponent<HurtBox>(); // "side note, get this as a global in start method" - Nick
        box.hitCounter = 2;
        //box.activate = true;
        //audio
        fightAudioSource.clip = punchSfx[Random.Range(0, 2)];
        fightAudioSource.Play();

        yield return new WaitForSeconds(hitTime);
        //box.activate = false;
        hurtBox.SetActive(false);
        hitting = false;
        actioned = false;
    }

    IEnumerator KickingDuration()
    {
        actioned = true;
        kicking = true;
        
        for (int i = 0; i < actionList.Length - 1; i++)
        {
            actionList[i] = actionList[i + 1];
        }
        actionList[actionList.Length - 1] = playerAction.kick;


        Vector3 oldSize = hurtBox.transform.localScale;

        bool combo = false;
        if (actionList[0] == playerAction.kick && actionList[1] == playerAction.hit && actionList[2] == playerAction.kick)
        {
            // make a combo here, so let kick become a big kick.
            float x = hurtBox.transform.localScale.x;
            float y = hurtBox.transform.localScale.y;
            float z = hurtBox.transform.localScale.z;
            hurtBox.transform.localScale = new Vector3(2 * x,2 * y,2 * z);
            //clear the list
            actionList[0] = actionList[1] = actionList[2] = playerAction.noAction;
            combo= true;
        }

        Vector3 oldPosition = hurtBox.transform.localPosition;
        hurtBox.transform.localPosition = new Vector3(1.573f, -0.5f, 0);  //hardcode
        hurtBox.SetActive(true);
        HurtBox box = hurtBox.GetComponent<HurtBox>();
        box.hitCounter = 2;
        //box.activate = true;
        //audio
        if (combo)
        {
            fightAudioSource.clip = punchSfx[Random.Range(6, 8)];
            fightAudioSource.Play();
        }
        else
        {
            fightAudioSource.clip = punchSfx[Random.Range(3, 5)];
            fightAudioSource.Play();
        }


        yield return new WaitForSeconds(kickTime);
        //box.activate = false;
        hurtBox.SetActive(false);
        hurtBox.transform.localPosition = oldPosition;
        hurtBox.transform.localScale = oldSize;
        kicking = false;
        actioned = false;
    }
}
