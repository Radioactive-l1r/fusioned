using Fusion;
using System.Diagnostics;
using TMPro;
using UnityEngine;

public class Player : NetworkBehaviour
{
    [Networked]
    public bool spawnedProjectile { get; set; }
    public Renderer red;
    public  bool mybool;


    [Networked]
    public string Health { get; set; }
    private void Start()
    {
        if (this.HasInputAuthority)
        {
            spawnedProjectile = BasicSpawner.instance._host;
            Health = BasicSpawner.instance._playerName;
            //  RPC_Configure(Health,BasicSpawner.instance._host,BasicSpawner.instance._color);

            RPC_SendMessage(BasicSpawner.instance._playerName, BasicSpawner.instance._host);

          

        }
    }
    private ChangeDetector _changeDetector;
    private void Update()
    {
        if (normalHoste)
        {
            transform.GetChild(1).gameObject.SetActive(true);
        }
    }
    public override void Spawned()
    {
      
    }
    /**  public override void Render()
      {
          foreach (var change in _changeDetector.DetectChanges(this))
          {
              switch (change)
              {
                  case nameof(spawnedProjectile):
                      mybool = spawnedProjectile;
                      red.material.color = Color.white;
                      break;
              }
          }
      }
    */
    [Networked]
    public bool normalHoste { get; set; }

    [Networked] public string playerName { get; set; }
    [Networked] public bool isHost { get; set; }

    [Networked] public Color playerColor { get; set; }

    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    public void RPC_Configure(string name,bool ishost,Color col)
    {
        playerName = name;
        isHost = ishost;
        playerColor=col;
            red.material.color = playerColor;
        gameObject.name = name;
      


    }


    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]

    // [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority, HostMode = RpcHostMode.SourceIsHostPlayer)]
    public void RPC_SendMessage(string name,bool ishost, RpcInfo info = default)
    {
        RPC_RelayMessage(name, info.Source,ishost);
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
  //  [Rpc(RpcSources.StateAuthority, RpcTargets.All, HostMode = RpcHostMode.SourceIsServer)]
    public void RPC_RelayMessage(string name, PlayerRef messageSource, bool ishost)
    {
        /**if (_messages == null)
              _messages = FindObjectOfType<TMP_Text>();

        if (messageSource == Runner.LocalPlayer)
        {
            message = $"You said: {message}\n";
        }
        else
        {
            message = $"Some other player said: {message}\n";
        }

        _messages.text += message;**/
        if (messageSource == Runner.LocalPlayer)
        {
            gameObject.name = name + " rpc";

            // message = $"You said: {message}\n";
        }
        else
        {
            gameObject.name = name + " rpc";

            //message = $"Some other player said: {message}\n";
        }
        normalHoste = ishost;


    }
    NetworkObject myObject;// Get the reference somehow

    // Call the RPC_Configure method
}