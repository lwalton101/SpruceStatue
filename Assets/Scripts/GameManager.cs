using RiptideNetworking;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

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

    public Animator waitingAnimator;
    public List<PlayerInfo> players = new List<PlayerInfo>();

    // Start is called before the first frame update
    void Awake()
    {
        Singleton = this;

        if(NetworkManager.Singleton.Server.IsRunning)
        {
            Debug.Log("I am server");
            NetworkManager.Singleton.SendScene();
        }

        NetworkManager.Singleton.players[NetworkManager.Singleton.Client.Id].gameReady = true;
        NetworkManager.Singleton.SendReady();

    }

    // Update is called once per frame
    void Update()
    {
        if (CheckIfReady())
        {
            Debug.Log("We ready to start");
            waitingAnimator.SetBool("open", false);
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
    

}
