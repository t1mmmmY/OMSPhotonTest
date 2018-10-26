using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkPlayer : OVRPlayerController
{
    NetworkPlayerController networkPlayerController;
    PhotonView photonView;
    

//    public override Transform transform
//    {
//        get
//        {
//            if (_transform == null) 
//            {
//                _transform = this.gameObject.transform;
//            }
//            return _transform;
//        }
//        set
//        {
//            _transform = value;
//        }
//    }

    public void Init(NetworkPlayerController owner)
    {
        //Disable old controller
        CharacterController oldController = GetComponent<CharacterController>();
        if (oldController != null)
        {
            oldController.enabled = false;
        }

        photonView = owner.photonView;

        networkPlayerController = owner;
        Controller = networkPlayerController.characterController;
//        _transform = networkPlayerController.transform;

//        CameraRig.verticalOffset = networkPlayerController.verticalOffset;

    }

    public void DisableCamera()
    {
        CameraRig.gameObject.SetActive(false);
    }

    public override void UpdateMovement()
    {
        if (photonView != null)
        {
            if (!photonView.isMine)
            {
                return;
            }
        }
        base.UpdateMovement();
    }

    protected override void UpdateController()
    {
        if (photonView != null)
        {
            if (!photonView.isMine)
            {
                return;
            }
        }

        base.UpdateController();
    }

}
