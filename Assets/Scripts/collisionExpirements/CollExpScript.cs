using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CollExpScript : MonoBehaviour
{
    [SerializeField] GameObject pole;
    [SerializeField] GameObject player;
    [SerializeField] GameObject partEff;
    Gamepad controller1;
    public bool grounded = false;
    public bool playerHit = false;
    public bool playerHitConfirmed = false;
    [SerializeField] BoxCollider2D upperBound;
    
    
    // Start is called before the first frame update
    void Start()
    {
        controller1 = player.GetComponent<bambooPlayerScript>().controller1;
        
    }

    // Update is called once per frame
    private void Update()
    {
        if (controller1 == null)
        {
            controller1 = player.GetComponent<bambooPlayerScript>().controller1;
        }
    }
    void FixedUpdate()
    {
        
        //Debug.Log(Physics2D.IsTouchingLayers(GetComponent<BoxCollider2D>(), 1 << 6));
        //detects if collided
        if (Physics2D.IsTouchingLayers(GetComponent<BoxCollider2D>(),1 << 6))
        {
            //Debug.Log("Basically Here");
            if (!grounded)
            {
                pole.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 0f);
                pole.GetComponent<Rigidbody2D>().angularVelocity = 0f;
                pole.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
                pole.GetComponent<Animator>().SetBool("grounded", true);
                pole.GetComponent<Animator>().SetBool("ReleaseFling", false);
                pole.GetComponent<Animator>().SetBool("BeginFling", false);
                pole.GetComponent<Animator>().SetBool("lungeRight", false);
                if (!Physics2D.IsTouchingLayers(player.GetComponent<CircleCollider2D>(), 1 << 6))
                {
                    grounded = true;
                }
                
                //if (controller1.rightShoulder.isPressed)
                //{
                //    player.GetComponent<bambooPlayerScript>().poleGrabbed = true;
                //}
            }
        }
        else
        {
            grounded = false;
            pole.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        }
        if (Physics2D.IsTouchingLayers(upperBound, 1 << 6)){
            grounded = false;
            pole.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        }



        //player attack stuff
        if (Physics2D.IsTouchingLayers(GetComponent<BoxCollider2D>(), 1 << 8))
        {
            playerHit = true;
            
        }
        else
        {
            playerHit = false;
        }

        if (playerHitConfirmed)
        {
            GameObject spawn = Instantiate(partEff);
            spawn.transform.position = transform.position;
            playerHitConfirmed = false;
        }
    }

    

}
