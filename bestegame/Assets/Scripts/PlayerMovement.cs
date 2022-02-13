using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public CharacterController controller;

    [Header("Movement Parameters")]
    [SerializeField] private float normalSpeed = 5f;
    [SerializeField] private float sprintSpeed = 8f;
    [SerializeField] private float crouchSpeed = 2.5f;
    [SerializeField]
    private float speed = 8;

    [Header("Jump Parameters")]
    [SerializeField] public float gravity = -9.81f;
    [SerializeField] public float jumpHeight = 3f;
    [SerializeField] public float stamina = 100f;
    bool stamina2 = true;

    public bool isShiftKeyDown;
    public bool isControlDown;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    public Transform playerBody;

    [Header("Crouch Parameters")]
    [SerializeField] private float CrouchTime = 0.3f;
    [SerializeField] private float crouchHeight = 1.4f;
    [SerializeField] public float NormalHeight = 1.8f;
    [SerializeField] private Vector3 standingCenter = new Vector3(0, 1, 0);
    [SerializeField] private Vector3 crouchingCenter = new Vector3(0, 1.5f, 0);
    private bool isCrouching;
    private bool duringCrouchAnimation;
    private bool wantsToCrouch;


    Vector3 velocity;
    bool isGrounded;

    // Update is called once per frame
    void Update()
    {
        // look if player is grounded
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if(isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        // player movement
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 move = transform.right * x + transform.forward * z;
        if (move.magnitude > 1)
        {
            move /= move.magnitude;
        }
        controller.Move(move * speed * Time.deltaTime);

        // if player jumps
        if(Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        if(Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyUp(KeyCode.LeftControl))
        {
            wantsToCrouch = true;
        }
        // if player crouches
        if(wantsToCrouch && isGrounded && !duringCrouchAnimation)
        {
            StartCoroutine(Crouch());
            wantsToCrouch = false;
        }


        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    }

    private void FixedUpdate()
    {
        isShiftKeyDown = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
        isControlDown = Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl);

        if(isCrouching)
        {
            speed = crouchSpeed;
            return;
        }

        // If player is sprinting
        if (isShiftKeyDown && (Input.GetAxis("Vertical") == 1) && isGrounded && stamina > 0 && stamina2)
        {
            speed = sprintSpeed;
            stamina -= 20 * Time.deltaTime;
        }
        else
        {
            speed = normalSpeed;
            stamina2 = false;
            // stamina needs to reach at least 20 befor sprinting
            if (stamina > 20) stamina2 = true;
            // exponential stamina regen
            if (stamina < 100)
            {
                stamina += 4 * (1 + (stamina / 20)) * Time.deltaTime;
                if (stamina > 100) stamina = 100;
            }
        }
    }

    //Coroutine (used for delaying a event while everything else is still working)
    IEnumerator Crouch()
    {
        Debug.Log("start crouching");
        duringCrouchAnimation = true;

        float timeElapsed = 0;
        float targetHeight = isCrouching ? NormalHeight : crouchHeight;
        float currentHeight = controller.height;
        Vector3 targetCenter = isCrouching ? standingCenter : crouchingCenter;
        Vector3 currentCenter = controller.center;

        while(timeElapsed < CrouchTime)
        {
            controller.height = Mathf.Lerp(currentHeight, targetHeight, timeElapsed / CrouchTime);
            controller.center = Vector3.Lerp(currentCenter, targetCenter, timeElapsed / CrouchTime);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        controller.height = targetHeight;
        controller.center = targetCenter;

        isCrouching = !isCrouching;

        duringCrouchAnimation = false;
        Debug.Log("finished crouching");
    }
}
