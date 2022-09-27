using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    private CharacterController controller;
    private Animator anim;
    
    private Vector3 moveDirection;
    private Vector3 velocity;
    
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

        float moveZ = Input.GetAxisRaw("Vertical");
        float moveX = Input.GetAxisRaw("Horizontal");
        moveDirection = new Vector3(moveX, 0, moveZ);
        moveDirection = transform.TransformDirection(moveDirection);

        if(isGrounded) {
            if(moveDirection != Vector3.zero && !Input.GetKey(KeyCode.LeftShift)) {
                // Walking with animation
                if(Input.GetKey(KeyCode.S)) {
                    WalkBackwards();
                }
                else {
                    Walk();
                }
            }
            else if(moveDirection != Vector3.zero && Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.LeftShift)) {
                // Running with animation
                Run();
            }
            else if(moveDirection == Vector3.zero) {
                // Stand in place with idle animation
                Idle();
            }

            moveDirection *= movementSpeed;

            if(Input.GetKeyDown(KeyCode.Space)) {
                Jump();
            }
        }

        controller.Move(moveDirection * Time.deltaTime);
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    private void WalkBackwards() {
        movementSpeed = walkSpeed;
        anim.SetFloat("Speed", -1, 0.1f, Time.deltaTime);
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
}