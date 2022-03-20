using System;
using System.Collections;
using System.Net;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{

	private static UIManager _singleton;
	public static UIManager Singleton
	{
		get => _singleton;
		private set
		{
			if (_singleton == null)
				_singleton = value;
			else if (_singleton != value)
			{
				Debug.Log($"{nameof(UIManager)} instance already exists, destroying object!");
				Destroy(value);
			}
		}
	}

	[SerializeField] private GameObject mainPanel;
	[SerializeField] private GameObject singleplayerPanel;
	[SerializeField] private GameObject multiplayerPanel;
	[SerializeField] private GameObject settingsPanel;
	[SerializeField] private GameObject multiplayerFirstLayerPanel;
	[SerializeField] private GameObject hostPanel;
	[SerializeField] private GameObject joinPanel;
	[SerializeField] private GameObject hostOptionsPanel;
	[SerializeField] private GameObject saveSelectPanel;
	[SerializeField] private TMP_InputField hostPortInputField;
	[SerializeField] private TMP_InputField hostUsernameInputField;
	[SerializeField] private TMP_InputField joinUsernameInputField;
	[SerializeField] private TMP_InputField joinPortInputField;
	[SerializeField] private TMP_InputField hostIPField;
	public void HostClicked()
	{
		NetworkManager.Singleton.Username = hostUsernameInputField.text;
		ushort port;
		try
		{
			port = ushort.Parse(hostPortInputField.text);
		}
		catch (Exception e)
		{
			Debug.LogError("There is a problem with the port parsing.");
			Debug.LogError(e);
			return;
		}

		NetworkManager.Singleton.StartHost(port);

	}
	public void JoinClicked()
	{
		NetworkManager.Singleton.Username = joinUsernameInputField.text;
		ushort port;
		try
		{
			port = ushort.Parse(joinPortInputField.text);
		}
		catch(Exception e)
		{
			Debug.LogError("There was a problem with parsing the port");
			Debug.LogError(e);
			return;
		}

		string ipAddressText = hostIPField.text;
		if (string.IsNullOrEmpty(ipAddressText))
		{
			NetworkManager.Singleton.StartClient("127.0.0.1", port);
			return;
		}

		IPAddress[] numAddresses = Dns.GetHostEntry(ipAddressText).AddressList;
		if(numAddresses.Length == 0)
		{
			Debug.LogError("UH oH. Looks like DNS Lookup didn't work. Probably the ips error");
		}
		else
		{
			NetworkManager.Singleton.StartClient(numAddresses[0].ToString(), port);
			return;
		}

		Debug.LogError("Client was not started");
	}
	public void ToHostOptionsPanel()
	{
		saveSelectPanel.SetActive(false);
		hostOptionsPanel.SetActive(true);
	}
	public void ToHostPanelClicked()
	{
		multiplayerFirstLayerPanel.SetActive(false);
		hostPanel.SetActive(true);
		saveSelectPanel.SetActive(true);
		hostOptionsPanel.SetActive(false);
	}
	public void ToJoinPanelClicked()
	{
		multiplayerFirstLayerPanel.SetActive(false);
		joinPanel.SetActive(true);
	}
	public void BackToFirstLayer()
	{
		multiplayerFirstLayerPanel.SetActive(true);
		hostPanel.SetActive(false);
		joinPanel.SetActive(false);
	}
    public void SingleplayerClicked()
	{
		mainPanel.SetActive(false);
		singleplayerPanel.SetActive(true);
	}

	public void MultiplayerClicked()
	{
		mainPanel.SetActive(false);
		multiplayerPanel.SetActive(true);
	}

	public void SettingsClicked()
	{
		mainPanel.SetActive(false);
		settingsPanel.SetActive(true);
	}

	public void QuitClicked()
	{
		Application.Quit();
	}
	public void BackToMain()
	{
		mainPanel.SetActive(true);
		singleplayerPanel.SetActive(false);
		multiplayerPanel.SetActive(false);
		settingsPanel.SetActive(false);
	}
}
