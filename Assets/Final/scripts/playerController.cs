using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using TMPro;
using Unity.VisualScripting;
public class playerController : NetworkBehaviour
{
    private CharacterController _cc;
    public NetworkObject _gems;
    public NetworkObject _gemSpawnEffect;
    public float moveSpeed = 5f;
    private Vector3 moveDirection = Vector3.zero;
    public Transform cam;
    float xRotation;
    int collectedGems = 0;
    //
  public  TMP_Text TX_totalgems;

    public NetworkObject gems;
    [Networked]

    public int Mygems { get; set; }
    GameObject MapCamAnchor;

    public NetworkObject PlayerIcon;
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
            MapCamAnchor = GameObject.Find("MapCamAnchor");




            MapCamAnchor.transform.localPosition = new Vector3(0, 10, 0);
            FindObjectOfType<MiniMapController>().gameObject.GetComponent<MiniMapController>().Player = this.transform;



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
            transform.Rotate(0, mouseX*3, 0);

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

            moveDirection.y -= 20 * Runner.DeltaTime;

            _cc.Move(moveDirection * Runner.DeltaTime);
            float mouseY = data.mouseY* 150f * Runner.DeltaTime;

            xRotation -= mouseY;

            xRotation = Mathf.Clamp(xRotation, -45f, 45f);

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
         collectedGems++;
            Mygems++;
            if (TX_totalgems!=null)
            {
                TX_totalgems.SetText("gems : " + Mygems);
            }


            Runner.Spawn(_gemSpawnEffect, other.gameObject.transform.position, Quaternion.identity);

            AudioManager.instance.PlayAudio("gem_collected");
            Runner.Despawn(other.gameObject.GetComponent<NetworkObject>());
        }
    }
    private void Update()
    {
        if (TX_totalgems != null)
        {
            TX_totalgems.SetText("gems : " + Mygems);
        }
        

        PlayerIcon.transform.position = new Vector3(transform.position.x, 8, transform.position.z);
        PlayerIcon.transform.rotation=transform.rotation;
    }
}
