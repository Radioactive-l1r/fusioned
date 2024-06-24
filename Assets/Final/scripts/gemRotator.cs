using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class gemRotator : NetworkBehaviour
{   
    public float rotationSpeed = 50f; 
    public float floatAmplitude = 0.5f; 
    public float floatFrequency = 1f; 

    private float direction=1;
    private Vector3 startPosition;
  public  NetworkObject NetworkObject;
    void Start()
    {
        
    }
    public override void Spawned()
    {
        direction = Random.Range(0, 2) == 0 ? -1f : 1f;
        startPosition = NetworkObject.transform.position;
    }
    public override void FixedUpdateNetwork()
    {
        NetworkObject.transform.Rotate(0, rotationSpeed * direction * Time.deltaTime, 0);

        float newY = startPosition.y + Mathf.Sin(Time.time * floatFrequency) * floatAmplitude;

        NetworkObject.transform.position = new Vector3(NetworkObject.transform.position.x, newY, NetworkObject.transform.position.z);
    }
}
