using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Grunt : MonoBehaviour
{
    //This script manages the enemy that will try and kill the player


    //managing self variables
    private bool allowedToMove = true;
    private bool alreadyPunched = false;
    private Vector3 home;


    //movement variables
    private CharacterController controller;
    public Vector2 chaseSpeedRange;
    private float chaseSpeed = 1f;

    //targetting player variables
    [SerializeField]
    private Transform playerTarget;


    


    //other entities
    public EnemyManager enemyManager;
    [SerializeField]
    private LocationsManager locationManager; //this is passed from EnemyManager when instantiated



    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        home = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(allowedToMove)
        {
            GruntMovement();
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


    private void GruntMovement()
    {
        Vector3 target = playerTarget.position - transform.position;
        target.y = 0;
        target.Normalize();
        controller.Move(target * chaseSpeed *Time.deltaTime);
    }



    public void InstantiateGrunt(EnemyManager enemyManager, Transform playerTransform, LocationsManager locationManager)
    {
        Debug.Log("set location manager? " + (locationManager != null));
        this.enemyManager = enemyManager;
        this.locationManager = locationManager;
        
        playerTarget = playerTransform;
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
