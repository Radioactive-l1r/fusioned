using Fusion;
using System.Diagnostics;
using TMPro;
using UnityEngine;

public class Player : NetworkBehaviour
{
    public Camera Camera;

    [Networked]
    public string myname { get; set; }
    [Networked]


    public Color myColor { get; set; }
    private NetworkCharacterController _cc;
    public NetworkObject gems;

    private void Awake()
    {
        _cc = GetComponent<NetworkCharacterController>();
    }
    public override void FixedUpdateNetwork()
    {
        if (GetInput(out NetworkInputData data))
        {
           
            if (data.direction.magnitude >= 0.1f)
            {
                // Calculate movement direction relative to camera
                Vector3 camForward = Camera.transform.forward;
                Vector3 camRight = Camera.transform.right;

                camForward.y = 0;
                camRight.y = 0;
                camForward = camForward.normalized;
                camRight = camRight.normalized;

                Vector3 moveDirection = camForward * data.direction.z + camRight * data.direction.x;

                // Rotate towards movement direction
                Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, 10f * Time.deltaTime);

                // Move the character
                _cc.Move(moveDirection * 5f * Time.deltaTime);
            }

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

            // Camera.main.GetComponent<followPlayer>().player = this.transform;

        }
        if (this.HasInputAuthority && Input.GetKeyDown(KeyCode.Space))
        {
         
        }
        if (HasStateAuthority)
        {
          //  Camera = Camera.main;
           // Camera.GetComponent<ThirdPersonCamera>().target = transform;
        }

        Camera = Instantiate(Camera);
        Camera.GetComponent<ThirdPersonCamera>().target = transform;

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
        Invoke("SetPlayerName", 1f);
    }

    private void SetPlayerName()
    {
      
      
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