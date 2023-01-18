using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class playerScript : MonoBehaviour
{
    float speed;
    float _maxSpeed;
    float _increaseSpeed;
    float _microAdjustSpeed;
    float _verticalVelocity;
    bool _onGround = false;
    bool _jumpInEffect = false;
    bool _upsideDown = false;
    Rigidbody2D body;
    Gamepad controller1;

    float jumpLiftOffTime = 0f;
    float jumpRotation = 0f;
    string lastDirect = "right";
    string jumpStyle = "charming";

    // Start is called before the first frame update
    void Awake()
    {
        speed = 0f;
        body = GetComponent<Rigidbody2D>();
        _maxSpeed = 10f;
        _increaseSpeed = 85f;
        _microAdjustSpeed = 15f;
        _verticalVelocity = 0f;

        for (int i = 0; i < Gamepad.all.Count; i++)
        {
            controller1 = Gamepad.all[0];
        }
        
        //controller1 = Gamepad.all[0];
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(controller1.leftStick.x.ReadValue());
        if (Input.GetKeyDown(KeyCode.N) || controller1.leftShoulder.isPressed)
        {
            jumpStyle = "charming";
            if (!_onGround)
            {
                _jumpInEffect = false;
            }
        }
        else if (Input.GetKeyDown(KeyCode.M) || controller1.rightShoulder.isPressed)
        {
            jumpStyle = "melancholy";
            if (!_onGround)
            {
                _jumpInEffect = false;
            }
            
        }
        
        switch (jumpStyle)
        {
            case "charming":
                if (_jumpInEffect)
                {

                    if (_verticalVelocity > 3f)
                    {
                        _verticalVelocity -= _verticalVelocity * 5 * Time.deltaTime;
                    }
                    else if ((!_upsideDown && jumpRotation < 180 - 30) || (_upsideDown && jumpRotation > 30))
                    {
                        _verticalVelocity -= _verticalVelocity * 5 * Time.deltaTime;

                        if (_upsideDown == true)
                        {
                            jumpRotation = Mathf.Lerp(jumpRotation, 0f, Time.deltaTime * 10f);
                        }
                        else
                        {
                            jumpRotation = Mathf.Lerp(jumpRotation, 180f, Time.deltaTime * 10f);
                        }

                        switch (lastDirect)
                        {
                            case "right":
                                if (_upsideDown)
                                {
                                    transform.rotation = Quaternion.Euler(0f, 0f, -jumpRotation);
                                }
                                else
                                {
                                    transform.rotation = Quaternion.Euler(0f, 0f, jumpRotation);
                                }

                                break;

                            case "left":
                                if (_upsideDown)
                                {
                                    transform.rotation = Quaternion.Euler(0f, 0f, jumpRotation);
                                }
                                else
                                {
                                    transform.rotation = Quaternion.Euler(0f, 0f, -jumpRotation);
                                }

                                break;
                        }
                    }
                    else
                    {
                        if (_upsideDown == true)
                        {
                            jumpRotation = Mathf.Lerp(jumpRotation, 0f, Time.deltaTime * 10f);
                        }
                        else
                        {
                            jumpRotation = Mathf.Lerp(jumpRotation, 360 / 2, Time.deltaTime * 10f);
                        }
                        
                        switch (lastDirect)
                        {
                            case "right":
                                if (_upsideDown)
                                {
                                    transform.rotation = Quaternion.Euler(0f, 0f, -jumpRotation);
                                }
                                else
                                {
                                    transform.rotation = Quaternion.Euler(0f, 0f, jumpRotation);
                                }
                                
                                break;

                            case "left":
                                if (_upsideDown)
                                {
                                    transform.rotation = Quaternion.Euler(0f, 0f, jumpRotation);
                                }
                                else
                                {
                                    transform.rotation = Quaternion.Euler(0f, 0f, -jumpRotation);
                                }
                                
                                break;
                        } 
                        _verticalVelocity -= 45f * Time.deltaTime;
                    }

                }
                else
                {
                    body.gravityScale = 7f;
                    transform.rotation = Quaternion.Euler(0f, 0f, 0f);
                }

                break;

            case ("melancholy"):

                if (_jumpInEffect)
                {
                    if (_verticalVelocity > 1f)
                    {
                        _verticalVelocity -= _verticalVelocity * 5 * Time.deltaTime;
                    }
                    else
                    {
                        if (Input.GetKey(KeyCode.RightArrow) && Input.GetKey(KeyCode.LeftArrow))
                        {
                            Debug.Log("Both Pressed");
                            jumpRotation = Mathf.Lerp(jumpRotation, 0f, Time.deltaTime * 3f);
                            transform.rotation = Quaternion.Euler(0f, 0f, jumpRotation);
                        }
                        else if (Input.GetKey(KeyCode.LeftArrow) || controller1.leftStick.left.isPressed)
                        {
                            jumpRotation = Mathf.Lerp(jumpRotation, -25f, Time.deltaTime * 3f);
                            transform.rotation = Quaternion.Euler(0f, 0f, jumpRotation);
                        }
                        else if (Input.GetKey(KeyCode.RightArrow) || controller1.leftStick.right.isPressed)
                        {
                            jumpRotation = Mathf.Lerp(jumpRotation, 25f, Time.deltaTime * 3f);
                            transform.rotation = Quaternion.Euler(0f, 0f, jumpRotation);
                        }
                        else
                        {
                            jumpRotation = Mathf.Lerp(jumpRotation, 0f, Time.deltaTime * 3f);
                            transform.rotation = Quaternion.Euler(0f, 0f, jumpRotation);
                        }
                        _verticalVelocity -= 2f * Time.deltaTime;
                        _verticalVelocity = Mathf.Clamp(_verticalVelocity, -1.5f, 1f);
                        speed = Mathf.Clamp(speed, -4f, 4f);
                    }

                }
                else
                {
                    body.gravityScale = 7f;
                    transform.rotation = Quaternion.Euler(0f, 0f, 0f);
                }

                break;
        }
        //transform.position = new Vector2(transform.position.x + speed, transform.position.y);
        if (_jumpInEffect)
        {
            body.velocity = new Vector2(speed, _verticalVelocity);
        }
        else
        {
            body.velocity = new Vector2(speed, body.velocity.y);
        }
        
        if (Input.GetKey(KeyCode.LeftArrow) && Input.GetKey(KeyCode.RightArrow))
        {
            if (_onGround)
            {
                speed = Mathf.Lerp(speed, 0f, _increaseSpeed * Time.deltaTime);
            }
            else
            {
                speed = Mathf.Lerp(speed, 0f, _microAdjustSpeed / 3f * Time.deltaTime);
            }

        }
        else if (Input.GetKey(KeyCode.LeftArrow) || controller1.leftStick.left.isPressed)
        {
            if (_onGround)
            {
                lastDirect = "left";
                if (body.velocity.x > 0)
                {
                    speed += -_increaseSpeed * 2f * Time.deltaTime;

                }
                else
                {
                    speed += -_increaseSpeed * Time.deltaTime;
                }
            }
            else
            {
                if (body.velocity.x > 0)
                {
                    speed += -_microAdjustSpeed * 2f * Time.deltaTime;

                }
                else
                {
                    speed += -_microAdjustSpeed * Time.deltaTime;
                }
            }
            
            
        }
        else if (Input.GetKey(KeyCode.RightArrow) || controller1.leftStick.right.isPressed)
        {
            if (_onGround)
            {
                lastDirect = "right";
                if (body.velocity.x < 0)
                {
                    speed += _increaseSpeed * 2 * Time.deltaTime;
                }
                else
                {
                    speed += _increaseSpeed * Time.deltaTime;
                }
            }
            else
            {
                if (body.velocity.x > 0)
                {
                    speed += _microAdjustSpeed * 2f * Time.deltaTime;
                }
                else
                {
                    speed += _microAdjustSpeed * Time.deltaTime;
                }
            }
        }
        else
        {
            if (_onGround)
            {
                speed = Mathf.Lerp(speed, 0f, _increaseSpeed * Time.deltaTime);
            }
            else
            {
                speed = Mathf.Lerp(speed, 0f, _microAdjustSpeed/3f * Time.deltaTime);
            }
        }

        speed = Mathf.Clamp(speed, -_maxSpeed, _maxSpeed);


        //Jump
        if (Input.GetKeyDown(KeyCode.UpArrow) || controller1.aButton.isPressed)
        {
            if (_onGround)
            {
                //body.AddForce(new Vector2(0, 1500f));
                _verticalVelocity = 25f;
                
                _jumpInEffect = true;
                body.gravityScale = 0;

                //setTimer
                jumpLiftOffTime = .3f;
            }
            
        }
        //Reduce Timers
        if (jumpLiftOffTime > 0)
        {
            jumpLiftOffTime -= Time.deltaTime;
        }

    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        _onGround = false;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        for (int i=0; i < collision.contactCount; i++)
        {
            Vector2 norm = collision.GetContact(i).normal;
            _onGround |= (norm.y >= .9f);

            if (jumpLiftOffTime <= 0 && _onGround)
            {
                _jumpInEffect = false;
                if (jumpRotation > -20f && jumpRotation < 20f)
                {
                    jumpRotation = 0f;
                    _upsideDown = false;
                }
                else
                {
                    jumpRotation = 180f;
                    _upsideDown = true;
                    
                }
                
            }
            
        }
    }
}