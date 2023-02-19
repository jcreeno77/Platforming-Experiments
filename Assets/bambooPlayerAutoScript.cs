using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class bambooPlayerAutoScript : MonoBehaviour
{
    float speed;
    float baseSpeed;
    float poleHeight;
    float moveWidth;
    //float arialSpeed = 0f;
    float maxArialSpeed = 0f;
    int jumpsLeft = 1;
    int dashesLeft = 1;
    public bool offPole;
    public bool dead;

    [SerializeField] float dashSpeed;
    [SerializeField] float flingForce;
    Vector2 grabLocation;
    Vector2 globalGrabLocation;
    Vector2 flingDirect;
    public bool poleGrabbed;
    [SerializeField] GameObject pole;
    [SerializeField] GameObject controllerRef;
    [SerializeField] PhysicMaterial frictionless;
    [SerializeField] Sprite[] sprArrayLeft;
    [SerializeField] Sprite[] sprArrayRight;
    public Sprite basePole;
    [SerializeField] Sprite spinningPole;

    //Animation
    public Animator animator;
    public float flingDistance;
    public Gamepad controller1;

    // Start is called before the first frame update
    private void Reset()
    {
        for (int i = 0; i < Gamepad.all.Count; i++)
        {
            if (i >= 4)
            {
                break;
            }
            //controller1 = Gamepad.all[i];
            Debug.Log(controllerRef.GetComponent<controllerUsedArray>().controllerArray[0]);
            Debug.Log(i);
            bool iInArr = false;
            foreach (int item in controllerRef.GetComponent<controllerUsedArray>().controllerArray)
            {
                if (i == item)
                {
                    iInArr = true;
                }
            }
            if (!iInArr)
            {
                controllerRef.GetComponent<controllerUsedArray>().controllerArray[i] = i;
                controller1 = Gamepad.all[i];
                break;
            }
        }
    }
    void Start()
    {
        flingForce = 50f;
        dashSpeed = 50f;
        poleGrabbed = false;
        baseSpeed = 7f;
        speed = baseSpeed;

        poleHeight = 5.38f;
        moveWidth = 1f;
        flingDirect = new Vector2(0f, 0f);

        for (int i = 0; i < Gamepad.all.Count; i++)
        {
            if (i >= 4)
            {
                break;
            }
            //controller1 = Gamepad.all[i];
            Debug.Log(controllerRef.GetComponent<controllerUsedArray>().controllerArray[0]);
            Debug.Log(i);
            bool iInArr = false;
            foreach (int item in controllerRef.GetComponent<controllerUsedArray>().controllerArray)
            {
                if (i == item)
                {
                    iInArr = true;
                }
            }
            if (!iInArr)
            {
                controllerRef.GetComponent<controllerUsedArray>().controllerArray[i] = i;
                controller1 = Gamepad.all[i];
                break;
            }
        }
        if (controller1 == null)
        {
            Destroy(transform.parent.gameObject);
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (dead)
        {
            dead = false;
            transform.parent = null;
            gameObject.AddComponent<Rigidbody2D>();
            GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            GetComponent<Rigidbody2D>().angularDrag = 1f;
            GetComponent<Rigidbody2D>().velocity = new Vector2(pole.GetComponent<Rigidbody2D>().velocity.x, pole.GetComponent<Rigidbody2D>().velocity.y);
            jumpsLeft = 0;
            dashesLeft = 0;
        }
        //Debug.Log("working");
        //Debug.Log(pole.GetComponent<bambooPoleAutoScript>().grounded);
        if (pole.GetComponent<bambooPoleAutoScript>().grounded)
        {
            pole.GetComponent<SpriteRenderer>().sprite = basePole;
            //Debug.Log("grounded");
            //Handle Grab Mechanics
            float localFlingDistance = (Mathf.Pow((grabLocation.x - transform.localPosition.x) * pole.transform.localScale.x, 2) + Mathf.Pow((grabLocation.y - transform.localPosition.y) * pole.transform.localScale.y, 2));
            //Debug.Log(localFlingDistance);
            //CONTROLLER VERSION
            if (controller1.rightShoulder.wasPressedThisFrame)
            {
                poleGrabbed = true;
                grabLocation = transform.localPosition;
                flingDirect = new Vector2(0f, 0f);
                //GetComponent<SpriteRenderer>().color = Color.red;
            }
            if (controller1.rightShoulder.isPressed)
            {
                Debug.Log("is pressed");
                float controllerXVal = controller1.leftStick.ReadValue().x;
                float controllerYVal = controller1.leftStick.ReadValue().y;
                flingDirect = new Vector2(Mathf.Lerp(flingDirect.x, -controllerXVal, Time.deltaTime * 15f), Mathf.Lerp(flingDirect.y, -controllerYVal, Time.deltaTime * 15f));

                //GetAngle
                //Debug.Log(Vector2.Angle(pole.transform.up, flingDirect));

                //pole anim
                float select = Vector2.Distance(new Vector2(0, 0), flingDirect) / 1.2f * 4f;
                //animator.SetInteger("selector", (int)select);

                //Debug.Log((int)select);
                float flingAngle = Vector2.Angle(transform.right, -flingDirect);
                if (flingAngle > 90)
                {
                    pole.GetComponent<SpriteRenderer>().sprite = sprArrayLeft[(int)select];
                }
                else
                {
                    pole.GetComponent<SpriteRenderer>().sprite = sprArrayRight[(int)select];
                }

            }
            if (controller1.rightShoulder.wasReleasedThisFrame)
            {
                pole.GetComponent<bambooPoleAutoScript>().grounded = false;
                pole.GetComponent<bambooPoleAutoScript>().timer = .3f;
                float angle = Vector2.Angle(pole.transform.up, flingDirect);
                maxArialSpeed = 0f;
                poleGrabbed = false;
                //GetComponent<SpriteRenderer>().color = Color.white;
                speed = baseSpeed;
                pole.GetComponent<Rigidbody2D>().velocity = new Vector2(flingDirect.x * flingForce, flingDirect.y * flingForce * 1.25f);

                //Handles angular Velocity
                float flingAngle = Vector2.Angle(transform.right, new Vector2(-flingDirect.x, -flingDirect.y));
                float amplifier;
                //Sets amplifier
                if (flingAngle > 90)
                {
                    amplifier = -flingAngle / 180;
                }
                else
                {
                    amplifier = (180 - flingAngle) / 180;
                }
                //Sets Anglular Velocity
                float angVel = (flingDirect.magnitude * 700f * amplifier);
                //Adjusts min and max clamp
                if (angVel >= 0)
                {
                    angVel = Mathf.Clamp(angVel, 300, 700);
                }
                else
                {
                    angVel = Mathf.Clamp(angVel, -700, -300);
                }
                //Activates da spin
                pole.GetComponent<Rigidbody2D>().angularVelocity = angVel;

                pole.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;

                //reset to straight pole: PASS TO ANIMATOR
                // timers stagger reflex animation -> basePole
                //setBool in Animator BEGIN Release
                pole.GetComponent<SpriteRenderer>().sprite = basePole;
                jumpsLeft = 1;
                dashesLeft = 2;
                flingDirect = new Vector2(0f, 0f);
            }
        }
        else
        {
            pole.GetComponent<SpriteRenderer>().sprite = spinningPole;

            //arial Movement Mechanics
            Vector2 currPoleVel = pole.GetComponent<Rigidbody2D>().velocity;
            //arial jump
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                if (jumpsLeft > 0)
                {
                    jumpsLeft -= 1;
                    pole.GetComponent<Rigidbody2D>().velocity = new Vector2(currPoleVel.x, 35f);
                }
            }
            maxArialSpeed = Mathf.Max(maxArialSpeed, Mathf.Abs(currPoleVel.x));
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                pole.GetComponent<Rigidbody2D>().velocity = new Vector2(Mathf.Clamp(currPoleVel.x - (10f * Time.deltaTime), -maxArialSpeed, maxArialSpeed), currPoleVel.y);
            }
            if (Input.GetKey(KeyCode.RightArrow))
            {
                pole.GetComponent<Rigidbody2D>().velocity = new Vector2(Mathf.Clamp(currPoleVel.x + (12f * Time.deltaTime), -maxArialSpeed, maxArialSpeed), currPoleVel.y);
            }

            //AERIAL DASH MECHANICS
            float horizontal = 0;
            float vertical = 0;
            /*
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                if (Input.GetKey(KeyCode.UpArrow))
                {
                    vertical += 1;
                }
                if (Input.GetKey(KeyCode.DownArrow))
                {
                    vertical -= 1;
                }
                if (Input.GetKey(KeyCode.RightArrow))
                {
                    horizontal += 1;
                }

                if (Input.GetKey(KeyCode.LeftArrow))
                {
                    horizontal -= 1;
                }
                if (dashesLeft > 0)
                {
                    dashesLeft -= 1;
                    Vector2 dashDirect = new Vector2(horizontal, vertical);
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
                
            }*/

            //Controller Version
            //aerial Jump
            if (controller1.buttonSouth.wasPressedThisFrame && !GetComponent<fightMovement>().inAttack)
            {
                if (jumpsLeft > 0)
                {
                    jumpsLeft -= 1;
                    pole.GetComponent<Rigidbody2D>().velocity = new Vector2(currPoleVel.x, 35f);
                }
            }

            //aerial Dash
            if (controller1.rightShoulder.wasPressedThisFrame && !GetComponent<fightMovement>().inAttack)
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
                if (dashesLeft > 0 /*&& vertical <= 0*/)
                {
                    dashesLeft -= 1;
                    Vector2 dashDirect = new Vector2(horizontal, vertical).normalized;
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

            //aerial rotation
            if (controller1.rightStick.right.isPressed)
            {
                pole.GetComponent<Rigidbody2D>().angularVelocity -= 500 * Time.deltaTime;
            }
            else if (controller1.rightStick.left.isPressed)
            {
                pole.GetComponent<Rigidbody2D>().angularVelocity += 500 * Time.deltaTime;
            }
            pole.GetComponent<Rigidbody2D>().angularVelocity = Mathf.Clamp(pole.GetComponent<Rigidbody2D>().angularVelocity, -700f, 700f);

        }
    }

}
