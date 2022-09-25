using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomPlayerController : MonoBehaviour
{
    [Header("Standard Player Settings")]
    public float playerHP = 100f;
    float maxHp = 100f;
    public int moveSpeed = 5; // how fast the player moves
    public float lookSpeedX = 1; // left/right mouse sensitivity
    public float lookSpeedY = 1; // up/down mouse sensitivity
    public float jumpForce = 10; // ammount of force applied to create a jump
    Rigidbody _rigidbody;
    public Image healthBar;
    public GameManager gm;

    [Header("Camera")]
    public Transform camTrans; // a reference to the camera transform
    float xRotation;
    float yRotation;

    [Header("Ground & Wall Checking")]
    //The physics layers you want the player to be able to jump off of. Just dont include the layer the palyer is on.
    public LayerMask groundLayer;
    public Transform feetTrans; //Position of where the players feet touch the ground
    float groundCheckDist = .1f; //How far down to check for the ground. The radius of Physics.CheckSphere
    public bool grounded = false; //Is the player on the ground
    public bool onGround = false;
    public bool onWall = false;
    public bool wallJumped = true;
    private GameObject wall;

    [Header("Jumping")]
    public float jumpTime = 0f;
    public float jumpCD = .2f;
    public int numConsecutiveJumps = 0;
    float consecTimer = 0f;
    public float resetConsecutiveTime = 0.2f;
    public float maxVelMod = .2f;

    [Header("Zipping")]
    public float maxRayDistance = 100f;
    public Ray ray1;
    [SerializeField]
    public LineRenderer lineRenderer;
    bool zipping = false;
    Vector3 curDirection;
    Vector3 endPoint;
    public Image crosshair;
    public float minZipSpeed = 50f;
    public float zipCD = 3f;
    public float zipTimeLeft = 0f;

    [Header("Attacks")]
    public Animation sword;
    int swordAnim = 2;
    public GameObject hitbox;
    public float damage = 5f;
    public GameObject playerProjectile;

    [Header("Effects")]
    public float shakeDuration = 0f;
    public Vector3 initialPosition;
    public float shakeMagnitude = 1f;
    public GameObject speedLines;
    public float speedLineSpeed = 50f;
    public float maxCameraFOVIncrease = 2f;
    public float slowMoMaxLength = .5f;
    public float slowMoAmount = .5f;
    public float curTimeflow;
    public GameObject damageFlash;
    public bool paused = false;

    void Start()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        lookSpeedX *= .65f; //WebGL has a bug where the mouse has higher sensitibity. This compensates for the change. 
        lookSpeedY *= .65f; //.65 is a rough guess based on testing in firefox.
#endif
        _rigidbody = GetComponent<Rigidbody>(); // Using GetComponent is expensive. Always do it in start and chache it when you can.
        Cursor.lockState = CursorLockMode.Locked; // Hides the mouse and locks it to the center of the screen.

        Physics.gravity = new Vector3(0, -20f, 0);
        speedLines.SetActive(false);
        curTimeflow = slowMoMaxLength;
        maxHp = playerHP;
        if (gm == null)
        {
            gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        }
    }

    void FixedUpdate()
    {
        damage = 5f + _rigidbody.velocity.magnitude;
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
        moveDir *= moveSpeed + numConsecutiveJumps * maxVelMod;
        Vector3 oldVelocity = _rigidbody.velocity;


        if (!grounded || numConsecutiveJumps > 0)
        {
            // Set the velocity to our movement vector
            moveDir += moveSpeed * (transform.forward * Input.GetAxisRaw("Vertical") + transform.right * Input.GetAxis("Horizontal")); 
        }
        moveDir.x = (Input.GetAxisRaw("Vertical") == 0 && Input.GetAxisRaw("Horizontal") == 0) && grounded? 0 :Mathf.Clamp(moveDir.x, -moveSpeed - numConsecutiveJumps*maxVelMod, moveSpeed + numConsecutiveJumps * maxVelMod);
        moveDir.z = (Input.GetAxisRaw("Vertical") == 0 && Input.GetAxisRaw("Horizontal") == 0) && grounded? 0 : Mathf.Clamp(moveDir.z, -moveSpeed - numConsecutiveJumps * maxVelMod, moveSpeed + numConsecutiveJumps * maxVelMod);

        

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

        if (zipping)
        {
            lineRenderer.enabled = true;
            moveDir = curDirection * (moveSpeed + numConsecutiveJumps * maxVelMod + minZipSpeed);
            if (Vector3.Dot(curDirection, (endPoint - transform.position)) < 0)
            {
                zipping = false;
            }
        }
        else
        {
            lineRenderer.enabled = false;
        }
        float mag = moveDir.magnitude;
        if (mag > speedLineSpeed)
        {
            speedLines.SetActive(true);
            Camera.main.fieldOfView = 60f + maxCameraFOVIncrease;
        }
        else
        {
            speedLines.SetActive(false);
            Camera.main.fieldOfView = 60f;
        }
        
        _rigidbody.velocity = moveDir;

        healthBar.fillAmount = (playerHP/ maxHp);
    }

    void FireRay()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, maxRayDistance))
        {
            Debug.Log(hit.transform.name + "Found!");
            
            lineRenderer.SetPosition(0, ray.origin);
            lineRenderer.SetPosition(1, hit.point);
            endPoint = hit.point;
            zipping = true;
            numConsecutiveJumps += 1;
            curDirection = ray.direction;
            zipTimeLeft = zipCD;
        }
    }

    void SwingSword()
    {
        print(swordAnim);
        switch (swordAnim){
            case 0:
                sword.Play("SwordSwing3");
                swordAnim = 1;
                break;
            case 1:
                sword.Play("SwordSwing2");
                swordAnim = 0;
                break;
            default:
                sword.Play("SwordSwing1");
                break;
        }
        
    }

    public void SpawnProjectile(Vector3 pos)
    {
        GameObject b = Instantiate(playerProjectile, pos, Camera.main.transform.rotation);
    }

    public void TakeDamage(float damage)
    {
        playerHP -= damage;
        shakeDuration = .2f;
        shakeMagnitude = .5f;
        numConsecutiveJumps = 0;
        zipping = false;
        damageFlash.SetActive(true);
        if (playerHP < 0)
        {
            //do something;
            gm.ShowDeathScreen();
        }
    }

    void Update()
    {
        if (!paused)
        {
            //switched the mouse inputs
            yRotation += Input.GetAxis("Mouse X") * lookSpeedX;
            xRotation -= Input.GetAxis("Mouse Y") * lookSpeedY; //inverted
            xRotation = Mathf.Clamp(xRotation, -90, 90); //Keeps up/down head rotation realistic
            camTrans.localEulerAngles = new Vector3(xRotation, 0, 0);
            transform.eulerAngles = new Vector3(0, yRotation, 0);

            if ((grounded || zipping) && Input.GetButtonDown("Jump")) //if the player is on the ground and press Spacebar
            {
                consecTimer = 0f;
                numConsecutiveJumps++;
                jumpTime = Time.time;
                _rigidbody.AddForce(new Vector3(0, jumpForce, 0), ForceMode.VelocityChange); // Add a force jumpForce in the Y direction
                zipping = false;
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
                zipping = false;
            }

            if (Input.GetButtonDown("Fire1"))
            {
                SwingSword();
            }
            if (sword.isPlaying)
            {
                hitbox.SetActive(true);
            } else
            {
                hitbox.SetActive(false);
            }
            if (Input.GetButtonDown("Fire2") && zipTimeLeft <= 0f)
            {
                FireRay();
            }

            ray1 = Camera.main.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
            RaycastHit hit;
            bool zipAvailable = Physics.Raycast(ray1, out hit, maxRayDistance);
            if (zipAvailable && zipTimeLeft <= 0f)
            {
                crosshair.color = Color.red;
            } else if (zipAvailable)
            {
                crosshair.color = Color.gray;
            }
            else
            {
                crosshair.color = Color.black;
            }
            //zipcd
            if(zipTimeLeft > 0)
            {
                zipTimeLeft -= Time.deltaTime;
            }
            //screenshake
            if (shakeDuration > 0)
            {
                camTrans.localPosition = initialPosition + Random.insideUnitSphere * shakeMagnitude;

                shakeDuration -= Time.deltaTime;
            }
            else
            {
                shakeDuration = 0f;
                camTrans.localPosition = initialPosition;
                damageFlash.SetActive(false);
            }
            Time.timeScale = Mathf.Lerp(slowMoAmount, 1f, curTimeflow / slowMoMaxLength);
            if (curTimeflow < slowMoMaxLength)
            {
                curTimeflow += Time.deltaTime;
            }
        } 
        else
        {
            Time.timeScale = 0;
        }
    } 
    private void OnCollisionEnter(Collision collision)
    {
        zipping = false;
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