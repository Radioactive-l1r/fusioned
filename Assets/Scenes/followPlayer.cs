using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class followPlayer : MonoBehaviour
{
    public float distance = 5.0f;  // Distance from the target
    public float height = 3.0f;    // Height from the target
    public float rotationSpeed = 1.0f;  // Speed of camera rotation

    private float currentRotationAngle = 0.0f;
    private float currentHeight = 0.0f;



    public Transform player;
    public Vector3 offset;
    public float xOffset = 1.0f; // Adjust this value as needed
    public float zOffset = 2.0f;
    public float smoothSpeed = 0.125f; // Adjust this for desired smoothness

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            currentRotationAngle += Input.GetAxis("Mouse X") * rotationSpeed;
            currentHeight -= Input.GetAxis("Mouse Y") * rotationSpeed;
            currentHeight = Mathf.Clamp(currentHeight, -30.0f, 50.0f); // Limit the height angle

            // Convert angles to quaternion for rotation
            Quaternion rotation = Quaternion.Euler(currentHeight, currentRotationAngle, 0);

            // Calculate the desired position of the camera
            Vector3 desiredPosition = player.position + offset - (rotation * Vector3.forward * distance) + Vector3.up * height;

            // Smoothly move the camera towards the desired position
            transform.position = Vector3.Lerp(transform.position, desiredPosition, 0.1f);

            // Make the camera look at the target
            transform.LookAt(player.position + offset);



            // transform.position = new Vector3(player.position.x + xOffset, transform.position.y , player.position.z + zOffset);
            /**Vector3 targetPosition = new Vector3(player.position.x + xOffset,transform.position.y, player.position.z+zOffset);

            // Smoothly interpolate to the target position
            transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);
            **/

        }
    }
}
