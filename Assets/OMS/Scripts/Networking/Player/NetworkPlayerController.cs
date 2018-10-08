using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkPlayerController : Photon.MonoBehaviour, IPunObservable 
{
    [SerializeField] NetworkPlayer networkPlayerPrefab;

    public CharacterController characterController;
    public float verticalOffset = 1.7f;
   
    NetworkPlayer player;

    void Awake()
    {
        player = GameObject.Instantiate<NetworkPlayer>(networkPlayerPrefab, this.transform);

        if (player != null)
        {
            player.Init(this);
        }
    }

    #region IPunObservable implementation

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
    }

    #endregion
}
