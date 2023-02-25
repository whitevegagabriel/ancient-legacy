using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float turnSpeed;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float walkSpeed;
    [SerializeField] private float jumpDistance;
    [SerializeField] private float runSpeed;
    private Vector3 moveDirection;
    private Vector3 velocity;
    [SerializeField] private bool isGrounded;
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float gravity;
    [SerializeField] private float jumpHeight;
    private float attackCooldown;
    private WeaponController weapon;
    
    private Animator anim;
    private CharacterController controller;

    private bool canJump = true;
    private Transform ground;
    private Vector3 lastGroundPosition;

    void Start() {
        anim = GetComponentInChildren<Animator>();
        controller = GetComponent<CharacterController>();
        turnSpeed = 90f;
    }

    void FixedUpdate() {
        if(Input.GetKey(KeyCode.Mouse0) && Time.time > attackCooldown) {
            StartCoroutine(Attack());
        }
    }

	void Update () {   
        anim.ResetTrigger("jump"); 
        Move();
	}

    private void Move() {
        isGrounded = Physics.CheckSphere(transform.position, groundCheckDistance, groundMask);

        if(isGrounded && velocity.y < 0) {
            velocity.y = -2f;
        }

        float moveZ = Input.GetAxis("Vertical");
        
        moveDirection = new Vector3(0, 0, moveZ);
        moveDirection = transform.TransformDirection(moveDirection);
        //Debug.Log("Move direction: " + moveDirection);

        transform.position += moveDirection * moveZ * Time.deltaTime;
        // Walking backwards
        if (moveZ < 0.0) {
            transform.Rotate( 0 , -1*(Input.GetAxis("Horizontal") * turnSpeed * Time.deltaTime) , 0 );
        }
        // Walking forward
        else {
            transform.Rotate( 0 , Input.GetAxis("Horizontal") * turnSpeed * Time.deltaTime , 0 );
        }

        if(isGrounded) {
            if (moveDirection != Vector3.zero && !Input.GetKey(KeyCode.LeftShift)) {
                anim.SetBool("walkforward", true);
                Walk();
            }
            else if (moveDirection != Vector3.zero && Input.GetKey(KeyCode.LeftShift)) {
                Run();
            }
            else if(moveDirection == Vector3.zero) {
                anim.SetBool("walkforward", false);
                Idle();
            }

            if (Input.GetKeyDown(KeyCode.Space)) {
                Jump();
            }
        }

        moveDirection *= moveSpeed;
        
        // account for ground movement
        if (ground != null) {
            var groundPosition = ground.position;
            Vector3 groundMovement = groundPosition - lastGroundPosition;
            controller.Move(groundMovement);
            lastGroundPosition = groundPosition;
        }

        controller.Move(moveDirection * Time.deltaTime);

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    private void Idle() {
        anim.SetFloat("speed", 0, 0.1f, Time.deltaTime);
    }

    private void Walk() {
        moveSpeed = walkSpeed;
        anim.SetFloat("speed", 0.5f, 0.1f, Time.deltaTime);
    }

    private void Run() {
        moveSpeed = runSpeed;
        anim.SetFloat("speed", 1f, 0.1f, Time.deltaTime);
    }

    private void Jump() {
        Debug.Log(canJump);
        /*
        if (canJump)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
            anim.SetTrigger("jump");
            canJump = false;
        }
        */
        velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
        anim.SetTrigger("jump");
    }

    private IEnumerator Attack() {
        WeaponController weapon = this.GetComponentInChildren<WeaponController>();
        anim.SetLayerWeight(anim.GetLayerIndex("Attack Layer"), 1);
        anim.SetTrigger("attack");
        weapon.setIsAttacking(true);

        yield return new WaitForSeconds(0.9f);
        anim.ResetTrigger("attack");
        anim.SetLayerWeight(anim.GetLayerIndex("Attack Layer"), 0);
        weapon.setIsAttacking(false);
        attackCooldown = Time.time + 0.9f;
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
}
