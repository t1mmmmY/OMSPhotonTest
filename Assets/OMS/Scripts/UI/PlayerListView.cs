using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerListView : MonoBehaviour 
{
	[SerializeField] Text PlayerNameText;

	int ownerId;

	public void Init(int playerId, string playerName)
	{
		ownerId = playerId;
		PlayerNameText.text = playerName;
	}
}
