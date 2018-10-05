using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using Photon.Pun;
//using Photon.Realtime;


namespace OMS.Networking
{
	public class NetworkLobby : Photon.PunBehaviour
	{
		public System.Action onConnectedToMaster;
		public System.Action onJoinedLobby;
		public System.Action onCreatedRoom;
		public System.Action onJoinedRoom;
		public System.Action onLeftRoom;
		public System.Action<RoomInfo[]> onRoomListUpdate;
		public System.Action<PhotonPlayer[]> onPlayersListUpdate;

		void Awake()
		{
			// #Critical
			// this makes sure we can use PhotonNetwork.LoadLevel() on the master client and all clients in the same room sync their level automatically
			PhotonNetwork.automaticallySyncScene = true;

		}

		void OnDestroy()
		{
		}

		public void Connect(string playerName)
		{
			PhotonNetwork.player.NickName = playerName;
			if (!PhotonNetwork.connected) 
			{
//				PhotonNetwork.AddCallbackTarget(this);
//				PhotonNetwork.gameVersion = "0.1";
				PhotonNetwork.ConnectUsingSettings("0.1");
			}
		}

		public void CreateRoom(string roomName, byte maxPlayers, string scenario)
		{
			RoomOptions roomOptions = new RoomOptions();
			roomOptions.MaxPlayers = maxPlayers;

			ExitGames.Client.Photon.Hashtable customRoomProperties = new ExitGames.Client.Photon.Hashtable();
			customRoomProperties.Add("scenario", scenario);
			roomOptions.CustomRoomProperties = customRoomProperties;

			//Expected users can be added here
			PhotonNetwork.CreateRoom(roomName, roomOptions, TypedLobby.Default);
		}

		public Room GetCurrentRoomInfo()
		{
			return PhotonNetwork.room;
		}

		public void JoinRoom(string roomName)
		{
			PhotonNetwork.JoinRoom(roomName);
		}

		public void StartGame(string sceneName)
		{
			PhotonNetwork.room.IsOpen = false;
			PhotonNetwork.room.IsVisible = false;

			PhotonNetwork.LoadLevel(sceneName);
		}

		public void LeaveRoom()
		{
			PhotonNetwork.LeaveRoom();
		}

		public override void OnConnectedToMaster()
		{
			if (onConnectedToMaster != null) 
			{
				onConnectedToMaster();
			}

			PhotonNetwork.JoinLobby();
		}
			

		#region ILobbyCallbacks implementation

		public override void OnJoinedLobby ()
		{
			if (onJoinedLobby != null) 
			{
				onJoinedLobby();
			}
		}

		public override void OnLeftLobby ()
		{

		}

		public override void OnReceivedRoomListUpdate()
		{
			RoomInfo[] roomList = PhotonNetwork.GetRoomList();
			if (onRoomListUpdate != null)
			{
				onRoomListUpdate(roomList);
			}
		}

//		public override void OnRoomListUpdate(List<PhotonRoomInfo> roomList)
//		{
//			if (onRoomListUpdate != null)
//			{
//				onRoomListUpdate(roomList);
//			}
//		}

		#endregion

		#region IMatchmakingCallbacks implementation

		public override void OnCreatedRoom ()
		{
			if (onCreatedRoom != null) 
			{
				onCreatedRoom();
			}
		}

		public override void OnPhotonCreateRoomFailed (object[] codeAndMsg)
		{
		}

		public override void OnJoinedRoom()
		{
			if (onJoinedRoom != null) 
			{
				onJoinedRoom();
			}
			if (onPlayersListUpdate != null) 
			{
				onPlayersListUpdate(PhotonNetwork.playerList);
			}

		}

		public override void OnPhotonJoinRoomFailed(object[] codeAndMsg)
		{
		}

		public override void OnLeftRoom ()
		{
			if (onLeftRoom != null) 
			{
				onLeftRoom();
			}

		}

		public override void OnPhotonPlayerConnected(PhotonPlayer newPlayer)
		{
			if (onPlayersListUpdate != null) 
			{
				onPlayersListUpdate(PhotonNetwork.playerList);
			}
		}

		public override void OnPhotonPlayerActivityChanged(PhotonPlayer otherPlayer)
		{
			if (onPlayersListUpdate != null) 
			{
				onPlayersListUpdate(PhotonNetwork.playerList);
			}
		}

//		public override void OnPlayerEnteredRoom(Player newPlayer)
//		{
//			if (onPlayersListUpdate != null) 
//			{
//				onPlayersListUpdate(PhotonNetwork.PlayerList);
//			}
//		}
//
//		public override void OnPlayerLeftRoom(Player otherPlayer)
//		{
//			if (onPlayersListUpdate != null) 
//			{
//				onPlayersListUpdate(PhotonNetwork.PlayerList);
//			}
//		}

		#endregion
	}

}