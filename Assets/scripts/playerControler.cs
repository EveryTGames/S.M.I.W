
using UnityEngine;

using static events;


public class playerControler : MonoBehaviour
{

    public static playerControler pc; //not needed currently 
    [SerializeField] bool grounded = false;
    [SerializeField] bool forwardHead = false;
    [SerializeField] bool forwardLeg = false;
     Animator an;
     Rigidbody2D rb;


    private void Awake()
    {
        if (pc == null)
        {
            pc = this;
        }
        else
        {
            Debug.LogWarning("multiple player controller ditected");
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;
        an = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();


        //subscribe to events
        onGroundCheck += OnGroundCheck;
        onForwardLegCheck += OnForwardLegCheck;
        onForwardHeadCheck += OnForwardHeadCheck;

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

    [HideInInspector] float timeSinceLastGround = 0;
    float timesinceTheLastJump = 0;
    void jump()
    {

        if (Time.time - timesinceTheLastJump > Time.deltaTime * 15)
        {

            if (grounded || Time.time - timeSinceLastGround < Time.deltaTime * 5)
            {
                an.SetTrigger("jump");
                timesinceTheLastJump = Time.time;
                rb.AddForce(new Vector3(0, jump_Force * Time.fixedDeltaTime, 0));
            }

        }



    }

    void OnGroundCheck(bool state)
    {
        grounded = state;
        if (state)
        {


            if (!an.GetBool("Grounded_bool"))
            {
                //first time to be true(enter)
                rb.velocity = new Vector3(0, 0, 0);
                an.SetBool("Grounded_bool", true);
                an.SetTrigger("grounded");
                an.ResetTrigger("jump");
            }
            else
            {
                //it was true before(stay)
               an.SetBool("Grounded_bool", true);
            }
        }
        else
        {
            timeSinceLastGround = Time.time;
            an.SetBool("Grounded_bool", false);
        }

    }
    void OnForwardLegCheck(bool state)
    {
        forwardLeg = state;
    }
    void OnForwardHeadCheck(bool state)
    {
        forwardHead = state;
    }

}
