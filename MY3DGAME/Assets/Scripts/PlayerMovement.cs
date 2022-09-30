using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    private CharacterController controller;
    private Animator anim;
    
    private Vector3 moveDirection;
    private Vector3 velocity;
    
    [SerializeField]private Transform cameraTransform;
    [SerializeField] private float turnSmoothTime = 0.1f;
    [SerializeField] private float turnSmoothVelocity;
    
    [SerializeField] private float movementSpeed;
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;
    [SerializeField] private float jumpHeight;
   

    [SerializeField] private bool isGrounded;
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float gravity;

    // Start is called before the first frame update
    private void Start() {
        controller = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    private void Update() {
        Move();
    }

    private void Move() {
        isGrounded = Physics.CheckSphere(transform.position, groundCheckDistance, groundMask);

        if(isGrounded && velocity.y < 0) {
            velocity.y = -2f;
        }

        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");
        
        moveDirection = new Vector3(moveX, 0, moveZ).normalized;
        //moveDirection = transform.TransformDirection(moveDirection);

        moveDirection = Quaternion.AngleAxis(cameraTransform.rotation.eulerAngles.y, Vector3.up) * moveDirection;
        //moveDirection.Normalize();

        if(isGrounded) {
            if(moveDirection != Vector3.zero && Input.GetKey(KeyCode.LeftShift)) {
                // Play running animation and call Run() function
                float targetAngle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
                transform.rotation = Quaternion.Euler(0, angle, 0);
                // Vector3 moveDirection = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;
                Run();
            }
            else if(moveDirection != Vector3.zero) {
                // Play walking animation and call Walk() function
                float targetAngle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
                transform.rotation = Quaternion.Euler(0, angle, 0);
                Walk();
            }
            else if(moveDirection == Vector3.zero) {
                // Play idle animation and call Idle() function
                Idle();
            }

            moveDirection *= movementSpeed;

            if(Input.GetKeyDown(KeyCode.Space)) {
                Jump();
            }

            
            if(Input.GetKeyDown(KeyCode.G)) {
                Dodge();
            }
        }

        controller.Move(moveDirection * Time.deltaTime);
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    private void Idle() {
        anim.SetFloat("Speed", 0, 0.1f, Time.deltaTime);
    }

    private void Walk() {
        movementSpeed = walkSpeed;
        anim.SetFloat("Speed", 0.5f, 0.1f, Time.deltaTime);
    }

    private void Run() {
        movementSpeed = runSpeed;
        anim.SetFloat("Speed", 1, 0.1f, Time.deltaTime);
    }

    private void Jump() {
        velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
        anim.SetTrigger("Jump");
    }

    private void Dodge() {
        anim.SetTrigger("Dodge");
    }

    private void OnApplicationFocus(bool focus) {
        if(focus) {
            Cursor.lockState = CursorLockMode.Locked;
        }
        else {
            Cursor.lockState = CursorLockMode.None;
        }
    }
}