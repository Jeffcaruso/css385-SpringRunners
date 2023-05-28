using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroBehavior : MonoBehaviour
{
    //Most of these Vars are made private b/c it will only add confusion modifying them in unity editor...
    [Header("Horizontal Movement")]
    private float moveSpeed = 300f;     //100
    private float maxSpeed = 200f;      //70

    [Header("Vertical Movement")]
    //ensure the next two items are the same, or you will only use defaultJumpForce value!!!
    private float jumpForce = 150f;             //100
    private float defaultJumpForce = 150f;      //100

    private float SpringShoeMultiplier = 2.0f;
    //public bool springShoesON = false; //MAY END UP BEING USEFUL FOR FUTURE INTEGRATIONS (could be a killswitch for letting shift do a jump...
    //or could adjust spring shoe multiplier to 1... (could have upgraded spring shoes on occasions if needed...

    private float jumpDelay = 0.25f;
    private float runningJumpForce = 50.0f; 


    [Header("Items To Handle in Unity Editor")]
    //do in unity editor to 'floor' layer!
    public LayerMask groundLayer;
    public float groundLength = 1.27f;  //Will need to be customized (useful to change in editor), leaving public for now
    public Vector3 colliderOffset;
    public int Change_Movement_In_Code = 1;  //reminder to change movement vars in the code here :)

    //more partially movement related items
    [Header("Misc Movement")]
    private float linearDrag = 2f;      //4
    private float gravity = 30f;        //10
    private float fallMultiplier = 3f;  //3     //was 5...
    

    [Header("Misc Private Vars")]
    private bool onGround = true;  //false 
    private Rigidbody2D rb;
    private Vector2 direction;
    private float jumpTimer;
    private GameObject finish;



    [Header("Grapple Vars")]
    public float grappleDist = 300f;
    public bool isGrappling = false;
    public GameObject player;
    Rigidbody playerRigidbody;

    public AudioSource jumpSoundEffect;
    public AudioSource hurtSoundEffect;

    private Animator animcontr;

    // Start is called before frame 0
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animcontr = GetComponent<Animator>();
        playerRigidbody = player.GetComponent<Rigidbody>();
        finish = GameObject.Find("Safety FinishLine");
    }

    // Update is called once per frame
    void Update()
    {
        ///Grapple stuff...
        Vector3 pos = transform.position;
        isGrappling = GetComponent<Grappler>().localGrapple;
        if(isGrappling)
        {
            return;
        } else {
        }


        // end grapple stuff...



        //ground detection via Raycasts
        //Debug.Log("On ground is : " + onGround + "\n");
        onGround = Physics2D.Raycast(transform.position + colliderOffset, Vector2.down, groundLength, groundLayer) || 
            Physics2D.Raycast(transform.position - colliderOffset, Vector2.down, groundLength, groundLayer) ||
            Physics2D.Raycast(transform.position + (colliderOffset /2), Vector2.down, groundLength, groundLayer) ||
            Physics2D.Raycast(transform.position - (colliderOffset /2), Vector2.down, groundLength, groundLayer) ||
            Physics2D.Raycast(transform.position, Vector2.down, groundLength, groundLayer);  //center

            if (!onGround) {
                animcontr.SetBool("isAirborne", true);
            } else {
                animcontr.SetBool("isAirborne", false);
            }

        //reset jump force to default
        if (jumpTimer == 0)
        {
            //generally won't need, but reduces shenanigans
            jumpForce = defaultJumpForce;
        }

        while (jumpTimer < Time.time)
        {
            //no jump...
            //spring shoes ultra jump
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                //do spring shoes (mega jump) this round
                //Debug.Log("Testing, LeftShift has been pressed, ultra jump!");

                //prevent overloading of force with many shifts being pressed...
                jumpForce = defaultJumpForce;

                jumpForce *= SpringShoeMultiplier;
                jumpTimer = Time.time + jumpDelay;
                break;
            }
            //normal jump
            else if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space)) //for now, w or space for norm. jump
            {
                //Debug.Log("Testing, W has been pressed, normal jump!");

                jumpForce = defaultJumpForce; //this also just might make more sense than the if on 50-53
                jumpTimer = Time.time + jumpDelay;
                break;
            }            
            //no vertical inputs
            break;
        }

        ///very important line directly below!!!
        direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));   
        
        if (transform.position.x > finish.transform.position.x + 50){
            rb.velocity = new Vector2(0,0);
            MenuSelections.win = true;
        }




    }


    private void FixedUpdate()
    {
        Move(direction.x);

        if(jumpTimer > Time.time && onGround)
        {
            Jump();
            jumpSoundEffect.Play();  //crashes game...?
        }

        ModifyPhysics();
    }

    private void Move(float horizontal)
    {
        //this method is important, especially for functional horizontal movement!
        //Debug.Log("actually using Move method!");
        if(horizontal == 0) {
            animcontr.SetBool("isRunning", false);
        } else {
            animcontr.SetBool("isRunning", true);
        }

        //horizontal movement
        rb.AddForce(Vector2.right * horizontal * moveSpeed);

        //if at top speed
        if (Mathf.Abs(rb.velocity.x) >= maxSpeed)
        {
            //velocity acknowledged(existing Y, the velocity of x..., get a true total velocity (diagonal distance...)
            rb.velocity = new Vector2(Mathf.Sign(rb.velocity.x) * maxSpeed, rb.velocity.y);
            if (onGround)
            {
                //optional coloring
                //gameObject.GetComponent<SpriteRenderer>().color = Color.yellow;
            }
            else
            {
                //optional coloring
                //gameObject.GetComponent<SpriteRenderer>().color = Color.red;
            }
        }
        else
        {
            //optional coloring
            //gameObject.GetComponent<SpriteRenderer>().color = Color.red;
        }
    }



    //effecitvely, this is to do two things
    // 1) Managing drag (make high/full amount) and gravity (can be low - not really relevant) ON GROUND
    // 2) Slow Deceleration when jumping up (reduced gravity while still moving upwards), still is quite high gravity compared to earth... IN AIR
    private void ModifyPhysics()
    {
        isGrappling = GetComponent<Grappler>().localGrapple;
        if (isGrappling)
        {
            return;
        }
        bool changingDirections = (direction.x > 0 && rb.velocity.x < 0) || (direction.x < 0 && rb.velocity.x > 0);

        if (onGround) {
            if (Mathf.Abs(direction.x) < 0.4f || changingDirections)
            {
                rb.drag = linearDrag;
            } else
            {
                rb.drag = 0f;
            }
            rb.gravityScale = 0;
        }
        else
        {
            rb.gravityScale = gravity;
            rb.drag = linearDrag * 0.15f;
            if(rb.velocity.y < 0)
            {
                //if already falling, fall at normal rate...
                rb.gravityScale = gravity * fallMultiplier;
            } 
            else if (rb.velocity.y > 0 && !(Input.GetKey(KeyCode.W)  || Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.Space)))  //Note  the W or <shift> or <space> making the 'reduced gravity'
            {
                rb.gravityScale = gravity * (fallMultiplier / 2);
            }
        }
    }


    //Jumping method
    //Can see that the spring shoes are creating the higher jump both from results on screen and the debug with exact jump force.
    private void Jump()
    {
        float runJump = (Mathf.Abs(rb.velocity.x) / maxSpeed) * runningJumpForce;
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.AddForce(Vector2.up * (jumpForce + runJump), ForceMode2D.Impulse);
       
        //Debug.Log("Testing jump values: " + (Vector2.up * (jumpForce + runJump)));

        //reset, so we won't keep doing jumps.
        jumpTimer = 0;
    }


    //optional visualization of ground contact detection
    //however, doesn't appear when actually running (only in editor preview, and is very helpful, so leave it on)
        // You can see if you have misconfigured your ground detection boundaries with these!
    //Set the offsets in Unity editor!
    private void OnDrawGizmos()
    {
        //set color
        Gizmos.color = Color.red;

        //Left and right (0%, 100%)
        Gizmos.DrawLine(transform.position + colliderOffset, transform.position + colliderOffset + Vector3.down * groundLength);
        Gizmos.DrawLine(transform.position - colliderOffset, transform.position - colliderOffset + Vector3.down * groundLength);

        //midpoint (25%, 75%)
        Gizmos.DrawLine(transform.position + (colliderOffset / 2), transform.position + (colliderOffset / 2) + Vector3.down * groundLength);
        Gizmos.DrawLine(transform.position - (colliderOffset / 2), transform.position - (colliderOffset / 2) + Vector3.down * groundLength);

        //true midpoint (50%)
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * groundLength);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.CompareTag("Piston"))
       {
        hurtSoundEffect.Play();
       }
       if(col.CompareTag("Void"))
       {
            hurtSoundEffect.Play();
            GameObject respawnPoint = col.gameObject.transform.parent.GetChild(0).gameObject;  //RespawnPoint Object must be in slot 0
            transform.position = new Vector3(respawnPoint.transform.position.x,
                                             respawnPoint.transform.position.y,
                                             transform.position.z);
       }
       if (col.CompareTag("BossTrigger")){
            gameObject.transform.GetChild(0).gameObject.SetActive(true);
            MenuSelections.checkpoint = true;
       }

       if (col.CompareTag("Bullet")){
            rb.velocity = new Vector2(0,0);
            hurtSoundEffect.Play();
       }

       if (col.CompareTag("Speedup")){
            GameObject.Find("Piston").GetComponent<PistonMovement>().pistonSpeed = 160f;
       }
    }

}





/*
Links to see to further facilitate this code
See ReadMe at: https://github.com/Jeffcaruso/css385-finalProject-jump-demo
Links:
Raycast: https://docs.unity3d.com/ScriptReference/Physics2D.Raycast.html 
Link info: https://github.com/t4guw/100-Unity-Mechanics-for-Programmers/tree/master/programs/super_mario_style_jump 
*/






/*
 * Currently Unused code... (Temporary storage, delete later)
 * - Realistically, becuase this is a demo, leave this here to explore as needed.
 * - No need to carry this part (comment section here) over to the final project...
 * 
 * 
 * Input code
 * 
        // if (Input.GetKeyDown(KeyCode.W))
        // {
        //     //need to check for touching the ground to be able to jump
        //     //need to check for contact with a 'Floor' tag!
        //     Debug.Log("TEST! with W");
        //     rb.AddForce(new Vector3(0f, 10f, 0f), ForceMode2D.Impulse);
        // }
        // if (Input.GetKey(KeyCode.A))
        // {
        //     Debug.Log("TEST! with A");
        //     rb.AddForce(new Vector3(-.1f, 0f, 0f), ForceMode2D.Impulse);
        // }
        // if (Input.GetKeyDown(KeyCode.S))
        // {
        //     Debug.Log("TEST! with S");
        //     rb.AddForce(new Vector3(0f, -5f, 0f), ForceMode2D.Impulse);
        // }
        // if (Input.GetKey(KeyCode.D))
        // {
        //     Debug.Log("TEST! with D");
        //     rb.AddForce(new Vector3(.1f, 0f, 0f), ForceMode2D.Impulse);
        // }
        // if(Input.GetKeyDown(KeyCode.Space))
        // {
        //     //need to check for contact with a 'Floor' tag!
        //     Debug.Log("TEST! with Space - Spring shoes!");
        //     rb.AddForce(new Vector3(0f, 21f, 0f), ForceMode2D.Impulse);
        // } 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 */ 