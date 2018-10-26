using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerListView : MonoBehaviour 
{
	[SerializeField] Text PlayerNameText;
    [SerializeField] Button readyButton;
    [SerializeField] GameObject waitingLabel;
    [SerializeField] GameObject readyLabel;

	int ownerId;
    ListOfPlayers owner;

    public void Init(ListOfPlayers owner, int playerId, string playerName, bool isLocal, bool isReady)
	{
        this.owner = owner;
		ownerId = playerId;
		PlayerNameText.text = playerName;

        readyLabel.SetActive(isReady);
        waitingLabel.SetActive(!isReady && !isLocal);
        readyButton.gameObject.SetActive(!isReady && isLocal);

//        if (isReady)
//        {
//            readyLabel.SetActive(true);
//            waitingLabel.SetActive(false);
//            readyButton.gameObject.SetActive(false);
//        }
//        else
//        {
//            if (isLocal)
//            {
//                
//            }
//            else
//            {
//            }
//        }
	}

    public void Ready()
    {
        readyButton.gameObject.SetActive(false);
        readyLabel.SetActive(true);

        owner.PlayerReady(ownerId);
    }
}
