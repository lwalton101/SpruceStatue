using UnityEngine;
using UnityEngine.SceneManagement;
using RiptideNetworking;
using RiptideNetworking.Utils;
using System;

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

    [SerializeField] private ushort maxClientCount;

    private void Start()
    {
        RiptideLogger.Initialize(Debug.Log, Debug.Log, Debug.LogWarning, Debug.LogError, false);

        Server = new Server { AllowAutoMessageRelay = true };

        Client = new Client();
        Client.Connected += OnClientConnect;

        DontDestroyOnLoad(this);
    }

	private void OnClientConnect(object sender, EventArgs e)
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
}
