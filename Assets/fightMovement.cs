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
    float dashSpeedMax = 80;
    float counter = .5f;
    float counterMax = .7f;
    public bool inAttack = false;
    Vector2 dashDirect;
    // Start is called before the first frame update
    private void Reset()
    {
        controller1 = GetComponent<bambooPlayerScript>().controller1;
    }
    void Start()
    {
        controller1 = GetComponent<bambooPlayerScript>().controller1;
    }

    // Update is called once per frame
    void Update()
    {
        if (!GetComponent<bambooPlayerScript>().dead) 
        { 
            if (controller1 == null)
            {
                controller1 = GetComponent<bambooPlayerScript>().controller1;
            }
            counter -= Time.deltaTime;
            if (dashSpeed > 0 && inAttack)
            {
                dashSpeed -= Time.deltaTime * dashSpeedMax * 10f;
                pole.GetComponent<Rigidbody2D>().velocity = dashDirect * dashSpeed;
            
            }
            if (counter <= 0 || root.GetComponent<CollExpScript>().grounded)
            {
                inAttack = false;
                counter = 0;
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
                        dashSpeed = dashSpeedMax;
                        inAttack = true;
                        counter = counterMax;
                        dashDirect = new Vector2(horizontal, vertical).normalized;
                        pole.GetComponent<Rigidbody2D>().velocity = dashDirect * dashSpeed;
                        pole.GetComponent<Rigidbody2D>().angularVelocity = 0f;
                        float angle = Vector2.Angle(new Vector2(horizontal, vertical), Vector3.up);
                        if (horizontal < 0)
                        {
                            pole.transform.rotation = Quaternion.Euler(0f, 0f, 180 + angle);
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
}
