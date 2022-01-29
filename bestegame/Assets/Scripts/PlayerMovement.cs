using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public CharacterController controller;

    public float speed = 12f;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;
    public float stamina = 100f;
    bool stamina2 = true;

    public bool isShiftKeyDown;
    public bool isControlDown;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    public Transform playerBody;

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

        // if player crouches
        if (isControlDown)
        {

        }
        else
        {

        }
        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    }

    private void FixedUpdate()
    {
        isShiftKeyDown = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
        isControlDown = Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl);
        // If player is sprinting
        if (isShiftKeyDown && (Input.GetAxis("Vertical") == 1) && isGrounded && stamina > 0 && stamina2)
        {
            speed = 14f;
            stamina -= 20 * Time.deltaTime;
        }
        else
        {
            speed = 8f;
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
}
