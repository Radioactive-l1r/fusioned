using Fusion;
using UnityEngine;
enum MyButtons
{
    space= 0,
    
}
public struct NetworkInputData : INetworkInput
{
    public Vector3 direction;
    public NetworkButtons buttons;


}