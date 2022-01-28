using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5.0f, jumpHeight = 0.5f, gravity, groundCheckRadius, groundDrag, airDrag;
    float turnSmoothVelocity, turnSmoothTime = 0.2f;
    Rigidbody rb;
    public Transform cameraPos, modelDirection, groundCheckOrigin;
    bool isGrounded, isJumping;
    public LayerMask groundMask;
    public Animator anim;

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
        
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cameraPos.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(modelDirection.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            modelDirection.rotation = Quaternion.Euler(0f, angle, 0f);
            
            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            rb.AddForce(moveDir.normalized * moveSpeed * Time.deltaTime);
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
                anim.Play("Harker_Idle");
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
    }
}
