using UnityEngine;

public class tps : MonoBehaviour
{
    public CharacterController _cc;
    public float speed = 5f;
    public float rotationSpeed = 10f; // Speed of rotation towards movement direction

    void Update()
    {
        // Get input direction
        Vector3 inputDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).normalized;

        if (inputDirection.magnitude >= 0.1f)
        {
            // Calculate movement direction relative to camera
            Vector3 camForward = Camera.main.transform.forward;
            Vector3 camRight = Camera.main.transform.right;

            camForward.y = 0;
            camRight.y = 0;
            camForward = camForward.normalized;
            camRight = camRight.normalized;

            Vector3 moveDirection = camForward * inputDirection.z + camRight * inputDirection.x;

            // Rotate towards movement direction
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            // Move the character
            _cc.Move(moveDirection * speed * Time.deltaTime);
        }
    }
}
