using System.Collections;
using System.Collections.Generic;
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
	public void ToHostOptionsPanel()
	{
		saveSelectPanel.SetActive(false);
		hostOptionsPanel.SetActive(true);
	}
	public void ToHostPanelClicked()
	{
		multiplayerFirstLayerPanel.SetActive(false);
		hostPanel.SetActive(true);
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
