using UnityEngine;
using UnityEngine.SceneManagement;
using RiptideNetworking;
using RiptideNetworking.Utils;
using System;

public enum MessageID
{
    lobbyInfo = 1
}
public class NetworkManager : MonoBehaviour
{
    private static NetworkManager _singleton;
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

    //Sends local info to new client
	private void OnClientConnect(object sender, ClientConnectedEventArgs e)
	{
        Message message = Message.Create(MessageSendMode.reliable, (ushort)MessageID.lobbyInfo);
        message.AddUShort(e.Id);
        message.AddString(Username);
        message.AddUShort(Client.Id);
        message.AddUShort((ushort)LobbyManager.Singleton._playerSprites.IndexOf(LobbyManager.Singleton.currentSprite));
        message.AddBool(LobbyManager.Singleton.isReady);
        Client.Send(message);
    }

    [MessageHandler((ushort)MessageID.lobbyInfo)]
    public static void LobbyInfo(ushort fromClientID, Message messageRecieved)
	{
        ushort newPlayerId = messageRecieved.GetUShort();
        Message sendMessage = Message.Create(MessageSendMode.reliable, (ushort)MessageID.lobbyInfo);
        sendMessage.AddString(messageRecieved.GetString());
        sendMessage.AddUShort(messageRecieved.GetUShort());
        sendMessage.AddUShort(messageRecieved.GetUShort());
        sendMessage.AddBool(messageRecieved.GetBool());
        Singleton.Server.Send(sendMessage, newPlayerId);
    }

	private void OnClientDisconnect(object sender, ClientDisconnectedEventArgs e)
	{
        LobbyManager.Singleton.ClientDisconnect(e.Id);
	}

	private void OnConnectionFailed(object sender, EventArgs e)
	{
        UIManager.Singleton.BackToMain();
	}

	private void OnDisconnect(object sender, EventArgs e)
	{
        SceneManager.LoadScene(0);
    }

	private void OnConnect(object sender, EventArgs e)
	{
        SceneManager.LoadScene(1);
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

}
