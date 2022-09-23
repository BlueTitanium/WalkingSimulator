using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomPlayerController : MonoBehaviour
{

    public int moveSpeed = 5; // how fast the player moves
    public float lookSpeedX = 6; // left/right mouse sensitivity
    public float lookSpeedY = 3; // up/down mouse sensitivity
    public float jumpForce = 10; // ammount of force applied to create a jump

    public Transform camTrans; // a reference to the camera transform
    float xRotation;
    float yRotation;
    Rigidbody _rigidbody;

    //The physics layers you want the player to be able to jump off of. Just dont include the layer the palyer is on.
    public LayerMask groundLayer;

    public Transform feetTrans; //Position of where the players feet touch the ground
    float groundCheckDist = .1f; //How far down to check for the ground. The radius of Physics.CheckSphere
    public bool grounded = false; //Is the player on the ground

    public bool onGround = false;
    public bool onWall = false;
    public bool wallJumped = true;

    private GameObject wall;

    public float jumpTime = 0f;
    public float jumpCD = .2f;

    public int numConsecutiveJumps = 0;
    float consecTimer = 0f;
    public float resetConsecutiveTime = 0.2f;
    public float maxVelMod = .2f;

    void Start()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        lookSpeedX *= .65f; //WebGL has a bug where the mouse has higher sensitibity. This compensates for the change. 
        lookSpeedY *= .65f; //.65 is a rough guess based on testing in firefox.
#endif
        _rigidbody = GetComponent<Rigidbody>(); // Using GetComponent is expensive. Always do it in start and chache it when you can.
        Cursor.lockState = CursorLockMode.Locked; // Hides the mouse and locks it to the center of the screen.

        Physics.gravity = new Vector3(0, -20f, 0);
    }

    void FixedUpdate()
    {

        //The sphere check draws a sphere like a ray cast and returns true if any collider is withing its radius.
        //grounded is set to true if a sphere at feetTrans.position with a radius of groundCheckDist detects any objects on groundLayer within it
        grounded = Physics.CheckSphere(feetTrans.position, groundCheckDist, groundLayer);
        if (grounded)
        {
            consecTimer += Time.deltaTime;
            if(resetConsecutiveTime < consecTimer)
            {
                numConsecutiveJumps = 0;
            }
        }
        //Creates a movement vector local to the direction the player is facing.
        Vector3 moveDir = transform.forward * Input.GetAxisRaw("Vertical") + transform.right * Input.GetAxis("Horizontal"); // Use GetAxisRaw for snappier but non-analogue  movement
        moveDir *= moveSpeed;
        if (onWall)
        {
            wallJumped = false;
            moveDir.y = 0;
        } 
        else if (!onWall && !wallJumped)
        {
            _rigidbody.AddForce(new Vector3(0, jumpForce * 0.5f, 0), ForceMode.VelocityChange);
            wallJumped = true;
            consecTimer = 0f;
            numConsecutiveJumps++;
            jumpTime = Time.time;
        }
        else
        {
            moveDir.y = _rigidbody.velocity.y; // We dont want y so we replace y with that the _rigidbody.velocity.y already is.
        }
        
        
        if (!grounded || numConsecutiveJumps > 0)
        {
            moveDir.x += _rigidbody.velocity.x;
            moveDir.z += _rigidbody.velocity.z;
            
             // Set the velocity to our movement vector
        }
        moveDir.x = Mathf.Clamp(moveDir.x, -moveSpeed - numConsecutiveJumps*maxVelMod, moveSpeed + numConsecutiveJumps * maxVelMod);
        moveDir.z = Mathf.Clamp(moveDir.z, -moveSpeed - numConsecutiveJumps * maxVelMod, moveSpeed + numConsecutiveJumps * maxVelMod);
        _rigidbody.velocity = moveDir;

    }

    void Update()
    {
        //switched the mouse inputs
        yRotation += Input.GetAxis("Mouse X") * lookSpeedX;
        xRotation -= Input.GetAxis("Mouse Y") * lookSpeedY; //inverted
        xRotation = Mathf.Clamp(xRotation, -90, 90); //Keeps up/down head rotation realistic
        camTrans.localEulerAngles = new Vector3(xRotation, 0, 0);
        transform.eulerAngles = new Vector3(0, yRotation, 0);
        /*
        if (grounded && Input.GetButton("Jump") && Time.time > jumpTime + jumpCD) //if the player is on the ground and press Spacebar
        {
            consecTimer = 0f;
            numConsecutiveJumps++;
            jumpTime = Time.time;
            _rigidbody.AddForce(new Vector3(0, jumpForce, 0), ForceMode.VelocityChange); // Add a force jumpForce in the Y direction
        }
        if(onWall && Input.GetButton("Jump") && Time.time > jumpTime + jumpCD)
        {
            consecTimer = 0f;
            numConsecutiveJumps++;
            jumpTime = Time.time;
            onWall = false;
            wallJumped = true;
            Vector3 direction = wall.transform.up;
            direction.Normalize();
            _rigidbody.AddForce(new Vector3(direction.x * jumpForce, jumpForce*1.5f, direction.z * jumpForce), ForceMode.VelocityChange);
        }
        */
        if (grounded && Input.GetButtonDown("Jump")) //if the player is on the ground and press Spacebar
        {
            consecTimer = 0f;
            numConsecutiveJumps++;
            jumpTime = Time.time;
            _rigidbody.AddForce(new Vector3(0, jumpForce, 0), ForceMode.VelocityChange); // Add a force jumpForce in the Y direction
        }
        if (onWall && Input.GetButtonDown("Jump"))
        {
            consecTimer = 0f;
            numConsecutiveJumps++;
            jumpTime = Time.time;
            onWall = false;
            wallJumped = true;
            Vector3 direction = wall.transform.up;
            direction.Normalize();
            _rigidbody.AddForce(new Vector3(direction.x * jumpForce, jumpForce * 1.5f, direction.z * jumpForce), ForceMode.VelocityChange);
        }

    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            onWall = true;
            wall = collision.gameObject;
        } 
        else
        {
            onWall = false;
            wall = null;
        }
        if (collision.gameObject.CompareTag("Ground"))
        {
            onGround = true;
        }
        else
        {
            onGround = false;
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            onWall = false;
            wall = null;
        }
    }

}