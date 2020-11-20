using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RampDirection
{
    UP,
    DOWN,
    NONE
}


public class PossumBehaviour : MonoBehaviour
{

    public float runForce;
    public float upForce;
    public Transform lookAheadPoint;
    public Transform lookFrontPoint;
    public Transform lookBackPoint;

    public Rigidbody2D rb;


    public LayerMask collisionGroundLayer;
    public LayerMask collisionWallLayer;
    public bool isGroundAhead;

    public bool onRamp;
    public RampDirection rampDir;


    public ContactFilter2D filter;

    // Start is called before the first frame update
    void Start()
    {
        rampDir = RampDirection.NONE;

        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        HasLOS();
        _LookInFront();
        _LookAhead();
        _Move();
    }

    private void HasLOS()
    {
        var size = new Vector2(13.0f, 2.0f);



        RaycastHit2D[] arrayOfHits = { };

        var hit = Physics2D.BoxCast(transform.position, size, 0.0f, Vector2.left * transform.localScale.x, filter, arrayOfHits);

        foreach(var h in arrayOfHits)
        {
            Debug.Log(h.transform.gameObject.name);
        }

    }

    private void _Move()
    {
        if (isGroundAhead)
        {
            rb.AddForce(Vector2.left * runForce * Time.deltaTime * transform.localScale.x);


            if (onRamp)
            {
                if(rampDir == RampDirection.UP)
                {
                    rb.AddForce(Vector2.up * upForce * Time.deltaTime);
                }
                else if(rampDir == RampDirection.DOWN)
                {
                    rb.AddForce(Vector2.down * upForce * Time.deltaTime);
                }

                //transform.rotation = Quaternion.Euler(0.0f, 0.0f, -26.5f);
                StartCoroutine(Rotate());
               
            }
            else
            {
                //transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
                StartCoroutine(Normalize());
            }

            rb.velocity *= 0.90f;

        }
        else
        {
            FlipX();
        }
    }

    private void _LookInFront()
    {
        var wallHit = Physics2D.Linecast(transform.position, lookFrontPoint.position, collisionWallLayer);
        if (wallHit)
        {
            if (!wallHit.collider.CompareTag("Ramps"))
            {
                FlipX();
                
            }
           
        }
        Debug.DrawLine(transform.position, lookFrontPoint.position, Color.green);


        if (lookFrontPoint.position.y > lookBackPoint.position.y)
        {
            rampDir = RampDirection.UP;
        }
        else if (lookFrontPoint.position.y < lookBackPoint.position.y)
        {
            rampDir = RampDirection.DOWN;
        }
        else
        {
            rampDir = RampDirection.NONE;
        }
    }
    
    private void FlipX()
    {
        transform.localScale = new Vector3(transform.localScale.x * -1.0f, transform.localScale.y, transform.localScale.z);
    }

    private void _LookAhead()
    {
        var groundHit = Physics2D.Linecast(transform.position, lookAheadPoint.position, collisionGroundLayer);
        if (groundHit)
        {
            if (groundHit.collider.CompareTag("Ramps"))
            {
                onRamp = true;

            }
            if (groundHit.collider.CompareTag("Platforms"))
            {
                onRamp = false;
            }
            isGroundAhead = true;
        }
        else
        {
            isGroundAhead = false;
        }

        Debug.DrawLine(transform.position, lookAheadPoint.position, Color.red);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.CompareTag("Platforms"))
        {
            isGroundAhead = true;
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Platforms"))
        {
            isGroundAhead = false;
        }
    }

    IEnumerator Rotate()
    {
        yield return new WaitForSeconds(0.05f);
        transform.rotation = Quaternion.Euler(0.0f, 0.0f, -26.5f);
    }

    IEnumerator Normalize()
    {
        yield return new WaitForSeconds(0.05f);
        transform.rotation = Quaternion.Euler(0.0f, 0.0f, -0.0f);
    }
}
