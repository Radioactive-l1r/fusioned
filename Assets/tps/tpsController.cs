using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tpsController : MonoBehaviour
{
    public CharacterController _characterController;
    public float speed = 6f;
    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;
    public Transform Cam;
    Vector3 direction;
    // Update is called once per frame
    void Update()
    {

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        // Get the camera's forward and right vectors
        Vector3 cameraForward = Cam.forward;
        Vector3 cameraRight = Cam.right;

        // Keep the vectors on the horizontal plane (ignore the y component)
        cameraForward.y = 0;
        cameraRight.y = 0;

        // Normalize the vectors to ensure consistent movement speed
        cameraForward.Normalize();
        cameraRight.Normalize();
        if (_characterController.isGrounded)
        {
            float moveDirectionY = direction.y;

            // Create movement direction based on camera orientation
            direction = (cameraForward * vertical + cameraRight * horizontal).normalized;

            // Move the player
            if (direction.magnitude >= 0.1f)
            {
               
                // Calculate the target rotation based on the movement direction
                Quaternion targetRotation = Quaternion.LookRotation(direction);

                // Smoothly rotate the player to the target rotation
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * speed);
            }
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                direction.y = 8;
            }else
            {
                direction.y = moveDirectionY;

            }


        }
        direction.y -= 20 * Time.deltaTime;
        _characterController.Move(direction * speed * Time.deltaTime);

    }
}
