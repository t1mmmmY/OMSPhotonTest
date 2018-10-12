using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkOmsEngine : FakeOmsEngine, IPunObservable 
{
    PhotonView photonView;
    ActiveObject[] allActiveObjects;

    protected override void Awake()
    {
        base.Awake();
        photonView = GetComponent<PhotonView>();
        allActiveObjects = GameObject.FindObjectsOfType<ActiveObject>();
    }

    protected override void StartPatientStateCoroutine()
    {
        if (PhotonNetwork.isMasterClient)
        {
            base.StartPatientStateCoroutine();
        }
    }

    public override bool DoAction(string actionKey, ActiveObject activeObject)
    {
        if (PhotonNetwork.isMasterClient)
        {
            bool actionSuccess = base.DoAction(actionKey, activeObject);
            if (actionSuccess)
            {
                photonView.RPC("DoActionRPC", PhotonTargets.Others, actionKey, activeObject.actionNumber);
            }

            return actionSuccess;
        }
        else
        {
            photonView.RPC("AskForActionRPC", PhotonTargets.MasterClient, actionKey, activeObject.actionNumber);
            return false;
        }
    }

    [PunRPC]
    void AskForActionRPC(string actionKey, int actionNumber)
    {
        //Only Master client is able to get this RPC
        DoAction(actionKey, FindActiveObject(actionNumber));
    }


    [PunRPC]
    void DoActionRPC(string actionKey, int actionNumber)
    {
        ActiveObject activeObject = FindActiveObject(actionNumber);
        if (activeObject != null)
        {
            activeObject.PerformAction();
        }
    }

    ActiveObject FindActiveObject(int actionNumber)
    {
        foreach (ActiveObject activeObject in allActiveObjects)
        {
            if (activeObject.actionNumber == actionNumber)
            {
                return activeObject;
            }
        }
        return null;
    }

    void OnMasterClientSwitched(PhotonPlayer newMasterClient)
    {
        Debug.Log("OnMasterClientSwitched " + newMasterClient.UserId);
        if (newMasterClient.UserId == PhotonNetwork.player.UserId)
        {
            //I become a master client!
            StartPatientStateCoroutine();
        }
    }

    #region IPunObservable implementation

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting && PhotonNetwork.isMasterClient)
        {
            //Master client writing
            stream.SendNext(patient.health);
        }
        else if (!stream.isWriting && !PhotonNetwork.isMasterClient)
        {
            //Other clients reading
            SetPatientHealth((int)stream.ReceiveNext());
        }
    }

    #endregion
}
