using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using OMS.Networking;

public class RoomListView : MonoBehaviour 
{
	[SerializeField] Text roomNameLabel;
	[SerializeField] Text scenarioNameLabel;
	[SerializeField] Text currentNumberOfPlayersLabel;
	[SerializeField] Text maxNumberOfPlayersLabel;

	string _roomName = "";
	string _scenarioName = "";
	int _currentNumberOfPlayers = 0;
	int _maxNumberOfPlayers = 0;

	public void Init(OMS.Networking.OMSRoomInfo roomInfo)
	{
		Init (roomInfo.roomName, roomInfo.scenarioName, roomInfo.currentPlayers, roomInfo.maximumPlayers);
	}

	public void Init(string roomName, string scenarioName, int numberOfPlayers, int maxNumberOfPlayers)
	{
		_roomName = roomName;
		_scenarioName = scenarioName;
		_currentNumberOfPlayers = numberOfPlayers;
		_maxNumberOfPlayers = maxNumberOfPlayers;

		roomNameLabel.text = _roomName;
		scenarioNameLabel.text = _scenarioName;
		currentNumberOfPlayersLabel.text = _currentNumberOfPlayers.ToString();;
		maxNumberOfPlayersLabel.text = _maxNumberOfPlayers.ToString();
	}

	public void JoinRoom()
	{
		NetworkLobbyHelper.JoinRoom(_roomName);
	}

	void OnChangeNumberOfPlayers(int playersNumber)
	{
		_currentNumberOfPlayers = playersNumber;
		currentNumberOfPlayersLabel.text = _currentNumberOfPlayers.ToString();
	}
}
