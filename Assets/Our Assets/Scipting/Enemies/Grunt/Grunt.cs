using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public class Grunt : MonoBehaviour
{
    //This script manages the enemy that will try and kill the player


    //managing self variables
    private bool allowedToMove = true;
    private bool alreadyPunched = false;
    private Vector3 home;

    public enum enemyState
    {
        none,
        walking,
        idle,
        waitingUpToPunch,
        punching,
        gettingUp,
        dying
    }
    [SerializeField]
    private enemyState currentState = enemyState.walking;

    [Space(20)]

    //movement variables
    private CharacterController controller;
    public Vector2 chaseSpeedRange;
    private float chaseSpeed = 1f;

    [Space(20)]

    //punching variables
    [SerializeField]
    private float distanceToTarget = .1f; //distance to initiate a punch
    [SerializeField]
    private float timeToHitPlayer = 1f; //time to wait before punching player
    private float hitTime = 0; //used to calculate when the enemy should punch

    [SerializeField]
    private float timeToPunchPlayer = .5f;
    private float punchTime = 0;

    [SerializeField]
    GameObject punchingHitbox;

    [Space(20)]

    //idle variables
    [SerializeField]
    private Vector2 idleTimeRange = new Vector2(.5f, 2f);
    private float idleTime = 0;

    [Space(20)]

    //targetting player variables
    [SerializeField]
    private Transform playerTargetParent;
    private Transform standingTarget;
    private int standingTargetIndex = 0;

    [Space(20)]


    //other entities
    public EnemyManager enemyManager;
    [SerializeField]
    private LocationsManager locationManager; //this is passed from EnemyManager when instantiated



    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        home = transform.position;
        punchingHitbox.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(allowedToMove)
        {
            CurrentEnemyState();
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
        if (other.gameObject.tag == "HurtBox" && !alreadyPunched)
        {
            Death();
        }
    }

    private void CurrentEnemyState()
    {
        switch (currentState)
        {
            case enemyState.none:
                break;
            case enemyState.walking: // the grunt is walking towards the player
                GruntMovement();
                break;
            case enemyState.idle: //waiting after punch to begin moving
                Idling();
                break;
            case enemyState.waitingUpToPunch: //the grunt has arrived at the player and is winding up a punch
                WaitForPunch();
                break;
            case enemyState.punching: //the enemy is punching the player
                Punching();
                break;
            case enemyState.gettingUp: //the enemy is getting up after the TV was kicked
                break;
            case enemyState.dying: //The enemy was kicked by the player
                break;
            default:
                break;


        }

    }
    

    private void GruntMovement()
    {
        //find the direction to move the enemy
        Vector3 target = standingTarget.position - transform.position;
        target.y = 0;
        target.Normalize();
        controller.Move(target * chaseSpeed *Time.deltaTime);

        //find out which direction to face
        if(transform.position.x < playerTargetParent.position.x)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }


        //see if close enough to punch player
        if (Vector3.Distance(transform.position, standingTarget.position) < distanceToTarget)
        {
            Debug.Log(this.gameObject.name + " has arrived at " + standingTarget.gameObject.name);
            currentState = enemyState.waitingUpToPunch;
            hitTime = Time.time + timeToHitPlayer;

        }
    }


    private void WaitForPunch()
    {
        if(Time.time > hitTime)
        {
            currentState = enemyState.punching;
            punchTime = Time.time + timeToPunchPlayer;
            punchingHitbox.SetActive(true);
        }
    }

    private void Punching() //this is just a clock for punching
    {
        if (Time.time > punchTime)
        {
            currentState = enemyState.idle;
            punchingHitbox.SetActive(false);
            idleTime = Time.time + Random.Range(idleTimeRange.x, idleTimeRange.y);
        }
    }

    private void Idling()
    {
        if (Time.time > idleTime)
        {
            currentState = enemyState.walking;
            standingTarget = playerTargetParent.GetChild(Random.Range(0, playerTargetParent.childCount)); //switch to random target location


        }
    }



    public void InstantiateGrunt(EnemyManager enemyManager, Transform playerTransform, int playerTransformChild, LocationsManager locationManager)
    {
        Debug.Log("set location manager? " + (locationManager != null));
        this.enemyManager = enemyManager;
        this.locationManager = locationManager;

        playerTargetParent = playerTransform;
        standingTarget = playerTransform.GetChild(playerTransformChild);
        standingTargetIndex = playerTransformChild;
        

        chaseSpeed = Random.Range(chaseSpeedRange.x, chaseSpeedRange.y);
    }


    private void OnKick()
    {
        //get a random spawn location
        Vector3 spawnLocation = locationManager == null ? Vector3.zero : locationManager.GetRandomLocation();


        if (spawnLocation == Vector3.zero) spawnLocation = home;


        //teleport player to new location
        controller.enabled = false;
        transform.position = new Vector3(spawnLocation.x, transform.position.y, spawnLocation.z);
        controller.enabled = true;


        allowedToMove = false;
    }

    private void OnRecoverFromKick()
    {
        allowedToMove = true;
    }


    private void Death()
    {
        alreadyPunched = true;

        if (enemyManager != null)
        {
            enemyManager.DespawnEnemy();
        }

        Destroy(this.gameObject); //for some reason, this doesnt delete the colider right away, so "alreadyPunched" is needed
    }
}
