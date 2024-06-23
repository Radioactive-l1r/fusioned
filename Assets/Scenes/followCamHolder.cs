using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class followCamHolder : MonoBehaviour
{
    bool isCamSet=false;
    public Transform camHolder;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
     
        if(camHolder!=null && !isCamSet)
        {
            transform.position = Vector3.zero;
            transform.SetParent(camHolder);
            isCamSet = true;
        }
        else
        {
            camHolder = GameObject.Find("CamHolder").transform;
        }
       
    }
}
