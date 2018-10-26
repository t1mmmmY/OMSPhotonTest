using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using OMS.Networking;

public class LobbyTimer : MonoBehaviour 
{
	[SerializeField] LobbyView lobbyView;
	[SerializeField] Text timerLabel;
	int timeLeft = 30;

	void OnEnable()
	{
		StartCoroutine("TimerTick");

//		if (PhotonNetwork.isMasterClient)
//		{
//			StartCoroutine("TimerTick");
//		}
//		else
//		{
//			GetCurrentTimer();
//			SetLabel();
//		}
	}

	void GetCurrentTimer()
	{
		timeLeft = NetworkLobbyHelper.GetCurrentTimer();
	}

	void SetLabel()
	{
		timerLabel.text = timeLeft.ToString();
	}

	IEnumerator TimerTick()
	{
		GetCurrentTimer();
		SetLabel();
		do
		{
			yield return new WaitForSeconds(1);
			if (PhotonNetwork.isMasterClient)
			{
				timeLeft--;
				NetworkLobbyHelper.SetCurrentTimer(timeLeft);
			}
			else
			{
				GetCurrentTimer();
			}
			SetLabel();

		} while (timeLeft > 0);

		lobbyView.OnStartGameButtonClicked();
	}
}
