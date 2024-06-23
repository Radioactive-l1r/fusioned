using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonControlelr : MonoBehaviour
{  public float speed = 5.0f;
    public float mouseSensitivity = 2.0f;
    public float jumpSpeed = 8.0f;
    public float gravity = 20.0f;

    private CharacterController characterController;
    private Vector3 moveDirection = Vector3.zero;
    private float rotationX = 0;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // Mouse look
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;

      

        transform.Rotate(0, mouseX, 0);
   //     Camera.main.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);

        // Movement
        if (characterController.isGrounded)
        {
            float moveDirectionY = moveDirection.y;
            moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection *= speed;

            if (Input.GetButton("Jump"))
            {
                moveDirection.y = jumpSpeed;
            }
            else
            {
                moveDirection.y = moveDirectionY;
            }
        }

        // Apply gravity
        moveDirection.y -= gravity * Time.deltaTime;

        // Move the controller
        characterController.Move(moveDirection * Time.deltaTime);
    }

}
