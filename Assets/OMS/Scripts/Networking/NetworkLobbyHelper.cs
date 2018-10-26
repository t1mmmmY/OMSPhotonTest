using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OMS.Networking
{
	/// <summary>
	/// Facade for NetworkLobby
	/// </summary>
	[RequireComponent(typeof(NetworkLobby))]
	public class NetworkLobbyHelper : MonoBehaviour 
	{
		private static int defaultTimer = 30;

		private static NetworkLobby networkLobby;

		public static System.Action onConnected;
		public static System.Action onCreatedRoom;
		public static System.Action onJoinedRoom;
		public static System.Action onLeftRoom;
		public static System.Action<List<OMSRoomInfo>> onRoomListUpdate;
		public static System.Action<List<PlayerInfo>> onPlayersListUpdate;

		public static RoomInfo currentRoomInfo { get; private set; }

		void Awake()
		{
			networkLobby = GetComponent<NetworkLobby>();
			networkLobby.onConnectedToMaster += OnConnected;
			networkLobby.onCreatedRoom += OnCreatedRoom;
			networkLobby.onJoinedRoom += OnJoinedRoom;
			networkLobby.onLeftRoom += OnLeftRoom;
			networkLobby.onRoomListUpdate += OnRoomListUpdate;
			networkLobby.onPlayersListUpdate += OnPlayersListUpdate;
		}

		void OnDestroy()
		{
			networkLobby.onConnectedToMaster -= OnConnected;
			networkLobby.onCreatedRoom -= OnCreatedRoom;
			networkLobby.onJoinedRoom -= OnJoinedRoom;
			networkLobby.onLeftRoom -= OnLeftRoom;
			networkLobby.onRoomListUpdate -= OnRoomListUpdate;
			networkLobby.onPlayersListUpdate -= OnPlayersListUpdate;
		}

		public static void Connect(string name)
		{
			networkLobby.Connect(name);
		}

		public static void CreateRoom(string roomName, byte maxPlayers, string scenario)
		{
			networkLobby.CreateRoom(roomName, maxPlayers, scenario);
		}

		public static void JoinRoom(string roomName)
		{
			networkLobby.JoinRoom(roomName);
		}

		public static void StartGame(string sceneName)
		{
			PlayerPrefs.SetString("SceneName", sceneName);
			networkLobby.StartGame("GameMediator");
		}

		public static void LeaveRoom()
		{
			networkLobby.LeaveRoom();
		}

		public static int GetCurrentTimer()
		{
			if (PhotonNetwork.room.CustomProperties.ContainsKey("Timer")) 
			{
				return (int)PhotonNetwork.room.CustomProperties["Timer"];
			} 
			else 
			{
				//some default value
				return defaultTimer;
			}
		}

		public static void SetCurrentTimer(int time)
		{
			ExitGames.Client.Photon.Hashtable hash = new ExitGames.Client.Photon.Hashtable();
			hash.Add("Timer", time);
			PhotonNetwork.room.SetCustomProperties(hash);
		}

		public static OMSRoomInfo GetCurrentRoomInfo()
		{
			Room currentRoom = networkLobby.GetCurrentRoomInfo();
			return new OMSRoomInfo(currentRoom.Name, (string)currentRoom.CustomProperties["scenario"], 
				currentRoom.PlayerCount, currentRoom.MaxPlayers, true);
		}

		void OnConnected()
		{
			if (onConnected != null) 
			{
				onConnected();
			}
		}

		void OnCreatedRoom()
		{
			Debug.Log("OnCreatedRoom");

			if (PhotonNetwork.isMasterClient)
			{
				SetCurrentTimer(defaultTimer);
			}

			if (onCreatedRoom != null) 
			{
				onCreatedRoom();
			}
		}

		void OnJoinedRoom()
		{
			Debug.Log("OnJoinedRoom");

            ExitGames.Client.Photon.Hashtable hash = new ExitGames.Client.Photon.Hashtable();
            hash.Add("isReady", false);
            PhotonNetwork.player.SetCustomProperties(hash);

			if (onJoinedRoom != null) 
			{
				onJoinedRoom();
			}
		}

		void OnLeftRoom()
		{
			Debug.Log("OnLeftRoom");
			if (onLeftRoom != null) 
			{
				onLeftRoom();
			}
		}

		void OnRoomListUpdate(RoomInfo[] roomList)
		{
			List<OMSRoomInfo> rooms = new List<OMSRoomInfo>();
			for (int i = 0; i < roomList.Length; i++) 
			{
				bool isVisible = roomList[i].IsOpen && roomList[i].IsVisible && !roomList[i].removedFromList;
				rooms.Add(new OMSRoomInfo(roomList[i].Name, (string)roomList[i].CustomProperties["scenario"], 
					roomList[i].PlayerCount, roomList[i].MaxPlayers, isVisible));
			}

			Debug.Log("OnRoomListUpdate " + roomList.Length.ToString());

			if (onRoomListUpdate != null)
			{
				onRoomListUpdate(rooms);
			}
		}

		void OnPlayersListUpdate(PhotonPlayer[] players)
		{
			List<PlayerInfo> listOfPlayers = new List<PlayerInfo>();
			foreach (PhotonPlayer player in players) 
			{
                bool isReady = false;
                if (player.CustomProperties.ContainsKey("isReady"))
                {
                    isReady = (bool)player.CustomProperties["isReady"];
                }
                listOfPlayers.Add(new PlayerInfo(player.ID, player.NickName, player.IsLocal, isReady));
			}

			Debug.Log("OnPlayersListUpdate " + players.Length.ToString());

			if (onPlayersListUpdate != null) 
			{
				onPlayersListUpdate(listOfPlayers);
			}
		}

	}

}
