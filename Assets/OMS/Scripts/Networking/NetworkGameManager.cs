using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using Photon.Realtime;
//using Photon.Pun.UtilityScripts;
//using Photon.Pun;

namespace OMS.Networking
{

	public class NetworkGameManager : Photon.MonoBehaviour, IPunObservable
	{
        [SerializeField] NetworkPlayerController networkPlayerControllerPrefab;
        [SerializeField] OMSSceneManager sceneManager;
		[SerializeField] Color[] colorPresets;
		[SerializeField] GameObject soundOffImage;

		ScenarioScene currentScene;
        NetworkPlayerController localPlayerController = null;

        public readonly byte InstantiateVrAvatarEventCode = 123;
		
		void Awake()
		{
			sceneManager.onCreateScene += OnCreateScene;
            PhotonNetwork.OnEventCall += OnEvent;
		}

		void OnDestroy()
		{
			sceneManager.onCreateScene -= OnCreateScene;
            PhotonNetwork.OnEventCall -= OnEvent;
		}

		public void QuitGame()
		{
			PhotonNetwork.LoadLevel("Lobby");
		}

		public void Mute(bool isMuted)
		{
			soundOffImage.SetActive(isMuted);
		}

		void OnCreateScene(ScenarioScene scene)
		{
			if (PhotonNetwork.isMasterClient) 
			{
				photonView.RPC("OnCreateSceneRPC", PhotonTargets.Others);
			}

			currentScene = scene;
			//Now we can create players
			CreatePlayer();
		}

		[PunRPC]
		void OnCreateSceneRPC()
		{
			currentScene = GameObject.FindObjectOfType<ScenarioScene>();
			CreatePlayer();
		}

        void CreatePlayer()
        {
            int viewId = PhotonNetwork.AllocateViewID();

            //if (photonView.isMine)
            //{
            //    int actorNumber = PhotonNetwork.player.ID;
            //    Debug.Log("CreatePlayer " + actorNumber.ToString());
            //    GameObject playerGO = PhotonNetwork.Instantiate("NetworkPlayer", currentScene.playerPositions[actorNumber].position,
            //        Quaternion.identity, 0);

            //    localPlayerController = playerGO.GetComponent<NetworkPlayerController>();
            //    localPlayerController.Init(this, photonView.isMine);
            //}

            PhotonNetwork.RaiseEvent(InstantiateVrAvatarEventCode, viewId, true, new RaiseEventOptions() { CachingOption = EventCaching.AddToRoomCache, Receivers = ReceiverGroup.All });
            GameObject voiceRecorderGO = PhotonNetwork.Instantiate("VoiceRecorder", Vector3.zero, Quaternion.identity, 0);
            voiceRecorderGO.GetComponent<VoiceController>().Init(this);
        }

        [PunRPC]
        void AssignViewID(int id)
        {
            localPlayerController.photonView.viewID = id;
        }

        private void OnEvent(byte eventcode, object content, int senderid)
        {
            if (eventcode == InstantiateVrAvatarEventCode)
            {
                bool isLocalPlayer = PhotonNetwork.player.ID == senderid;

                int actorNumber = PhotonNetwork.player.ID;

                Debug.Log("CreatePlayer " + actorNumber.ToString());
                //if (isLocalPlayer)
                //{
                //GameObject playerGO = PhotonNetwork.Instantiate("NetworkPlayer", currentScene.playerPositions[actorNumber].position,
                //    Quaternion.identity, 0);

                //NetworkPlayerController networkPlayerController = playerGO.GetComponent<NetworkPlayerController>();
                //}

                localPlayerController = GameObject.Instantiate<NetworkPlayerController>(networkPlayerControllerPrefab,
                    currentScene.playerPositions[actorNumber].position, Quaternion.identity);

                localPlayerController.Init(this, isLocalPlayer);
                if (isLocalPlayer)
                {
                    int playerID = PhotonNetwork.AllocateViewID();
                    photonView.RPC("AssignViewID", PhotonTargets.Others, playerID);
                    localPlayerController.photonView.viewID = playerID;
                }

                GameObject go = null;

                if (isLocalPlayer)
                {
                    go = Instantiate(Resources.Load("LocalAvatar"), localPlayerController.transform) as GameObject;
                }
                else
                {
                    //Destroy(networkPlayerController.gameObject);
                    go = Instantiate(Resources.Load("RemoteAvatar"), localPlayerController.transform) as GameObject;
                }

                if (go != null)
                {
                    PhotonView pView = go.GetComponent<PhotonView>();

                    if (pView != null)
                    {
                        pView.viewID = (int)content;
                    }
                }
            }
        }

		#region IPunObservable implementation
		public void OnPhotonSerializeView (PhotonStream stream, PhotonMessageInfo info)
		{
		}
		#endregion
	}
}