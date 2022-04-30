using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RiptideNetworking;
using TMPro;
using System;
using UnityEngine.SceneManagement;

public class LobbyManager : MonoBehaviour
{
	private static LobbyManager _singleton;
	public static LobbyManager Singleton
	{
		get => _singleton;
		private set
		{
			if (_singleton == null)
				_singleton = value;
			else if (_singleton != value)
			{
				Debug.Log($"{nameof(LobbyManager)} instance already exists, destroying object!");
				Destroy(value);
			}
		}
	}

	[SerializeField] public bool isReady = false;
	public static List<Sprite> playerSprites = new List<Sprite>();
	public List<Sprite> _playerSprites = new List<Sprite>();
	[SerializeField] public Sprite currentSprite;
	[SerializeField] private Image playerImage;
	[SerializeField] private Button startReadyButton;
	[SerializeField] private TextMeshProUGUI startReadyText;
	[SerializeField] private Color notReadyColor;
	[SerializeField] private Color readyColor;

	[Header("Player Displays")]
	public List<PlayerDisplay> playerDisplays = new List<PlayerDisplay>();
	private static List<PlayerDisplay> availableDisplays = new List<PlayerDisplay>();
	private static Dictionary<ushort, PlayerDisplay> takenDisplays = new Dictionary<ushort, PlayerDisplay>();
	[SerializeField] private PlayerDisplay mainPlayerDisplay;

	/*
	 * Sets currentSprite, Sends lobby info and changes buttons according to server or host
	 */
	private void Awake()
	{
		mainPlayerDisplay.background.color = notReadyColor;
		currentSprite = playerImage.sprite;

		if (NetworkManager.Singleton.Server.IsRunning)
		{
			startReadyText.text = "Start";
			startReadyButton.onClick.AddListener(StartClicked);
			isReady = true;
		}
		else
		{
			startReadyText.text = "Ready";
			startReadyButton.onClick.AddListener(ReadyClicked);
		}

		SendLobbyInfo();
		availableDisplays = playerDisplays;
		playerSprites = _playerSprites;

		PlayerInfo playerInfo = new PlayerInfo(NetworkManager.Singleton.Client.Id, NetworkManager.Singleton.Username, (ushort)_playerSprites.IndexOf(currentSprite), NetworkManager.Singleton.Server.IsRunning);
		NetworkManager.Singleton.players.Add(NetworkManager.Singleton.Client.Id,playerInfo);

		Singleton = this;
	}

	public void LeftArrowClicked()
	{
		int nextIndex = _playerSprites.IndexOf(currentSprite) - 1;
		Debug.Log(nextIndex);
		if (nextIndex < 0)
		{
			playerImage.sprite = _playerSprites[_playerSprites.Count - 1];
		}
		else
		{
			playerImage.sprite = _playerSprites[nextIndex];
		}

		currentSprite = playerImage.sprite;

		SendLobbyInfo();
	}
	public void RightArrowClicked()
	{
		int nextIndex = _playerSprites.IndexOf(currentSprite) + 1;
		if (nextIndex > _playerSprites.Count - 1)
		{
			playerImage.sprite = _playerSprites[0];
		}
		else
		{
			playerImage.sprite = _playerSprites[nextIndex];
		}

		currentSprite = playerImage.sprite;

		SendLobbyInfo();
	}

	public void ReadyClicked()
	{
		isReady = !isReady;
		SendLobbyInfo();
	}
	public void StartClicked()
	{
		
		foreach(PlayerDisplay playerDisplay in takenDisplays.Values)
		{
			if (!playerDisplay.ready)
			{
				return;
			}
		}

		print("Everyone one is ready. Time to make a game");
		SceneManager.LoadScene((int)SceneID.levelSelect);
	}

	//Frees up PlayerDisplay in list
	internal void ClientDisconnect(ushort id)
	{
		availableDisplays.Add(takenDisplays[id]);
		takenDisplays.Remove(id);
	}

	//Sends Name(string), ID(ushort), spriteIndex(ushort) and bool isReady and bool isHost. Also
	public void SendLobbyInfo()
	{
		Message message = Message.Create(MessageSendMode.reliable, (ushort)MessageID.lobbyInfo, shouldAutoRelay: true);
		message.AddString(NetworkManager.Singleton.Username);
		message.AddUShort(NetworkManager.Singleton.Client.Id);
		message.AddUShort((ushort)_playerSprites.IndexOf(currentSprite));
		message.AddBool(isReady);
		message.AddBool(NetworkManager.Singleton.Server.IsRunning);
		NetworkManager.Singleton.Client.Send(message);

		mainPlayerDisplay.username.text = NetworkManager.Singleton.Username + (NetworkManager.Singleton.Server.IsRunning ? "<color=\"red\"> Host" : "");
		mainPlayerDisplay.background.color = isReady ? readyColor : notReadyColor;
	}

	//Reads lobbyInfo packet and adjusts lobby accordingly
	//lobbyInfo is sent from new client to old client here
	[MessageHandler((ushort)MessageID.lobbyInfo)]
	public static void LobbyInfo(Message message)
	{
		string username = (message.GetString());
		ushort id = message.GetUShort();
		ushort spriteIndex = message.GetUShort();
		bool isReady = message.GetBool();
		bool isHost = message.GetBool();
		PlayerDisplay playerDisplay;

		if(takenDisplays.TryGetValue(id, out playerDisplay))
		{
			Debug.Log($"Player called {username} with ID {id} was found in takenDisplays");
			
		}
		else
		{
			Debug.Log($"Player called {username} with ID {id} was not found in takenDisplays");

			playerDisplay = availableDisplays[0];
			takenDisplays.Add(id, playerDisplay);
			availableDisplays.Remove(playerDisplay);

			PlayerInfo playerInfo = new PlayerInfo(id, username, spriteIndex, isHost);
			NetworkManager.Singleton.players.Add(id, playerInfo);

			Debug.Log($"Added display to takenDisplays then remove available. Current count = {availableDisplays.Count}");
		}
		playerDisplay.image.gameObject.SetActive(true);
		playerDisplay.username.text = username + (isHost ? "<color=\"red\"> Host" : "");
		playerDisplay.image.sprite = playerSprites[spriteIndex];
		playerDisplay.background.color = isReady ? Singleton.readyColor : Singleton.notReadyColor;
		playerDisplay.ready = isReady;

		
	}

	//When server recieves message it forwards it to new client
	[MessageHandler((ushort)MessageID.lobbyInfo)]
	public static void LobbyInfo(ushort fromClientID, Message messageRecieved)
	{
		ushort newPlayerId = messageRecieved.GetUShort();
		Message sendMessage = Message.Create(MessageSendMode.reliable, (ushort)MessageID.lobbyInfo);
		sendMessage.AddString(messageRecieved.GetString());
		sendMessage.AddUShort(messageRecieved.GetUShort());
		sendMessage.AddUShort(messageRecieved.GetUShort());
		sendMessage.AddBool(messageRecieved.GetBool());
		sendMessage.AddBool(messageRecieved.GetBool());
		NetworkManager.Singleton.Server.Send(sendMessage, newPlayerId);
	}



}
