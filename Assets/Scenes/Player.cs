using Fusion;
using System.Diagnostics;
using TMPro;
using UnityEngine;

public class Player : NetworkBehaviour
{

  public  Camera mycamera;

    [Networked]
    public string myname { get; set; }
    [Networked]


    public Color myColor { get; set; }
    private CharacterController _cc;
    public NetworkObject gems;

    public Transform target;  // The target object to follow
    public Transform holder;
    public float distance = 5.0f;  // Distance from the target
    public float height = 3.0f;    // Height from the target
    public float rotationSpeed = 50f;  // Speed of camera rotation
    public Vector3 offset;  // Additional offset from the target

    private float currentRotationAngle = 0.0f;
    private float currentHeight = 0.0f;
    public float moveSpeed = 5f;
    private Vector3 moveDirection = Vector3.zero;

    private void Awake()
    {
        _cc = GetComponent<CharacterController>();
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
                moveDirection =data.direction;
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
                spawnGems();
            }    
        }
    }
    public override void Spawned()
    {
        if (this.HasInputAuthority )
        {

            RPC_SendMessage(BasicSpawner.instance._playerName, BasicSpawner.instance._host,BasicSpawner.instance._color);

        }
        if (Object.HasInputAuthority)
        {

          
            holder.name = "CamHolder";
            // Camera.main.GetComponent<followPlayer>().player = this.transform;
            Camera.main.gameObject.SetActive(false);


        }
        else
        {
            Camera localCamera = GetComponentInChildren<Camera>();
            localCamera.enabled = false;
        }

        holder.parent = null;
    }





    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority, HostMode = RpcHostMode.SourceIsHostPlayer)]
    public void RPC_SendMessage(string name,bool ishost, Color playerColor,RpcInfo info = default)
    {
        RPC_RelayMessage(name, info.Source,ishost,playerColor);
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All, HostMode = RpcHostMode.SourceIsServer)]
    public void RPC_RelayMessage(string name, PlayerRef messageSource, bool ishost, Color playerColor)
    {
        myname = name;
        myColor = playerColor;
    }

  
    private void Update()
    {
        this.gameObject.name = myname;
        gameObject.transform.GetChild(0).gameObject.GetComponent<Renderer>().material.color = myColor;
            }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag=="treasure")
        {
            //Destroy(collision.gameObject);
            Runner.Despawn(collision.gameObject.GetComponent<NetworkObject>());
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "treasure")
        {
            //Destroy(collision.gameObject);
            Runner.Despawn(other.gameObject.GetComponent<NetworkObject>());
        }
    }
    public void spawnGems()
    {
        Runner.Spawn(gems, Vector3.zero, Quaternion.identity);
    }
}