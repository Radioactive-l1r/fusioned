using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
public class gemSpawner : NetworkBehaviour
{   

    public NetworkObject gems;
    bool isSpawned=false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(PlayerPrefs.GetString("type").Contains("HOST"))
        {
            if(Input.GetKeyDown(KeyCode.Space) && !isSpawned)
            {
                spawnGems();
            }
        }
    }

     public void spawnGems()
    {   

        foreach(Transform spawnT in transform)
        {
            spawnT.gameObject.GetComponent<MeshRenderer>().enabled = false;
            Runner.Spawn(gems, spawnT.position, Quaternion.identity);
        }
        isSpawned = true;
      
    }
}
