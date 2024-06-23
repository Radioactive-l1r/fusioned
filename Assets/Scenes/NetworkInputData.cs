using Fusion;
using UnityEngine;
enum MyButtons
{
    space= 0,
    jump=1,
    
}
public struct NetworkInputData : INetworkInput
{
    public Vector3 direction;
    public NetworkButtons buttons;
    
    public float mouseX;
    public float mouseY;


}