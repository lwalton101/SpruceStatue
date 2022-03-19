using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RiptideNetworking;
using TMPro;
using System;

public class LobbyManager : MonoBehaviour
{
	[SerializeField] private List<Sprite> playerSprites = new List<Sprite>();
	[SerializeField] private Sprite currentSprite;
	[SerializeField] private Image playerImage;
	[SerializeField] private Button startReadyButton;
	[SerializeField] private TextMeshProUGUI startReadyText;

	[Header("Player Displays")]
	[SerializeField] private PlayerDisplay mainPlayerDisplay;
	[SerializeField] private PlayerDisplay playerDisplay1;
	[SerializeField] private PlayerDisplay playerDisplay2;
	[SerializeField] private PlayerDisplay playerDisplay3;
	[SerializeField] private PlayerDisplay playerDisplay4;
    public void LeftArrowClicked()
	{
		int nextIndex = playerSprites.IndexOf(currentSprite) - 1;
		if(nextIndex < 0)
		{
			playerImage.sprite = playerSprites[playerSprites.Count - 1];
		}
		else
		{
			playerImage.sprite = playerSprites[nextIndex];
		}

		currentSprite = playerImage.sprite;
	}
	public void RightArrowClicked()
	{
		int nextIndex = playerSprites.IndexOf(currentSprite) + 1;
		if (nextIndex > playerSprites.Count - 1)
		{
			playerImage.sprite = playerSprites[0];
		}
		else
		{
			playerImage.sprite = playerSprites[nextIndex];
		}

		currentSprite = playerImage.sprite;
	}
	private void Start()
	{
		currentSprite = playerImage.sprite;

		if (NetworkManager.Singleton.Server.IsRunning)
		{
			startReadyText.text = "HOST";
		}
		else
		{
			startReadyText.text = "Client";
		}

		SpawnPlayerObjects();
	}

	private void SpawnPlayerObjects()
	{
		throw new NotImplementedException();
	}

	public void SendLobbyInfo()
	{
		Message.Create();
	}
}
