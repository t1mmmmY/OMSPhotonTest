using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OMS.Networking;

public class ListOfPlayers : MonoBehaviour 
{
	[SerializeField] RoomListView roomInfoView;
	[SerializeField] PlayerListView playerListViewPrefab;
	[SerializeField] Transform content;
	[SerializeField] LobbyView lobbyView;

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
		bool allPlayersReady = true;

		foreach (PlayerInfo player in players)
		{
			PlayerListView playerView = GameObject.Instantiate<PlayerListView>(playerListViewPrefab, content);
            playerView.Init(this, player.number, player.name, player.isLocal, player.isReady);

			if (!player.isReady)
			{
				allPlayersReady = false;
			}

			allPlayers.Add(player.number, playerView);
		}

		roomInfoView.Init(NetworkLobbyHelper.GetCurrentRoomInfo());

		if (allPlayersReady)
		{
			//All players ready - start game!
			lobbyView.OnStartGameButtonClicked();
		}
	}

	void ClearList()
	{
		foreach (PlayerListView player in allPlayers.Values)
		{
			Destroy(player.gameObject);
		}

		allPlayers = new Dictionary<int, PlayerListView>();
	}

    public void PlayerReady(int playerNumber)
    {
        ExitGames.Client.Photon.Hashtable hash = new ExitGames.Client.Photon.Hashtable();
        hash.Add("isReady", true);
        PhotonNetwork.player.SetCustomProperties(hash);
    }

}
