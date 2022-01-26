using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public CharacterController controller;

    public float speed = 12f;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;

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
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if(isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        if (move.magnitude > 1)
        {
            move /= move.magnitude;
        }
        controller.Move(move * speed * Time.deltaTime);

        if(Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        isShiftKeyDown = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
        isControlDown = Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl);
        if(isShiftKeyDown && Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S))
        {
            speed = 14f;
            if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.S) && isGrounded) speed = 0f;
        }
        else
        {
            speed = 8f;
            if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.S) && isGrounded) speed = 0f;
        }
        if (isControlDown)
        {

        }
        else
        {

        }
        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    }
}
