using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gullScript : MonoBehaviour
{
    string state = "base";
    GameObject parentObject;
    SpriteRenderer sprRend;
    Rigidbody2D rb;
    Vector2 startPos;
    Animator animator;
    GameObject attackTarget;
    [SerializeField] GameObject particleEffect;
    [SerializeField] float speed;
    float startAnim;

    Vector2 chargePoint;
    float begingFloatTimer;
    float beginAttackTimer;
    float betweenTimer;
    bool attackIn = false;
    
    // Start is called before the first frame update
    void Start()
    {
        sprRend = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        startPos = transform.localPosition;
        
        animator = GetComponent<Animator>();
        speed = 450f;
        parentObject = transform.parent.gameObject;
        animator.enabled = false;
        startAnim = Random.Range(0f, 3f);

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!animator.enabled)
        {
            if (Time.time >= startAnim)
            {
                animator.enabled = true;
            }
        }
        switch (state)
        {
            case "base":
                if (transform.parent.GetComponent<Rigidbody2D>().velocity.x > 0)
                {
                    sprRend.flipX = false;
                }
                else
                {
                    sprRend.flipX = true;
                }
                break;

            case "dive":
                begingFloatTimer -= Time.deltaTime;
                Vector2 pursuitVector = transform.position - attackTarget.transform.Find("bambooPlayer").position;
                if (begingFloatTimer < 0)
                {
                    pursuitVector = -pursuitVector.normalized;
                    rb.velocity = pursuitVector * speed * Time.deltaTime;
                }
                if (Vector2.Distance(transform.position, attackTarget.transform.Find("bambooPlayer").position) < 4f)
                {
                    state = "attack";
                    animator.SetBool("PlayerWithin4", true);
                    animator.SetBool("HitPlayer", true);
                    beginAttackTimer = 0f;
                    chargePoint = transform.position;
                    betweenTimer = 1f;
                }
                break;

            case "attack":
                rb.velocity = new Vector2(0f, 0f);
                if (Vector2.Distance(transform.position, attackTarget.transform.Find("bambooPlayer").position) > 6f)
                {
                    state = "dive";
                    animator.SetBool("PlayerWithin4", false);
                    animator.SetBool("HitPlayer", false);
                    begingFloatTimer = .5f;
                }

                beginAttackTimer += Time.deltaTime;
                if (beginAttackTimer >= .3f)
                {
                    beginAttackTimer = 0f;
                    attackIn = !attackIn;
                    chargePoint = attackTarget.transform.Find("bambooPlayer").position + new Vector3(Random.Range(-3, 3), Random.Range(1.5f, 2f),0);
                }

                if (attackIn)
                {
                    betweenTimer += Time.deltaTime;
                    if (betweenTimer >= .1f)
                    {
                        transform.position = attackTarget.transform.Find("bambooPlayer").position + new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), 0);
                        betweenTimer = 0f;
                        GameObject particleObj = Instantiate(particleEffect);
                        particleObj.transform.position = transform.position;
                    }
                }
                else
                {
                    //set random location 
                    transform.position = chargePoint;
                }

                break;

            case "returnHome":
                if (transform.parent.GetComponent<Rigidbody2D>().velocity.x > 0)
                {
                    sprRend.flipX = false;
                }
                else
                {
                    sprRend.flipX = true;
                }
                Vector2 returnVector = new Vector2(transform.localPosition.x,transform.localPosition.y) - startPos;
                returnVector = -returnVector.normalized;
                transform.localPosition += new Vector3(returnVector.x,returnVector.y,0) * 10f * Time.deltaTime;
                if(Vector2.Distance(transform.localPosition,startPos) < 2f)
                {
                    state = "base";
                }
                break;

        }


    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player" && attackTarget == null)
        {
            state = "dive";
            attackTarget = collision.gameObject;
            animator.SetBool("PlayerWithin10", true);
            transform.parent = null;
            begingFloatTimer = .25f;

        }
        
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            state = "returnHome";
            transform.parent = parentObject.transform;
            animator.SetBool("PlayerWithin10", false);
            attackTarget = null;
        }
    }
}
