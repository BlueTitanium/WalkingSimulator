using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BOSS2 : MonoBehaviour
{
    public enum BossState { isStunned, isThrusting, isSpinning, isIdling }
    public enum ThrustStates { startUp, moving, ending}
    public enum SpinningStates { startUp, moving, ending }

    public float maxHP = 100;
    public float currentHP = 100f;
    public Image HPUI;
    public CustomPlayerController target;
    public LineRenderer lineRenderer;
    public Rigidbody rb;
    public BossState state = BossState.isIdling;
    public ThrustStates thrustState = ThrustStates.startUp;
    public SpinningStates spinState = SpinningStates.startUp;
    float distFromPlayer;
    public float speed = 30f;
    public float highSpeed = 100f;
    public float ThrustDistFromPlayer;
    public float spinDistFromPlayer;
    public float spinSpeed = 20f;
    public Vector3 MoveDir;
    public GameObject bossBody;
    public GameObject SwordFocus;
    public GameObject SwordHitbox, FireStrike;
    public ParticleSystem StunParticles;
    public ParticleSystem teleportingFX;
    public Vector3 RegPosition, RegRotation, ThrustRotation;
    public Vector3 nextPos;
    public float[] thrustCDs;
    public Vector3 thrustPosEnd;
    public float[] spinCDs;
    public Vector3[] spinPositions;
    public float stunTime;
    public float currentTimer = 0f;

    public float currentY = 0f;


    public AudioClip spinning, teleport, thrust, stunned;
    public AudioSource aud;

    public GameObject explosionparticles;
    /* 
     * Phase 2: Spawn the Mad Scientist(guy has a sword)
     *  - Refill player health
     *  - Mad Scientist can dash to the player(fixed speed (fairly fast)) if the player is far enough away and the scientist will do a thrust attack
     *  - if the player is in range, the mad scientist will do a circular swing in the direction of the player.
     *  - If the player attacks during his attack at the right timing, the parry spot will appear and the blade can be parried and the scientist will be stunned.
     *  - The parry will also heal the player(maybe).
     *  
     */

    // Start is called before the first frame update
    void Start()
    {
        currentHP = maxHP;
        state = BossState.isIdling;
    }
    public void TakeDamage(float damage)
    {
        print("hello");
        currentHP -= damage;
        print(currentHP);
        if (currentHP < 0)
        {
            StartCoroutine(Die());
            //Instantiate(particles, this.transform.position, particles.transform.rotation);
        }
    }

    IEnumerator Die()
    {
        Instantiate(explosionparticles, this.transform.position, explosionparticles.transform.rotation, null);
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        HPUI.fillAmount = currentHP / maxHP;
        if(currentTimer >= 0)
        {
            currentTimer -= Time.deltaTime;
        }
        switch (state)
        {
            case BossState.isIdling:
                bossBody.transform.localEulerAngles = new Vector3(0, 0, 0);
                MoveDir = (target.transform.position - this.transform.position);
                distFromPlayer = MoveDir.magnitude;
                MoveDir = MoveDir.normalized;
                transform.localRotation = Quaternion.LookRotation(new Vector3(MoveDir.x, 0, MoveDir.z));
                transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y - 90, 0);
                //SwordFocus.transform.localPosition = RegPosition;
                if (distFromPlayer < ThrustDistFromPlayer && distFromPlayer > spinDistFromPlayer)
                {
                    rb.velocity = MoveDir * speed;
                } else if (distFromPlayer > ThrustDistFromPlayer)
                {
                    rb.velocity = Vector3.zero;
                    currentTimer = thrustCDs[0];
                    thrustState = ThrustStates.startUp;
                    state = BossState.isThrusting;
                } else if (distFromPlayer < spinDistFromPlayer)
                {
                    rb.velocity = Vector3.zero;
                    currentTimer = spinCDs[0];
                    spinState = SpinningStates.startUp;
                    state = BossState.isSpinning;
                    aud.PlayOneShot(spinning);
                }
                if(transform.localPosition.y < 0)
                {
                    transform.localPosition = new Vector3(transform.localPosition.x, 0, transform.localPosition.z);
                }
                break;
            case BossState.isSpinning:
                
                switch (spinState)
                {
                    case SpinningStates.startUp:
                        rb.velocity = Vector3.zero;
                        SwordFocus.transform.localEulerAngles = ThrustRotation;
                        currentY = bossBody.transform.eulerAngles.y;
                        bossBody.transform.localPosition = Vector3.Lerp(spinPositions[1],spinPositions[0],currentTimer/spinCDs[0]);
                        if(currentTimer <= 0)
                        {
                            currentTimer = spinCDs[1];
                            spinState = SpinningStates.moving;
                            teleportingFX.Play();
                            aud.PlayOneShot(teleport);
                            //bossBody.transform.localPosition = new Vector3(bossBody.transform.localPosition.x, bossBody.transform.localPosition.y - 4f, bossBody.transform.localPosition.z);
                        }
                        break;
                    case SpinningStates.moving:
                        SwordHitbox.SetActive(true);
                        MoveDir = (target.transform.position - this.transform.position);
                        distFromPlayer = MoveDir.magnitude;
                        MoveDir = MoveDir.normalized;
                        rb.velocity = MoveDir * speed;
                        
                        currentY += spinSpeed;
                        transform.localRotation = Quaternion.LookRotation(new Vector3(MoveDir.x, 0, MoveDir.z));
                        transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y - 90, 0);
                        bossBody.transform.eulerAngles = new Vector3(0, currentY, 0);

                        if (distFromPlayer < 1f)
                        {
                            rb.velocity = Vector3.zero;
                            
                        }

                        if (currentTimer <= 0)
                        {
                            currentTimer = spinCDs[2];
                            spinState = SpinningStates.ending;
                        }
                        break;
                    case SpinningStates.ending:
                        aud.Stop();
                        SwordHitbox.SetActive(false);
                        rb.velocity = Vector3.zero;
                        SwordFocus.transform.localEulerAngles = RegRotation;
                        bossBody.transform.localEulerAngles = new Vector3(0, 0, 0);
                        bossBody.transform.localPosition = Vector3.Lerp(spinPositions[0], spinPositions[1], currentTimer / spinCDs[2]);
                        if (currentTimer <= 0)
                        {
                            state = BossState.isIdling;
                            teleportingFX.Play();
                            aud.PlayOneShot(teleport);
                        }
                        break;
                }
                break;
            case BossState.isThrusting:
                //do this
                switch (thrustState)
                {
                    case ThrustStates.startUp:
                        rb.velocity = Vector3.zero;
                        SwordFocus.transform.localEulerAngles = ThrustRotation;
                        MoveDir = (target.transform.position - this.transform.position);
                        transform.LookAt(target.transform);
                        bossBody.transform.localEulerAngles = new Vector3(0, -75, 0);
                        //lineRenderer.enabled = true;
                        lineRenderer.SetPosition(0, bossBody.transform.position);
                        lineRenderer.SetPosition(1, target.transform.position);
                        thrustPosEnd = target.transform.position;
                        bossBody.transform.localPosition = Vector3.Lerp(spinPositions[1], spinPositions[0], currentTimer / thrustCDs[0]);
                        if (currentTimer <= 0)
                        {
                            aud.PlayOneShot(thrust);
                            FireStrike.SetActive(true);
                            MoveDir = (target.transform.position - this.transform.position);
                            rb.velocity = MoveDir.normalized * highSpeed;
                            currentTimer = thrustCDs[1];
                            thrustState = ThrustStates.moving;
                            lineRenderer.enabled = false;
                            //teleportingFX.Play();
                            //bossBody.transform.localPosition = new Vector3(bossBody.transform.localPosition.x, bossBody.transform.localPosition.y - 4f, bossBody.transform.localPosition.z);
                        }
                        break;
                    case ThrustStates.moving:
                        
                        //if (Vector3.Dot(MoveDir, (thrustPosEnd - transform.position)) < 0)
                        //{
                        //    rb.velocity = Vector3.zero;
                        //}
                        if(currentTimer <= 0)
                        {
                            currentTimer = thrustCDs[2];
                            FireStrike.SetActive(false);
                            SwordHitbox.SetActive(false);
                            thrustState = ThrustStates.ending;
                            aud.Stop();
                            //teleportingFX.Play();
                        }
                        break;
                    case ThrustStates.ending:
                        
                        rb.velocity = Vector3.zero;
                        SwordFocus.transform.localEulerAngles = RegRotation;
                        //bossBody.transform.localEulerAngles = new Vector3(0, 0, 0);
                        //transform.localRotation = Quaternion.LookRotation(new Vector3(MoveDir.x, 0, MoveDir.z));
                        //transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y - 90, 0);
                        bossBody.transform.localPosition = Vector3.Lerp(spinPositions[0], spinPositions[1], currentTimer / thrustCDs[2]);
                        if (currentTimer <= 0)
                        {
                            state = BossState.isIdling;
                            teleportingFX.Play();
                            aud.PlayOneShot(teleport);
                        }
                        break;
                }
                break;
            case BossState.isStunned:
                if (!StunParticles.isEmitting)
                {
                    aud.Stop();
                    aud.PlayOneShot(stunned);
                    StunParticles.Play();
                    target.Heal(10f);
                }
                SwordHitbox.SetActive(false);
                FireStrike.SetActive(false);
                bossBody.transform.localPosition = Vector3.Lerp(spinPositions[0], bossBody.transform.localPosition, currentTimer / spinCDs[2]);
                rb.velocity = Vector3.zero;
                if (currentTimer <= 0)
                {
                    state = BossState.isIdling;
                    StunParticles.Stop();
                    teleportingFX.Play();
                    aud.Stop();
                    aud.PlayOneShot(teleport);
                }
                //do this
                break;
            default:
                break;
        }
    }
}
