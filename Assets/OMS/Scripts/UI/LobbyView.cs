using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using OMS.Networking;

public class LobbyView : MonoBehaviour 
{
    [SerializeField] bool autoConnect = true;

	[SerializeField] GameObject loginPanel;
	[SerializeField] GameObject selectionPanel;
	[SerializeField] GameObject createRoomPanel;
	[SerializeField] GameObject roomListPanel;
	[SerializeField] GameObject insideRoomPanel;


	[SerializeField] InputField playerNameInput;
	[SerializeField] InputField createRoomNameInput;
	[SerializeField] Dropdown scenarioDropdown;
	[SerializeField] Slider numberOfPlayerSlider;

	[SerializeField] Button createButton;

	void Start()
	{
		if (PhotonNetwork.connected) 
		{
			SetActivePanel (selectionPanel.name);
			return;
		}

        playerNameInput.text =  "Player " + Random.Range(1000, 10000);

        //playerNameInput.text = PlayerPrefs.GetString("PlayerName", "Player " + Random.Range(1000, 10000));

        createButton.interactable = false;
		NetworkLobbyHelper.onConnected += OnConnected;
		NetworkLobbyHelper.onJoinedRoom += OnJoinedRoom;
		NetworkLobbyHelper.onPlayersListUpdate += OnPlayersListUpdate;

        if (autoConnect)
        {
            Login();
        }
	}

	void OnDestroy()
	{
		NetworkLobbyHelper.onConnected -= OnConnected;
		NetworkLobbyHelper.onJoinedRoom -= OnJoinedRoom;
		NetworkLobbyHelper.onPlayersListUpdate -= OnPlayersListUpdate;
	}

	void OnConnected()
	{
		this.SetActivePanel(selectionPanel.name);

		createButton.interactable = true;
	}

	public void Login()
	{
		string playerName = playerNameInput.text;
		if (playerName == "") 
		{
			playerName = "Player " + Random.Range(1000, 10000);
		}

		PlayerPrefs.SetString("PlayerName", playerName);

		NetworkLobbyHelper.Connect(playerName);
	}

	public void OnCreateRoomButtonClicked()
	{
		SetActivePanel(createRoomPanel.name);
	}

	public void OnBackButtonClicked()
	{
		SetActivePanel(selectionPanel.name);
	}

	public void OnStartGameButtonClicked()
	{
		if (PhotonNetwork.isMasterClient)
		{
			StartGame();
		}
	}

	public void OnLeaveGameButtonClicked()
	{
		NetworkLobbyHelper.LeaveRoom();
	}

	public void OnRoomListButtonClicked()
	{
		SetActivePanel(roomListPanel.name);
	}

	public void CreateRoom()
	{
		string roomName = createRoomNameInput.text;
		if (roomName == "") 
		{
			roomName = "Room " + Random.Range(1000, 10000);
		}
		NetworkLobbyHelper.CreateRoom(roomName, (byte)numberOfPlayerSlider.value, scenarioDropdown.captionText.text);
	}

	private void StartGame()
	{
		OMSRoomInfo roomInfo = NetworkLobbyHelper.GetCurrentRoomInfo();
		NetworkLobbyHelper.StartGame(roomInfo.scenarioName);
	}

	private void OnJoinedRoom()
	{
		SetActivePanel(insideRoomPanel.name);
	}

	private void OnPlayersListUpdate(List<PlayerInfo> players)
	{
		OMSRoomInfo roomInfo = NetworkLobbyHelper.GetCurrentRoomInfo();
		if (roomInfo.currentPlayers == roomInfo.maximumPlayers) 
		{
			//Start the game automatically when all players are ready
			StartGame();
		}
	}

	private void SetActivePanel(string activePanel)
	{
		loginPanel.SetActive(activePanel.Equals(loginPanel.name));
		selectionPanel.SetActive(activePanel.Equals(selectionPanel.name));
		createRoomPanel.SetActive(activePanel.Equals(createRoomPanel.name));
		roomListPanel.SetActive(activePanel.Equals(roomListPanel.name));    // UI should call OnRoomListButtonClicked() to activate this
		insideRoomPanel.SetActive(activePanel.Equals(insideRoomPanel.name));
	}
}
