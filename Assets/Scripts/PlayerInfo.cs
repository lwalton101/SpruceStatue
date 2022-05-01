using RiptideNetworking;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerInfo
{
    public ushort Id;
    public string username;
    public ushort spriteIndex;
    public bool isHost;
    public bool gameReady = false;
    public GameObject playerObject;
    
    public PlayerInfo(ushort Id, string username, ushort spriteIndex, bool isHost)
	{
        this.Id = Id;
        this.username = username;
        this.spriteIndex = spriteIndex;
        this.isHost = isHost;
	}
}
