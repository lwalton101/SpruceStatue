using System;
using RiptideNetworking;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager _singleton;
    public static GameManager Singleton
    {
        get => _singleton;
        private set
        {
            if (_singleton == null)
                _singleton = value;
            else if (_singleton != value)
            {
                Debug.LogError($"{nameof(GameManager)} instance already exists, destroying object!");
                Destroy(value.gameObject);
            }
        }
    }
    [Header("Prefabs")]
    [SerializeField] private GameObject localPlayerPrefab;
    [SerializeField] private GameObject playerPrefab;
    
    public bool inGame;
    [SerializeField] private Camera mainCamera;
    
    
    public Animator waitingAnimator;

    // Start is called before the first frame update
    void Awake()
    {
        Singleton = this;

        if(NetworkManager.Singleton.Server.IsRunning)
        {
            Debug.Log("I am server");
            NetworkManager.Singleton.SendScene();
        }

        inGame = false;
        /*foreach (PlayerInfo singletonPlayer in NetworkManager.Singleton.players.Values)
        {
            singletonPlayer.gameReady = false;
        }*/
        
        NetworkManager.Singleton.players[NetworkManager.Singleton.Client.Id].gameReady = true;
        NetworkManager.Singleton.SendReady();

    }

    // Update is called once per frame
    void Update()
    {
        if (!inGame && CheckIfReady())
        {
            Debug.Log("We ready to start");
            SpawnPlayers();
            waitingAnimator.SetBool("open", false);
            inGame = true;
        }

        foreach (PlayerInfo singletonPlayer in NetworkManager.Singleton.players.Values)
        {
            Debug.Log($"Player with id {singletonPlayer.Id} and name {singletonPlayer.username} has the ready status {singletonPlayer.gameReady}");
        }
    }

    //Loops through players in NetworkManager and spawns prefab, then sets info like sprite and name
    private void SpawnPlayers()
    {
        foreach (PlayerInfo playerInfo in NetworkManager.Singleton.players.Values)
        {
            GameObject prefab;
            if (playerInfo.Id == NetworkManager.Singleton.Client.Id)
            {
                Debug.Log("Spawning Local Player");
                prefab = Instantiate(localPlayerPrefab, Vector3.zero, Quaternion.identity);
                mainCamera.GetComponent<CameraFollow>().playerTransform = prefab.transform;
            }
            else
            {
                Debug.Log("Spawning Server Players");
                prefab = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
            }

            prefab.GetComponentInChildren<TextMeshPro>().text = (playerInfo.isHost ? "<color=\"red\">" : "") + playerInfo.username;
            prefab.GetComponent<SpriteRenderer>().sprite = LobbyManager.playerSprites[playerInfo.spriteIndex];
            playerInfo.playerObject = prefab;
        }
    }

    private bool CheckIfReady()
    {
        foreach (PlayerInfo playerInfo in NetworkManager.Singleton.players.Values)
        {
            if (!playerInfo.gameReady)
            {
                return false;
            }
        }

        return true;
    }


    public void ClientDisconnect(ushort Id)
    {
        Destroy(NetworkManager.Singleton.players[Id].playerObject);
        NetworkManager.Singleton.players.Remove(Id);
    }

    //Only called on host
    public void StartLevel(SceneID sceneID)
    {
        if (NetworkManager.Singleton.Server.IsRunning)
            SceneManager.LoadScene((int)sceneID);
    }
}
