using System;
using System.Collections;
using System.Collections.Generic;
using Collectables;
using Combat;
using StateManagement;
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

    public enum airState
    {
        Jump,
        Fall
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

    private List<Transform> groundPriority = new List<Transform>();
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
    public static int health = 10;
    public GameObject playerHand;
    private RelicsCountManager relicsCountManager;

    private GameObject controls;
    private GameObject controlsWithoutRun;
    private GameObject controlsWithoutJumpOrRun;

    void Awake() {
        anim = GetComponentInChildren<Animator>();
        controller = GetComponent<CharacterController>();
        targetable = GetComponent<Targetable>();
        relicsCountManager = GetComponent<RelicsCountManager>();
        targetable.InitHealth(health, 10 + PlayerInventory.ItemCount(HeartController.CollectableName));
        controls = GameObject.Find("Controls");
        controlsWithoutRun = GameObject.Find("ControlsWithoutRun");
        controlsWithoutJumpOrRun = GameObject.Find("ControlsWithoutJumpOrRun");
        if (PlayerState.JumpCount == 3 && PlayerState.RunCount == 3) {
            setControls(true, false, false);
        }
        else if (PlayerState.JumpCount == 3) {
            setControls(false, true, false);
        }
        else {
            setControls(false, false, true);
        }
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
        isDefeated = targetable.GetHealth() <= 0;

        if (isDefeated) {
            anim.SetTrigger("defeated");
            return;
        }


        if (!isAttacking) {
            isGrounded = controller.isGrounded;

            lastGroundedTime = isGrounded ? Time.time : lastGroundedTime;
        }

        if(isGrounded && velocity.y < 0) {
            ResetJumpAndFall();
        }

        Move();

        if (isJumping) {
            anim.SetBool("jump", isJumping);
        }

        if (!isGrounded && !isJumping && Time.time > lastGroundedTime + .5f) {
            anim.SetBool("fall", true);
        }
	}

    public void OnMove(InputAction.CallbackContext context) {
        moveInput = context.ReadValue<Vector2>();
    }

    private void Move() {

        float moveY = moveInput.y;
        float moveX = moveInput.x;
        
        float inputMagnitude = Mathf.Clamp01(moveDirection.magnitude);
        moveDirection = new Vector3(moveX, 0, moveY);
        moveDirection = transform.TransformDirection(moveDirection);
        
        velocity.y += gravity * Time.deltaTime;

        if (!isRunning) {
            moveY = Mathf.Clamp(moveY, -1, 0.66f);
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
                    if (!isBlocking) { 
                        anim.SetBool("walking", true);
                    }
                    WalkForward();
                    moveY = 0.66f;
                    break;

                case movementState.BackwardWalk:
                    if (!isBlocking) { 
                        anim.SetBool("walking", true);
                    }
                    WalkBackward();
                    break;
                    
                case movementState.ForwardRun:
                    if (!isBlocking) { 
                        if (!anim.GetBool("walking")) {
                            anim.SetBool("walking", true);
                        }
                    }
                    Run();
                    break;

                case movementState.LeftStrafe:
                    if (!isBlocking) { 
                        if (!anim.GetBool("walking")) {
                            anim.SetBool("walking", true);
                        }
                    }
                    Strafe();
                    break;

                case movementState.RightStrafe:
                    if (!isBlocking) { 
                        if (!anim.GetBool("walking")) {
                            anim.SetBool("walking", true);
                        }
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

        if (groundPriority.Count > 0)
        {
            var ground = groundPriority[0];
            var groundPosition = ground.position;
            var groundMovement = groundPosition - lastGroundPosition;
            controller.Move(groundMovement);
            lastGroundPosition = groundPosition;
        } 

        if(!isGrounded && canMove) {
            Vector3 localVelocity = moveDirection * inputMagnitude * 3f;
            localVelocity.y = velocity.y;
            controller.Move(localVelocity * Time.deltaTime);
        }
    }

    void OnAnimatorMove() {
        if (isGrounded) { 
            Vector3 velocitySpeed = anim.deltaPosition;
            velocitySpeed.y = velocity.y  * Time.deltaTime;
            if (canMove) {        
                controller.Move(velocitySpeed);
            }   
        }
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
        if(context.started && PlayerState.RunCount == 3) {
            isRunning = true;
        }
        else if(context.canceled) {
            isRunning = false;
        }
    }
    public void OnJump(InputAction.CallbackContext context) {
        if(isGrounded && canMove && Time.time > jumpCooldown && PlayerState.JumpCount == 3) {
            velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
            isJumping = true;
            EventManager.TriggerEvent<JumpEvent, Vector3>(transform.position);
        }
    }

    public void OnBlock(InputAction.CallbackContext context) {
        if(!isAttacking && context.started) {
            isBlocking = true;
            anim.SetBool("block", true);
            anim.SetBool("walking", false);
        }
        else if(context.canceled) {
            isBlocking = false;
            anim.SetBool("block", false);
        }
    }

    public void OnAttack(InputAction.CallbackContext context) {
        
        if(!isAttacking && !isBlocking && isGrounded) {
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

        canMove = true;

        weapon.StopAttack();
        isAttacking = false;
        anim.SetBool("attack", isAttacking);
    }

    private void OnTriggerEnter(Collider hit)
    {
        if (hit.gameObject.CompareTag("JumpRelics"))
        {
            Debug.Log("Hit Jump Relics!");
            PlayerState.JumpCount++;
            relicsCountManager.updateJumpRelicImage();
            if (PlayerState.JumpCount == 3) {
                setControls(false, true, false);
            }
        }
        if (hit.gameObject.CompareTag("RunRelics"))
        {
            Debug.Log("Hit Run Relics!");
            PlayerState.RunCount++;
            relicsCountManager.updateRunRelicImage();
            if (PlayerState.RunCount == 3) {
                setControls(true, false, false);
            }
        }
        if (hit.gameObject.CompareTag("Ground") && !groundPriority.Contains(hit.transform))
        {
            if (groundPriority.Count == 0)
            {
                lastGroundPosition = hit.transform.position;
            }
            groundPriority.Add(hit.transform);
        }
        var collectable = hit.gameObject.GetComponent<ICollectable>();
        if (collectable != null)
        {
            PlayerInventory.AddItem(collectable.Name, 1);
            if (collectable.Name == HeartController.CollectableName)
            {
                targetable.IncreaseMaxHealth(1);
                targetable.ResetHealth();
            }
        }
    }

    private void OnTriggerExit(Collider hit)
    {
        if (hit.gameObject.CompareTag("Ground") && groundPriority.Contains(hit.transform))
        {
            groundPriority.Remove(hit.transform);
            if (groundPriority.Count > 0)
            {
                lastGroundPosition = groundPriority[0].position;
            }
        }
    }

    private void ResetJumpAndFall() {
        
        velocity.y = -2f;
        if (isJumping) {
            isJumping = false;
            anim.SetBool("jump", isJumping);
            jumpCooldown = Time.time + .2f;
            EventManager.TriggerEvent<PlayerLandsEvent, Vector3, airState>(transform.position, airState.Jump);
        }
        
        // stop falling
        if (anim.GetBool("fall")) {
            anim.SetBool("fall", false);
            EventManager.TriggerEvent<PlayerLandsEvent, Vector3, airState>(transform.position, airState.Fall);
        }
    }

    private void setControls(bool setControls, bool setControlsWithoutRun, bool setControlsWithoutJumpAndRun) {
        controls.SetActive(setControls);
        controlsWithoutRun.SetActive(setControlsWithoutRun);
        controlsWithoutJumpOrRun.SetActive(setControlsWithoutJumpAndRun);
    }
}