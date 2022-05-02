using UnityEngine;
using UnityEngine.SceneManagement;
using RiptideNetworking;
using RiptideNetworking.Utils;
using System;
using System.Collections.Generic;

public enum MessageID
{
    lobbyInfo = 1,
    loadScene,
    ready,
    playerPosition
}
public class NetworkManager : MonoBehaviour
{
    private static NetworkManager _singleton;
    public Dictionary<ushort, PlayerInfo> players = new Dictionary<ushort, PlayerInfo>();
    
    public static NetworkManager Singleton
    {
        get => _singleton;
        private set
        {
            if (_singleton == null)
                _singleton = value;
            else if (_singleton != value)
            {
                Debug.LogError($"{nameof(NetworkManager)} instance already exists, destroying object!");
                Destroy(value.gameObject);
            }
        }
    }

	private void Awake()
    {
        Singleton = this;
    }

    public Server Server { get; private set; }
    public Client Client { get; private set; }

    [SerializeField] private ushort maxClientCount = 5;

    public string Username { get; set; }
    private void Start()
    {
        RiptideLogger.Initialize(Debug.Log, Debug.Log, Debug.LogWarning, Debug.LogError, false);

        Server = new Server { AllowAutoMessageRelay = true };

        Client = new Client();
        Client.Connected += OnConnect;
        Client.Disconnected += OnDisconnect;
        Client.ConnectionFailed += OnConnectionFailed;
        Client.ClientDisconnected += OnClientDisconnect;
        Client.ClientConnected += OnClientConnect;

        DontDestroyOnLoad(this);
    }

    public void SendScene()
	{
        Message message = Message.Create(MessageSendMode.reliable, (ushort)MessageID.loadScene, shouldAutoRelay: true);
        message.AddUShort((ushort)SceneManager.GetActiveScene().buildIndex);
        Client.Send(message);
    }
    public void SendReady()
	{
        Debug.Log("Sending Ready Message from " + Singleton.Client.Id);
        Message message = Message.Create(MessageSendMode.reliable, (ushort)MessageID.ready, shouldAutoRelay: true);
        message.AddUShort(Client.Id);
        Client.Send(message);
    }
	private void FixedUpdate()
    {
        if (Server.IsRunning)
            Server.Tick();

        Client.Tick();
    }

	public void StartHost(ushort port)
    {
        Server.Start(port, maxClientCount);
        Client.Connect($"127.0.0.1:{port}");
    }

    public void StartClient(string ipAddress, ushort port)
    {
        Client.Connect($"{ipAddress}:{port}");
        //Debug.Log($"{ipAddress}:{port}");
    }
    private void OnApplicationQuit()
    {
        Server.Stop();
        Client.Disconnect();
    }

    #region Handlers
    
    [MessageHandler((ushort)MessageID.loadScene)]
    private static void HandleLoadScene(Message messageRecieved)
    {
        SceneID sceneID = (SceneID) messageRecieved.GetUShort();
        SceneManager.LoadScene((int) sceneID);
    }

    [MessageHandler((ushort) MessageID.ready)]
    private static void HandleReady(Message messageRecieved)
    {
        ushort Id = messageRecieved.GetUShort();
        Debug.Log("Recieved Ready Packet from " + Singleton.players[Id].username);
        Singleton.players[Id].gameReady = true;
    }

    [MessageHandler((ushort) MessageID.playerPosition)]
    private static void HandlePlayerPositon(Message messageRecieved)
    {
        ushort Id = messageRecieved.GetUShort();
        Vector2 pos = messageRecieved.GetVector2();
        Debug.Log($"{Singleton.players[Id].username} is at {pos.x}x {pos.y}y");
        Singleton.players[Id].playerObject.transform.position = pos;
    }
    #endregion

    #region Client Events
    private void OnDisconnect(object sender, EventArgs e)
	{
        SceneManager.LoadScene((int)SceneID.MainMenu);
        players.Clear();
        LobbyManager.Singleton.playerDisplays.Clear();
    }

	private void OnConnect(object sender, EventArgs e)
	{
        SceneManager.LoadScene((int)SceneID.LobbyScene);
	}
    private void OnClientDisconnect(object sender, ClientDisconnectedEventArgs e)
    {
        if (SceneManager.GetActiveScene().buildIndex == (int)SceneID.LobbyScene)
            LobbyManager.Singleton.ClientDisconnect(e.Id);
        else if (GameManager.Singleton.inGame)
            GameManager.Singleton.ClientDisconnect(e.Id);

    }

    private void OnConnectionFailed(object sender, EventArgs e)
    {
        UIManager.Singleton.BackToMain();
    }
    
    private void OnClientConnect(object sender, ClientConnectedEventArgs e)
    {
        //Sends local info to new client
        Message message = Message.Create(MessageSendMode.reliable, (ushort)MessageID.lobbyInfo);
        message.AddUShort(e.Id);
        message.AddString(Username);
        message.AddUShort(Client.Id);
        message.AddUShort((ushort)LobbyManager.Singleton._playerSprites.IndexOf(LobbyManager.Singleton.currentSprite));
        message.AddBool(LobbyManager.Singleton.isReady);
        message.AddBool(Singleton.Server.IsRunning);
        Client.Send(message);
    }
    #endregion

    #region Server Events
    #endregion

    public PlayerInfo GetPlayerInfo()
    {
        return players[Client.Id];
    }
}
