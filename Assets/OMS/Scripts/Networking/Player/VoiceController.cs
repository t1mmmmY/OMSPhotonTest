using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OMS.Networking;

public class VoiceController : MonoBehaviour
{
    PhotonView photonView;
    PhotonVoiceRecorder voiceRecorder;
    NetworkGameManager networkGameManager;

    private void Awake()
    {
        photonView = GetComponent<PhotonView>();
        voiceRecorder = GetComponent<PhotonVoiceRecorder>();
    }

    public void Init(NetworkGameManager gameManager)
    {
        networkGameManager = gameManager;
    }

    void Update()
    {
        if (photonView != null)
        {
            if (!photonView.isMine)
            {
                return;
            }
        }

        if (voiceRecorder != null)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                voiceRecorder.Transmit = !voiceRecorder.Transmit;
                networkGameManager.Mute(!voiceRecorder.Transmit);
            }
        }
    }

}
