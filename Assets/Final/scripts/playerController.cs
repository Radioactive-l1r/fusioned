using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using TMPro;
public class playerController : NetworkBehaviour
{
    private CharacterController _cc;
    public NetworkObject _gems;
    public float moveSpeed = 5f;
    private Vector3 moveDirection = Vector3.zero;
    public Transform cam;
    float xRotation;
    int collectedGems = 0;
    //
  public  TMP_Text TX_totalgems;

    public NetworkObject gems;
    private void Awake()
    {
       
        _cc = GetComponent<CharacterController>();
    }

    public override void Spawned()
    {
        
        if (Object.HasInputAuthority)
        {
            Camera.main.gameObject.SetActive(false);
            TX_totalgems = GameObject.Find("TX_totalgems").GetComponent<TMP_Text>();
            TX_totalgems.SetText("local");

        }
        else
        {
            Camera localCamera = GetComponentInChildren<Camera>();
            localCamera.enabled = false;
        }   
    }
    public override void FixedUpdateNetwork()
    {
        if (GetInput(out NetworkInputData data))
        {

            float mouseX = data.mouseX;
            transform.Rotate(0, mouseX, 0);

            if (_cc.isGrounded)
            {
                float moveDirectionY = moveDirection.y;
                moveDirection = data.direction;
                moveDirection = transform.TransformDirection(moveDirection);
                moveDirection *= moveSpeed;

                if (data.buttons.IsSet(MyButtons.jump))
                {
                    moveDirection.y = 8;
                }
                else
                {
                    moveDirection.y = moveDirectionY;
                }
            }

            // Apply gravity
            moveDirection.y -= 20 * Runner.DeltaTime;

            // Move the controller
            _cc.Move(moveDirection * Runner.DeltaTime);
            //camera input
            float mouseY = data.mouseY* 100f * Runner.DeltaTime;

            // Apply the mouse input to the camera's local X-axis rotation
            xRotation -= mouseY;

            // Clamp the rotation to prevent the camera from rotating too far up or down
            xRotation = Mathf.Clamp(xRotation, -45f, 45f);

            // Apply the rotation to the camera
            cam.transform.localRotation = Quaternion.Euler(xRotation, 0.0f, 0.0f);
            if (data.buttons.IsSet(MyButtons.space) && HasStateAuthority)
            {
                spawnGems();
            }
        }
    }

    void spawnGems()
    {
       // Runner.Spawn(gems, new Vector3(-7,-20,0), Quaternion.identity);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "treasure")
        {
            //Destroy(collision.gameObject);
         collectedGems++;
          //  TX_totalgems.SetText("gems : " + collectedGems);
          if(Object.HasInputAuthority)
            {
                TX_totalgems.SetText("gems : " + collectedGems);
            }
           
            AudioManager.instance.PlayAudio("gem_collected");
            Runner.Despawn(other.gameObject.GetComponent<NetworkObject>());
        }
    }
}
