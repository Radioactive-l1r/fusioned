using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
public class playerController : NetworkBehaviour
{
    private CharacterController _cc;
    public NetworkObject _gems;
    public float moveSpeed = 5f;
    private Vector3 moveDirection = Vector3.zero;
    private void Awake()
    {
        ;
        _cc = GetComponent<CharacterController>();
    }

    public override void Spawned()
    {
        
        if (Object.HasInputAuthority)
        {
            Camera.main.gameObject.SetActive(false);
            Vector3 spawnPosition = new Vector3(-50, -10, Runner.LocalPlayer.PlayerId + 2);
            transform.position = spawnPosition;
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
            moveDirection.y -= 20 * Time.deltaTime;

            // Move the controller
            _cc.Move(moveDirection * Time.deltaTime);
            if (data.buttons.IsSet(MyButtons.space) && HasStateAuthority)
            {
                //spawnGems();
            }
        }
    }

}
