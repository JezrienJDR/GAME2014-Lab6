using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{


    public Joystick joystick;

    public float xDead;
    public float yDead = 1;

    public float walkForce;
    public float jumpForce;

    private Rigidbody2D rb;

    private SpriteRenderer sr;
    private Animator anim;

    public bool isGrounded;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        _Move();
    }

    void _Move()
    {
        if (isGrounded)
        {


            if (joystick.Horizontal > xDead)
            {
                rb.AddForce(Vector2.right * walkForce * Time.deltaTime);

                sr.flipX = false;

                anim.SetInteger("AnimState", 1);
            }
            else if (joystick.Horizontal < -xDead)
            {
                rb.AddForce(Vector2.left * walkForce * Time.deltaTime);
                sr.flipX = true;


                anim.SetInteger("AnimState", 1);
            }
            else
            {

                anim.SetInteger("AnimState", 0);
            }
            if (joystick.Vertical > yDead)
            {
                rb.AddForce(Vector2.up * jumpForce * Time.deltaTime);
                anim.SetInteger("AnimState", 2);
            }

        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        isGrounded = true;
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        isGrounded = false;
    }
}
