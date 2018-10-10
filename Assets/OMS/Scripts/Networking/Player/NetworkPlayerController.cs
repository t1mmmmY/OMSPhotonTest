using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OMS.Networking;

public class NetworkPlayerController : Photon.MonoBehaviour, IPunObservable 
{
    [SerializeField] NetworkPlayer networkPlayerPrefab;

    public CharacterController characterController;
    public float verticalOffset = 1.7f;

    NetworkGameManager gameManager;
    NetworkPlayer player;
    //private PhotonVoiceRecorder voiceRecorder;

    //void Awake()
    //{
    //    voiceRecorder = GetComponent<PhotonVoiceRecorder>();

    //    player = GameObject.Instantiate<NetworkPlayer>(networkPlayerPrefab, this.transform);

    //    if (player != null)
    //    {
    //        player.Init(this);
    //    }
    //}

    public void Init(NetworkGameManager gameManager, bool isLocalPlayer)
    {
        this.gameManager = gameManager;

        //voiceRecorder = GetComponent<PhotonVoiceRecorder>();

        player = GameObject.Instantiate<NetworkPlayer>(networkPlayerPrefab, this.transform);

        if (player != null)
        {
            player.Init(this);
        }

        if (!isLocalPlayer)
        {
            player.DisableCamera();
        }
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

        //if (voiceRecorder != null)
        //{
        //    if (Input.GetKeyDown(KeyCode.Space))
        //    {
        //        voiceRecorder.Transmit = !voiceRecorder.Transmit;
        //        gameManager.Mute(!voiceRecorder.Transmit);
        //    }
        //}
    }

    #region IPunObservable implementation

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
    }

    #endregion
}
