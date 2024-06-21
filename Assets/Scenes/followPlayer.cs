using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class followPlayer : MonoBehaviour
{
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
            // transform.position = new Vector3(player.position.x + xOffset, transform.position.y , player.position.z + zOffset);
            Vector3 targetPosition = new Vector3(player.position.x + xOffset,transform.position.y, player.position.z+zOffset);

            // Smoothly interpolate to the target position
            transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);
        }
    }
}
