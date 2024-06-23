using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class SetCamerPos : NetworkBehaviour
{
   
    public Transform target;  // The target object to follow
    public float distance = 5.0f;  // Distance from the target
    public float height = 3.0f;    // Height from the target
    public float rotationSpeed = 50f;  // Speed of camera rotation
    public Vector3 offset;  // Additional offset from the target

    private float currentRotationAngle = 0.0f;
    private float currentHeight = 0.0f;

    public override void Spawned()
    {
        transform.parent = null;
        if (Object.HasInputAuthority)
        {
            gameObject.name = "CamHolder";
        }
    }


    public override void FixedUpdateNetwork()
    {
        if (GetInput(out NetworkInputData data))
        {
           

               // Calculate the current rotation angles
                currentRotationAngle += data.mouseX * rotationSpeed*Runner.DeltaTime;
                currentHeight -= data.mouseY * rotationSpeed*Runner.DeltaTime;
                currentHeight = Mathf.Clamp(currentHeight, -30.0f, 50.0f); // Limit the height angle

                // Convert angles to quaternion for rotation
                Quaternion rotation = Quaternion.Euler(currentHeight, currentRotationAngle, 0);

                // Calculate the desired position of the camera
                Vector3 desiredPosition = target.position + offset - (rotation * Vector3.forward * distance) + Vector3.up * height;

                // Smoothly move the camera towards the desired position
                transform.position = Vector3.Lerp(transform.position, desiredPosition, 0.1f);

                // Make the camera look at the target
                transform.LookAt(target.position + offset);
            

        }
    }

}