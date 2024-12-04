using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.PlayerLoop;


public class playerControler : MonoBehaviour
{


    public bool grounded = false;
    public bool forwardHead = false;
    public bool forwardLeg = false;
    public Animator an;
    public Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;
        an = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }
    [SerializeField] float speed = 0.1f;
    [SerializeField] float jump_Force = 10f;
    // Update is called once per frame
    float x;
    void Update()
    {
        if (roundTo1(rb.velocity.x) != 0)
        {

            transform.localScale = new Vector3(Mathf.Sign(roundTo1(rb.velocity.x)) * 1, 1, 1);
        }
        x = Input.GetAxis("Horizontal");
        if (Input.GetKey(KeyCode.LeftShift) && (x == 1 || x == -1) && forwardLeg && !forwardHead)
        {
            forwardLeg = false;
            jump();
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            jump();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    void FixedUpdate()
    {
        an.SetFloat("yVelocity", roundTo1(rb.velocity.y));

        //control the animation --------------------------------------
        if (roundTo1(rb.velocity.x) != 0 && grounded)
        {
            an.SetBool("walk", true);
        }
        else
        {
            an.SetBool("walk", false);
        }
        //Debug.Log(1 / Time.deltaTime);
        rb.AddForce(new Vector3(x * speed * Time.deltaTime, 0, 0));

    }

    float roundTo1(float value)
    {
        return Mathf.Round(value * 10f) / 10f;
    }

   [HideInInspector] public float timeSinceLastGround = 0;
    float timesinceTheLastJump = 0;
    void jump()
    {

        if (Time.time - timesinceTheLastJump > Time.deltaTime * 15)
        {

            if (grounded  || Time.time - timeSinceLastGround < Time.deltaTime * 5)
            {
                an.SetTrigger("jump");
                timesinceTheLastJump = Time.time;
                rb.AddForce(new Vector3(0, jump_Force * Time.fixedDeltaTime, 0));
            }
           
        }



    }

}
