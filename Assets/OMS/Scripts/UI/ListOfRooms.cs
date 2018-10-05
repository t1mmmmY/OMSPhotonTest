using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OMS.Networking;

public class ListOfRooms : MonoBehaviour 
{
	[SerializeField] RoomListView roomInfoPrefab;
	[SerializeField] Transform content;

//	Dictionary<string, OMSRoomInfo> cachedRoomList = new Dictionary<string, OMSRoomInfo>();
	Dictionary<string, RoomListView> listOfRooms = new Dictionary<string, RoomListView>();

	void Awake()
	{
		NetworkLobbyHelper.onRoomListUpdate += OnRoomListUpdate;
	}

	void OnDestroy()
	{
		NetworkLobbyHelper.onRoomListUpdate -= OnRoomListUpdate;
	}

	void OnEnable()
	{
	}

	void OnRoomListUpdate(List<OMSRoomInfo> roomList)
	{
		Debug.Log("ListOfRooms " + roomList.Count.ToString());

		ClearList();

//		UpdateCachedRoomList(roomList);
		UpdateRoomListView(roomList);
	}

	void ClearList()
	{
		foreach (RoomListView room in listOfRooms.Values)
		{
			Destroy(room.gameObject);
		}

		listOfRooms = new Dictionary<string, RoomListView>();
	}

//	private void UpdateCachedRoomList(List<OMSRoomInfo> roomList)
//	{
//		foreach (OMSRoomInfo info in roomList)
//		{
//			// Remove room from cached room list if it got closed, became invisible or was marked as removed
//			if (!info.isVisible)
//			{
//				if (cachedRoomList.ContainsKey(info.roomName))
//				{
//					cachedRoomList.Remove(info.roomName);
//				}
//
//				continue;
//			}
//
//			// Update cached room info
//			if (cachedRoomList.ContainsKey(info.roomName))
//			{
//				cachedRoomList[info.roomName] = info;
//			}
//			// Add new room info to cache
//			else
//			{
//				cachedRoomList.Add(info.roomName, info);
//			}
//		}
//	}

//	private void UpdateRoomListView()
//	{
//		foreach (OMSRoomInfo info in cachedRoomList.Values)
//		{
//			CreateRoomItem(info);
//		}
//	}

	private void UpdateRoomListView(List<OMSRoomInfo> roomList)
	{
		foreach (OMSRoomInfo info in roomList)
		{
			CreateRoomItem(info);
		}
	}

	void CreateRoomItem(OMSRoomInfo roomInfo)
	{
		RoomListView newRoomItem = GameObject.Instantiate<RoomListView>(roomInfoPrefab, content);
		newRoomItem.Init(roomInfo);
		listOfRooms.Add(roomInfo.roomName, newRoomItem);
	}

}
