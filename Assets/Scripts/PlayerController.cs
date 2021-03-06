using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5.0f, jumpHeight = 0.5f, gravity, groundCheckRadius, groundDrag, airDrag, attackMoveSpeed;
    float turnSmoothVelocity, turnSmoothTime = 0.2f;
    Rigidbody rb;
    public Transform cameraPos, modelDirection, groundCheckOrigin, keyPosition;
    bool isGrounded, isJumping, isAttacking;
    public LayerMask groundMask;
    public Animator anim;

    GameObject triggerCollider;
    float healthStopWatch;
    public float attackDelay, attackKnockback;

    bool mouse0_down;

    void OnDrawGizmos()
    {
        Gizmos.color = isGrounded ? Color.green : Color.red;
        Gizmos.DrawWireSphere(groundCheckOrigin.position, groundCheckRadius);
    }
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        float vertical = Input.GetAxisRaw("Vertical");
        float horizontal = Input.GetAxisRaw("Horizontal");
        float jump = Input.GetAxisRaw("Jump");
        float mouse0 = Input.GetAxisRaw("Fire1");
        float mouse1 = Input.GetAxisRaw("Fire2");

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            moveSpeed *= 2;
            anim.speed = 1.5f;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            moveSpeed /= 2;
            anim.speed = 1;
        }
        
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cameraPos.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(modelDirection.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            modelDirection.rotation = Quaternion.Euler(0f, angle, 0f);
            
            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            rb.AddForce(moveDir.normalized * moveSpeed * Time.deltaTime);
            
        }
        
        if (mouse0 > 0 && !mouse0_down)
        {
            mouse0_down = true;
        }
        else if (mouse0 == 0)
        {
            //anim.SetBool("attack2", false);
            mouse0_down = false;
        }

        if (mouse0 > 0)
        {
            
            if (isAttacking)
            {
                healthStopWatch += Time.deltaTime;
                if (healthStopWatch >= attackDelay && triggerCollider != null && !anim.GetCurrentAnimatorStateInfo(0).IsName("attack1") && !anim.GetCurrentAnimatorStateInfo(0).IsName("attack2"))
                {
                    Health h;
                    if (triggerCollider.TryGetComponent<Health>(out h))
                    h.health--;
                    
                    healthStopWatch = 0;
                }
                
            }
        }
        

        isGrounded = Physics.CheckSphere(groundCheckOrigin.position, groundCheckRadius, groundMask);

        if (jump > 0 && isGrounded)
        {
            isJumping = true;
            rb.drag = groundDrag;
            rb.AddForce(Vector3.up * jumpHeight * Time.deltaTime, ForceMode.Impulse);

            anim.Play("Harker_Jump");
        }
        else if (jump <= 0 && isGrounded)
        {
            isJumping = false;
            
            if (direction.magnitude < 0.1f && !isJumping)
            {
                if (mouse0 > 0 && mouse0_down)
                {
                    anim.SetBool(Random.Range(0,2) == 1 ? "attack1" : "attack2", true);
                }
                
                if (!anim.GetBool("attack1") && !anim.GetBool("attack2"))
                {
                    anim.Play("Harker_Idle");
                }
            }
            else if (direction.magnitude >= 0.1f && !isJumping)
            {
                anim.Play("Harker_Run");
                
            }
        }
        else
        {
            rb.drag = airDrag;
            
        }


        rb.AddForce(-Vector3.up * gravity * Time.deltaTime);
        //anim.SetBool(Random.Range(0,2) == 1 ? "attack1" : "attack2", false);
        Debug.Log("direction magnitude: " + direction.magnitude);
        
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Harker_Run"))
        {
            anim.SetBool("attack1", false);
            anim.SetBool("attack2", false);
            
        }
    }

    void OnTriggerEnter(Collider other)
    {
        triggerCollider = other.gameObject;
        
        if (other.gameObject.tag == "Enemy")
        isAttacking = true;
        else if (other.gameObject.tag == "Key")
        {
            other.transform.position = keyPosition.position;
            other.transform.parent = keyPosition;

        }
    }

    void OnTriggerExit(Collider other)
    {
        triggerCollider = null;
        
        if (other.gameObject.tag == "Enemy")
        isAttacking = false;
    }
}
