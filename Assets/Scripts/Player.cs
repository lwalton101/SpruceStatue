using RiptideNetworking;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Dictionary<ushort, Player> List = new Dictionary<ushort, Player>();
	

    public ushort Id;
    private string username;

	//[MessageHandler((ushort)MessageID.lobbyInfo)]
	//private static void LobbyInfo(ushort fromClientId, Message message)
	//{
	//	SpawnLobby(fromClientId, message.GetString());
	//}
	//private static void SpawnLobby(ushort fromClientId, string username)
	//{
	//	GameObject playerObject = new GameObject($"Player {fromClientId}: {username}");
	//	Player player = playerObject.AddComponent<Player>();
	//	player.Id = fromClientId;
	//	player.username = username;

	//	List.Add(fromClientId, player);
	//}
}
