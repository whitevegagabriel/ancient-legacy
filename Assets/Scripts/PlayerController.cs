using System.Collections;
using System.Collections.Generic;
using Combat;
using TMPro;
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
    private float jumpCooldown;
    private WeaponController weapon;
    
    private Animator anim;
    private CharacterController controller;

    private Transform ground;
    private Vector3 lastGroundPosition;

    public bool canMove; // used to prevent the character from moving during an animation
    public bool isAttacking;
    public bool isJumping;
    public bool isBlocking;
    public bool isRunning;

    public bool isDefeated;
    private movementState direction;

    public InputAction playerControls;
    private Vector2 moveInput;
    [SerializeField] InputAction input;

    float lastGroundedTime;
    private Targetable targetable;

    //Collectable relics parameters
    public int jumpCount = 0; //set public for testing purpose
    public int runCount = 0; //set public for testing purpose


    void Awake() {
        anim = GetComponentInChildren<Animator>();
        controller = GetComponent<CharacterController>();
        targetable = GetComponent<Targetable>();
        targetable.InitHealth(10);
    }

    void Start() {
        canMove = true;
        isAttacking = false;
        isJumping = false;
        isGrounded = false;
        isBlocking = false;
        isRunning = false;
        isDefeated = false;
        direction = movementState.Idle;
        input = new InputAction();
        lastGroundedTime = Time.time;

    }

    void FixedUpdate() {
        
    }

	void Update () {   
        isDefeated = targetable.GetHealth() <= 0 ? true : false;

        if (isDefeated) {
            anim.SetTrigger("defeated");
            return;
        }

        if (isAttacking == false) {
            canMove = true;
        }

        isGrounded = controller.isGrounded;

        lastGroundedTime = isGrounded ? Time.time : lastGroundedTime;

        if(isGrounded && velocity.y < 0) {
            ResetJumpAndFall();
        }

        Move();

        if (isJumping) {
            anim.SetBool("jump", isJumping);
        }

        if (!isGrounded && !isJumping && Time.time > (lastGroundedTime + 1f)) {
            anim.SetBool("fall", true);
        }
	}

    public void OnMove(InputAction.CallbackContext context) {
        moveInput = context.ReadValue<Vector2>();
    }

    private void Move() {

        float moveY = moveInput.y;
        float moveX = moveInput.x;
        
        moveDirection = new Vector3(moveX, 0, moveY);
        moveDirection = transform.TransformDirection(moveDirection);

        transform.position += moveDirection * moveY * Time.deltaTime;

        if (!isRunning) {
            moveY = Mathf.Clamp(moveY, -1, 0.5f);
            direction = movementState.ForwardWalk;
        }
        else {
            direction = movementState.ForwardRun;
        }
        
        if (moveY == 0f && moveX == 0f) {
            direction = movementState.Idle;
        }

        // Walking backwards
        if (moveY < 0.0) {
            direction = movementState.BackwardWalk; 
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
                    moveY = 0.5f;
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
            var groundMovement = groundPosition - lastGroundPosition;
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

    public void OnRun(InputAction.CallbackContext context) {
        if(context.started && runCount == 3) {
            isRunning = true;
        }
        else if(context.canceled) {
            isRunning = false;
        }
    }
    public void OnJump(InputAction.CallbackContext context) {
        if(isGrounded && canMove && Time.time > jumpCooldown && jumpCount == 3) {
            velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
            isJumping = true;
            EventManager.TriggerEvent<JumpEvent, Vector3>(transform.position);
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
        
        if(!isAttacking && !isBlocking) {
            StartCoroutine(Attack());
        }
    }

    private IEnumerator Attack() {
        WeaponController weapon = GetComponentInChildren<WeaponController>();
        weapon.SetDamage(2);
        
        Debug.Log(weapon);
        weapon.StartAttack();
        EventManager.TriggerEvent<AttackEvent, Vector3>(GameObject.Find("Sword").transform.position);
        canMove = false;
        isAttacking = true;
        anim.SetBool("attack", isAttacking);
        anim.SetFloat("speed", 0, 0.1f, Time.deltaTime);
        
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);

        weapon.StopAttack();
        isAttacking = false;
        anim.SetBool("attack", isAttacking);
    }

    private void OnTriggerEnter(Collider hit)
    {
        if (hit.gameObject.CompareTag("JumpRelics"))
        {
            Debug.Log("Hit Jump Relics!");
            jumpCount++;
        }
        if (hit.gameObject.CompareTag("RunRelics"))
        {
            Debug.Log("Hit Run Relics!");
            runCount++;
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.collider.tag == "Ground" && ground != hit.transform)
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
            EventManager.TriggerEvent<PlayerLandsEvent, Vector3, float>(transform.position, 301f);
        }

        // stop falling
        if (anim.GetBool("fall")) {
            anim.SetBool("fall", false);
            EventManager.TriggerEvent<PlayerLandsEvent, Vector3, float>(transform.position, 501f);
        }
    }
}
