using Fusion;
using Fusion.Sockets;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Unity.VisualScripting;

public class FusionManager : MonoBehaviour, INetworkRunnerCallbacks
{
    public static FusionManager instance;

    private NetworkRunner _runner;
    [SerializeField] private NetworkPrefabRef _playerPrefab;
    private Dictionary<PlayerRef, NetworkObject> _spawnedCharacters = new Dictionary<PlayerRef, NetworkObject>();
    //------
    public TMP_Text TXtype;
    public TMP_Text TXname;
    public TMP_Text TX_room_name;

    public string _name;
    public string _room_name;
    public string _type;
    //------
    public GameObject loadingUi;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

    }
    void Start()
    {
        _name = PlayerPrefs.GetString("name");
        _type = PlayerPrefs.GetString("type");
        _room_name = PlayerPrefs.GetString("room_name");

        TXtype.SetText(_type);
        TXname.SetText(_name);
        TX_room_name.SetText(_room_name);
        if (_type.Contains("HOST"))
        {
            StartGame(GameMode.Host, _name);
        }
        else
        {
            StartGame(GameMode.Client, _name);
        }
        loadingUi.SetActive(true);
    }
    async void StartGame(GameMode mode, string PlayerName)
    {
        // Create the Fusion runner and let it know that we will be providing user input
        _runner = gameObject.AddComponent<NetworkRunner>();
        _runner.ProvideInput = true;
     

        // Create the NetworkSceneInfo from the current scene
        var scene = SceneRef.FromIndex(SceneManager.GetActiveScene().buildIndex);
        var sceneInfo = new NetworkSceneInfo();
        if (scene.IsValid)
        {
            sceneInfo.AddSceneRef(scene, LoadSceneMode.Additive);
        }

        // Start or join (depends on gamemode) a session with a specific name
        await _runner.StartGame(new StartGameArgs()
        {
            GameMode = mode,
            SessionName = "gemhunter",
            Scene = scene,
            SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
        });
    }

    public void OnConnectedToServer(NetworkRunner runner)
    {
    }

    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
    {
    }

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
    {
    }

    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
    {
    }

    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason)
    {
    }

    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
    {
    }

    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        var data = new NetworkInputData();
        data.buttons.Set(MyButtons.space, Input.GetKeyDown(KeyCode.Space));
        data.buttons.Set(MyButtons.jump, Input.GetKey(KeyCode.LeftShift));
        data.mouseX = Input.GetAxis("Mouse X");
        data.mouseY = Input.GetAxis("Mouse Y");
        data.direction = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        input.Set(data);
    }

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
    {
    }

    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
    }

    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
    }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {

        loadingUi.SetActive(false);

        if (runner.IsServer)
        {
            // Create a unique position for the player
            // Vector3 spawnPosition = new Vector3(-50, -10, player.PlayerId+2);
            //            Vector3 spawnPosition = new Vector3(player.PlayerId, 2, 0);

            Vector3 spawnPosition = new Vector3(5, 5, (player.RawEncoded % runner.Config.Simulation.PlayerCount) * 3);
            
            NetworkObject networkPlayerObject = runner.Spawn(_playerPrefab, spawnPosition, Quaternion.identity, player);
            networkPlayerObject.GetComponent<CharacterController>().enabled = true;
            // Keep track of the player avatars for easy access
            _spawnedCharacters.Add(player, networkPlayerObject);


        }
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {

        if (_spawnedCharacters.TryGetValue(player, out NetworkObject networkObject))
        {
            runner.Despawn(networkObject);
            _spawnedCharacters.Remove(player);
        }
    }

    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress)
    {
    }

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data)
    {
    }

    public void OnSceneLoadDone(NetworkRunner runner)
    {
    }

    public void OnSceneLoadStart(NetworkRunner runner)
    {
    }

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {
    }

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {
    }

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
    {
    }
}
