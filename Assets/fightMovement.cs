using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class fightMovement : MonoBehaviour
{
    [SerializeField] GameObject pole;
    [SerializeField] GameObject root;
    

    Gamepad controller1;
    float dashSpeed = 40f;
    float dashSpeedMax = 40f;
    float counter = .5f;
    float counterMax = .5f;
    public bool inAttack = false;
    Vector2 dashDirect;
    // Start is called before the first frame update
    void Start()
    {
        controller1 = GetComponent<bambooPlayerScript>().controller1;
    }

    // Update is called once per frame
    void Update()
    {
        if (controller1 == null)
        {
            controller1 = GetComponent<bambooPlayerScript>().controller1;
        }
        if (counter > 0 && !root.GetComponent<CollExpScript>().grounded)
        {
            counter -= Time.deltaTime;
            dashSpeed -= Time.deltaTime * dashSpeedMax * 2f;
            inAttack = true;
            pole.GetComponent<Rigidbody2D>().velocity = dashDirect * dashSpeed;
            
        }
        else
        {
            counter = 0;
            inAttack = false;
            dashSpeed = dashSpeedMax;
        }
        
        float vertical = 0;
        float horizontal = 0;
        if (!root.GetComponent<CollExpScript>().grounded)
        {
            if (controller1.buttonWest.wasPressedThisFrame && counter <= 0)
            {
                
                if (controller1.leftStick.up.isPressed)
                {
                    vertical += 1;
                }
                if (controller1.leftStick.down.isPressed)
                {
                    vertical -= 1;
                }
                if (controller1.leftStick.right.isPressed)
                {
                    horizontal += 1;
                }

                if (controller1.leftStick.left.isPressed)
                {
                    horizontal -= 1;
                }

                if (horizontal == 0 && vertical == 0)
                {
                    vertical = -1;
                }

                if (vertical <= 0)
                {
                    counter = counterMax; counter = counterMax;
                    dashDirect = new Vector2(horizontal, vertical).normalized;
                    pole.GetComponent<Rigidbody2D>().velocity = dashDirect * dashSpeed;
                    pole.GetComponent<Rigidbody2D>().angularVelocity = 0f;
                    float angle = Vector2.Angle(new Vector2(horizontal, vertical), Vector3.up);
                    if (horizontal < 0)
                    {
                        pole.transform.rotation = Quaternion.Euler(0f, 0f, angle + 180);
                    }
                    else
                    {
                        pole.transform.rotation = Quaternion.Euler(0f, 0f, 180 - angle);
                    }
                }
                
                
            }
        }
    }
}
