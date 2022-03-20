using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RiptideNetworking;
using TMPro;
using System;

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
	private void Start()
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
	}

	internal void ClientDisconnect(ushort id)
	{
		availableDisplays.Add(takenDisplays[id]);
		takenDisplays.Remove(id);
	}

	//Sends Name(string), ID(ushort), spriteIndex(ushort) and bool isReady
	public void SendLobbyInfo()
	{
		Message message = Message.Create(MessageSendMode.reliable, (ushort)MessageID.lobbyInfo, shouldAutoRelay: true);
		message.AddString(NetworkManager.Singleton.Username);
		message.AddUShort(NetworkManager.Singleton.Client.Id);
		message.AddUShort((ushort)_playerSprites.IndexOf(currentSprite));
		message.AddBool(isReady);
		NetworkManager.Singleton.Client.Send(message);

		mainPlayerDisplay.username.text = NetworkManager.Singleton.Username;
		mainPlayerDisplay.background.color = isReady ? readyColor : notReadyColor;
	}

	[MessageHandler((ushort)MessageID.lobbyInfo)]
	public static void LobbyInfo(Message message)
	{
		string username = (message.GetString());
		ushort id = message.GetUShort();
		ushort spriteIndex = message.GetUShort();
		bool isReady = message.GetBool();
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

			Debug.Log($"Added display to takenDisplays then remove available. Current count = {availableDisplays.Count}");
		}
		playerDisplay.image.gameObject.SetActive(true);
		playerDisplay.username.text = username;
		playerDisplay.image.sprite = playerSprites[spriteIndex];
		playerDisplay.background.color = isReady ? Singleton.readyColor : Singleton.notReadyColor;
		playerDisplay.ready = isReady;
	}

}
