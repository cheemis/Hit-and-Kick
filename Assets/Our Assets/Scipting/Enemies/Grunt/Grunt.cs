using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Grunt : MonoBehaviour
{
    //This script manages the enemy that will try and kill the player

    //movement variables
    private CharacterController controller;
    public Vector2 chaseSpeedRange;
    private float chaseSpeed = 1f;

    //targetting player variables
    [SerializeField]
    private Transform playerTarget;


    //managing self variables
    private bool alreadyPunched = false;


    //other entities
    public EnemyManager enemyManager;



    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        GruntMovement();
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



    public void InstantiateGrunt(EnemyManager enemyManager, Transform playerTransform)
    {
        this.enemyManager = enemyManager;
        playerTarget = playerTransform;
        chaseSpeed = Random.Range(chaseSpeedRange.x, chaseSpeedRange.y);
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
