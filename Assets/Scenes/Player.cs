using Fusion;
using System.Diagnostics;

public class Player : NetworkBehaviour
{
    private NetworkRunner _runner;
    public int idd;

    private void Start()
    {
        _runner=FindObjectOfType<NetworkRunner>();


        if(_runner.LocalPlayer.PlayerId==1)
        {
           

        }
    }
}