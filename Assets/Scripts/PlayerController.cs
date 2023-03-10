using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour {

    public enum movementState
    {
        Idle,
        ForwardWalk,
        ForwardRun,
        BackwardWalk,
        LeftStrafe,
        RightStrafe
    }

    public float turnSpeed;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float walkSpeed;
    [SerializeField] private float strafeSpeed;
    [SerializeField] private float runSpeed;
    [SerializeField] private float jumpDistance;
    private Vector3 moveDirection;
    private Vector3 velocity;
    [SerializeField] private bool isGrounded;
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float gravity;
    [SerializeField] private float jumpHeight;
    private float attackCooldown;
    private float jumpCooldown;
    private WeaponController weapon;
    
    private Animator anim;
    private CharacterController controller;

    private bool canJump = true;
    private Transform ground;
    private Vector3 lastGroundPosition;

    public bool canMove; // used to prevent the character from moving during an animation
    public bool isAttacking;
    public bool isJumping;
    public bool isBlocking;
    private movementState direction;

    public InputAction playerControls;
    private Vector2 moveInput;
    [SerializeField] InputAction input;

    void Awake() {
        anim = GetComponentInChildren<Animator>();
        controller = GetComponent<CharacterController>();
    }

    void Start() {
        turnSpeed = 90f;
        canMove = true;
        isAttacking = false;
        isJumping = false;
        isGrounded = false;
        isBlocking = false;
        direction = movementState.Idle;
        input = new InputAction();
    }

    void FixedUpdate() {
        
    }

	void Update () {   
        if (isAttacking == false) {
            canMove = true;
        }

        isGrounded = controller.isGrounded;

        if(isGrounded && velocity.y < 0) {
            ResetJumpAndFall();
        }

        Move();

        if (isJumping) {
            anim.SetBool("jump", isJumping);
        }

        if (!isGrounded && !isJumping) {
            anim.SetBool("fall", true);
        }
	}

    public void OnMove(InputAction.CallbackContext context) {
        moveInput = context.ReadValue<Vector2>();
    }

    private void Move() {

        float moveY;
        float moveX;
        if (!Input.GetKey(KeyCode.LeftShift)) {
            moveY = Mathf.Clamp(moveInput.y, -1, 0.5f);
            direction = movementState.ForwardWalk;
        }
        else {
            moveY = moveInput.y;
            direction = movementState.ForwardRun;
        }
        moveX = moveInput.x;
        
        moveDirection = new Vector3(moveX, 0, moveY);
        moveDirection = transform.TransformDirection(moveDirection);

        transform.position += moveDirection * moveY * Time.deltaTime;
        
        // Walking backwards
        if (moveY < 0.0) {
            direction = movementState.BackwardWalk; 
        }

        if (moveY == 0f && moveX == 0f) {
            direction = movementState.Idle;
        }

        if ((moveX > 0)) {
            direction = movementState.LeftStrafe;
        }
        else if ((moveX < 0)) {
            direction = movementState.RightStrafe;
        }

        if(isGrounded && canMove) {
            switch(direction){

                case movementState.ForwardWalk:
                    anim.SetBool("walking", true);
                    WalkForward();
                    break;

                case movementState.BackwardWalk:
                    anim.SetBool("walking", true);
                    WalkBackward();
                    break;
                    
                case movementState.ForwardRun:
                    if (!anim.GetBool("walking")) {
                        anim.SetBool("walking", true);
                    }
                    Run();
                    break;

                case movementState.LeftStrafe:
                    if (!anim.GetBool("walking")) {
                        anim.SetBool("walking", true);
                    }
                    Strafe();
                    break;

                case movementState.RightStrafe:
                    if (!anim.GetBool("walking")) {
                        anim.SetBool("walking", true);
                    }
                    Strafe();
                    break;

                case movementState.Idle:
                    anim.SetBool("walking", false);
                    Idle();
                    break;

            }
            anim.SetFloat("velx", moveX);
            anim.SetFloat("vely", moveY);
        }

        if (ground != null) {
            var groundPosition = ground.position;
            Vector3 groundMovement = groundPosition - lastGroundPosition;
            controller.Move(groundMovement);
            lastGroundPosition = groundPosition;
        }

        moveDirection *= moveSpeed;

        if (canMove) {
            controller.Move(moveDirection * Time.deltaTime);
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    private void Idle() {
        
    }

    private void WalkForward() {
        moveSpeed = walkSpeed;
        
    }

    private void WalkBackward() {
        moveSpeed = strafeSpeed;

    }

    private void Strafe() {
        moveSpeed = strafeSpeed;

    }

    private void Run() {
        moveSpeed = runSpeed;

    }

    public void OnJump(InputAction.CallbackContext context) {
        if(isGrounded && canMove && Time.time > jumpCooldown) {
            /*
            if (canJump)
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
                anim.SetTrigger("jump");
                canJump = false;
            }
            */
            velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
            isJumping = true;
        }
    }

    public void OnBlock(InputAction.CallbackContext context) {
        if(!isAttacking && context.started) {
            isBlocking = true;
            anim.SetBool("block", true);
        }
        else if(context.canceled) {
            isBlocking = false;
            anim.SetBool("block", false);
        }
    }

    public void OnAttack(InputAction.CallbackContext context) {
        
        if(Time.time > attackCooldown && !isAttacking && !isBlocking) {
            StartCoroutine(Attack());
        }
    }

    private IEnumerator Attack() {
        WeaponController weapon = this.GetComponentInChildren<WeaponController>();
        Debug.Log(weapon);
        weapon.StartAttack();
        canMove = false;
        isAttacking = true;
        anim.SetBool("attack", isAttacking);
        anim.SetFloat("speed", 0, 0.1f, Time.deltaTime);
        
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);

        weapon.StopAttack();
        attackCooldown = Time.time + 0.1f;
        isAttacking = false;
        anim.SetBool("attack", isAttacking);
    }

    private void OnTriggerEnter(Collider hit)
    {
        if (hit.gameObject.CompareTag("JumpRelics"))
        {
            Debug.Log("Hit Jump Relics!");
            hit.gameObject.SetActive(false);
            canJump = true;
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.collider.tag == "Ground")
        {
            ground = hit.transform;
            lastGroundPosition = ground.position;
        }
    }

    private void ResetJumpAndFall() {
        
        velocity.y = -2f;
        if (isJumping) {
            isJumping = false;
            anim.SetBool("jump", isJumping);
            jumpCooldown = Time.time + .2f;
        }

        // stop falling
        if (anim.GetBool("fall")) {
            anim.SetBool("fall", false);
        }
    }
}
