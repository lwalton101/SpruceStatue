using System;
using System.Collections;
using System.Net;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
	[SerializeField] private GameObject mainPanel;
	[SerializeField] private GameObject singleplayerPanel;
	[SerializeField] private GameObject multiplayerPanel;
	[SerializeField] private GameObject settingsPanel;
	[SerializeField] private GameObject multiplayerFirstLayerPanel;
	[SerializeField] private GameObject hostPanel;
	[SerializeField] private GameObject joinPanel;
	[SerializeField] private GameObject hostOptionsPanel;
	[SerializeField] private GameObject saveSelectPanel;
	[SerializeField] private TMP_InputField portInputField;
	[SerializeField] private TMP_InputField hostIPField;
	public void HostClicked()
	{
		try
		{
			ushort port = ushort.Parse(portInputField.text);
			NetworkManager.Singleton.StartHost(port);
		}
		catch (Exception e)
		{
			Debug.LogError("There is a problem with the port parsing.");
		}
	}
	public void JoinClicked()
	{
		try
		{
			ushort port = ushort.Parse(portInputField.text);
			if (string.IsNullOrEmpty(hostIPField.text))
			{
				//HAHA Switch scenes
				//connectPanel.SetActive(false);
				//loadingPanel.SetActive(true);
				NetworkManager.Singleton.StartClient("127.0.0.1", port);
				return;
			}
			//connectPanel.SetActive(false);
			//loadingPanel.SetActive(true);

			IPHostEntry entry = Dns.GetHostEntry(hostIPField.text);
			if (entry.AddressList.Length == 0)
			{
				//problem with connection
			}
			else
			{
				NetworkManager.Singleton.StartClient(entry.AddressList[0].ToString(), port);
			}
		}
		catch
		{
			
		}

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
