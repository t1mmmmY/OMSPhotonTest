using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OMS.Networking;

public class NetworkPlayerController : Photon.MonoBehaviour, IPunObservable 
{
    [SerializeField] NetworkPlayer networkPlayerPrefab;

    public CharacterController characterController;
    public float verticalOffset = 1.7f;

    NetworkPlayer player;
    GameObject localAvatar;

    public int avatarViewId;

    public void Init()
    {
        CreatePlayer();

        if (photonView.isMine)
        {
            CreateLocalAvatar();
        }
        else
        {
            player.DisableCamera();
        }

    }

    public void RestoreRemotePlayer()
    {
        if (player == null && !photonView.isMine)
        {
            CreatePlayer();
            player.DisableCamera();
        }

        if (localAvatar == null && !photonView.isMine)
        {
            CreateAvatar("RemoteAvatar", avatarViewId);
        }
    }

    void CreatePlayer()
    {
        player = GameObject.Instantiate<NetworkPlayer>(networkPlayerPrefab, this.transform);
        if (player != null)
        {
            player.Init(this);
        }
    }

    void CreateLocalAvatar()
    {
        avatarViewId = PhotonNetwork.AllocateViewID();
        CreateAvatar("LocalAvatar", avatarViewId);

        photonView.RPC("OnCreateAvatarRPC", PhotonTargets.Others, avatarViewId);
    }

    [PunRPC]
    void OnCreateAvatarRPC(int viewId)
    {
        CreateAvatar("RemoteAvatar", viewId);
    }

    void CreateAvatar(string avatarName, int viewId)
    {
        localAvatar = Instantiate(Resources.Load(avatarName), this.transform) as GameObject;

        PhotonView pView = localAvatar.GetComponent<PhotonView>();
        if (pView != null)
        {
            pView.viewID = viewId;
        }
    }

    void Update()
    {
        if (photonView != null)
        {
            if (!photonView.isMine)
            {
                if (player == null)
                {
                    CreatePlayer();
                    player.DisableCamera();
                }
                return;
            }
        }

    }

    #region IPunObservable implementation

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(avatarViewId);
        }
        else
        {
            avatarViewId = (int)stream.ReceiveNext();
        }
    }

    #endregion
}
