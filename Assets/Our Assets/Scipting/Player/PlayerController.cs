using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //This script is moves the hero within the game


    //managing variables
    private bool gameOver = false;


    //movement variables
    private CharacterController controller;
    public float horizontalSpeed = 10f;
    public float verticalSpeed = 5f;


    //hitting varaibles

    private bool hitting = false;
    public float hitTime = 2f;


    //visual variables
    [SerializeField]
    private Transform heroTransform;
    [SerializeField]
    private GameObject hurtBox;


    //animation varaibles


    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        hurtBox.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(!gameOver)
        {
            PlayerControls();
        }
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            gameOver = true;
        }
    }

    //this container function recieves and reacts to all player input
    private void PlayerControls()
    {
        //check to see if the player threw a punch
        Hit();

        //if the player is not punching, then allow them to move
        if(!hitting)
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
        if(!hitting && Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(HittingDuration());
        }
    }

    IEnumerator HittingDuration()
    {
        hitting = true;
        hurtBox.SetActive(true);
        yield return new WaitForSeconds(hitTime);
        hurtBox.SetActive(false);
        hitting = false;
    }
}
