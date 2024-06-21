using Fusion;
using System.Diagnostics;
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
            RPC_Configure(Health,BasicSpawner.instance._host,BasicSpawner.instance._color);

        }
    }
    private ChangeDetector _changeDetector;

    public override void Spawned()
    {
      
       if(isHost)
        {
           
        }

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
        if(isHost)
        {
            transform.GetChild(1).gameObject.SetActive(true);
        }
       


    }
    NetworkObject myObject;// Get the reference somehow

    // Call the RPC_Configure method
}