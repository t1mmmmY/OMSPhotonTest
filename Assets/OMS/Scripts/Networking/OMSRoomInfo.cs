using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OMS.Networking
{
	public class OMSRoomInfo
	{
		public string roomName;
		public string scenarioName;
		public int currentPlayers;
		public int maximumPlayers;
		public bool isVisible;

		public OMSRoomInfo(string roomName, string scenarioName, int currentPlayers, int maximumPlayers, bool isVisible)
		{
			this.roomName = roomName;
			this.scenarioName = scenarioName;
			this.currentPlayers = currentPlayers;
			this.maximumPlayers = maximumPlayers;
			this.isVisible = isVisible;
		}
	}
}