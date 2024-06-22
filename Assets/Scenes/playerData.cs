using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class playerData : NetworkBehaviour
{
    [Networked]
    public bool spawnedProjectile { get; set; }

    [Networked]
    public string playername { get; set; }
    [Networked]
    public Color myColor { get; set; }  
    private ChangeDetector _changeDetector;

    public override void Spawned()
    {
        _changeDetector = GetChangeDetector(ChangeDetector.Source.SimulationState);


        if (HasInputAuthority)
        {
            spawnedProjectile = BasicSpawner.instance._host;
            playername=BasicSpawner.instance._playerName;
            myColor = BasicSpawner.instance._color;
        }

        if (spawnedProjectile)
        {
            transform.GetChild(2).gameObject.SetActive(true);
        }
        gameObject.name = playername;
      

    }

    public override void Render()
    {
        foreach (var change in _changeDetector.DetectChanges(this))
        {
            switch (change)
            {
                case nameof(spawnedProjectile):
                   
                    break;
                case nameof(myColor):

                    break;
            }
        }
    }

    private void Update()
    {
        gameObject.transform.GetChild(0).gameObject.GetComponent<Renderer>().material.color = myColor;
    }
}
