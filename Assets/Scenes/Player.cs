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
            RPC_Configure(Health);

        }
    }
    private ChangeDetector _changeDetector;

    public override void Spawned()
    {


        _changeDetector = GetChangeDetector(ChangeDetector.Source.SimulationState);

    }
    public override void Render()
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


    [Networked] public string playerName { get; set; }
    [Networked] public Color playerColor { get; set; }

    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    public void RPC_Configure(string name)
    {
        playerName = name;
        
    }
    NetworkObject myObject;// Get the reference somehow

    // Call the RPC_Configure method
}