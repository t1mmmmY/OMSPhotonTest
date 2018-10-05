using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OMS.Networking;

public class ListOfPlayers : MonoBehaviour 
{
	[SerializeField] RoomListView roomInfoView;
	[SerializeField] PlayerListView playerListViewPrefab;
	[SerializeField] Transform content;

	private Dictionary<int, PlayerListView> allPlayers = new Dictionary<int, PlayerListView>();


	void Awake()
	{
		NetworkLobbyHelper.onJoinedRoom += OnJoinedRoom;
		NetworkLobbyHelper.onLeftRoom += OnLeftRoom;
		NetworkLobbyHelper.onPlayersListUpdate += OnPlayersListUpdate;
	}

	void OnDestroy()
	{
		NetworkLobbyHelper.onJoinedRoom -= OnJoinedRoom;
		NetworkLobbyHelper.onLeftRoom -= OnLeftRoom;
		NetworkLobbyHelper.onPlayersListUpdate -= OnPlayersListUpdate;
	}

	void OnJoinedRoom()
	{
//		allPlayers = new Dictionary<int, PlayerListView>();
	}

	void OnLeftRoom()
	{
		ClearList();
	}

	void OnPlayersListUpdate(List<PlayerInfo> players)
	{
		Debug.Log("ListOfPlayers " + players.Count.ToString());
		ClearList();

		foreach (PlayerInfo player in players)
		{
			PlayerListView playerView = GameObject.Instantiate<PlayerListView>(playerListViewPrefab, content);
			playerView.Init(player.number, player.name);

			allPlayers.Add(player.number, playerView);
		}

		roomInfoView.Init(NetworkLobbyHelper.GetCurrentRoomInfo());
	}

	void ClearList()
	{
		foreach (PlayerListView player in allPlayers.Values)
		{
			Destroy(player.gameObject);
		}

		allPlayers = new Dictionary<int, PlayerListView>();
	}
}
