using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OMS.Networking
{

	public class NetworkGameManager : Photon.MonoBehaviour, IPunObservable
	{
        private static NetworkGameManager _instance;
        public static NetworkGameManager Instance
        {
            get
            {
                return _instance;
            }
            private set
            {
                _instance = value;
            }
        }

        [SerializeField] NetworkPlayerController networkPlayerControllerPrefab;
        [SerializeField] OMSSceneManager sceneManager;
		[SerializeField] Color[] colorPresets;
		[SerializeField] GameObject soundOffImage;

		ScenarioScene currentScene;
        NetworkPlayerController localPlayerController = null;

		void Awake()
		{
            Instance = this;
			sceneManager.onCreateScene += OnCreateScene;
		}

		void OnDestroy()
		{
			sceneManager.onCreateScene -= OnCreateScene;
		}

        void Start()
        {
            StartCoroutine("WaitToCreatePlayerCoroutine");
        }

        IEnumerator WaitToCreatePlayerCoroutine()
        {
            do
            {
                yield return new WaitForSeconds(0.5f);

                currentScene = GameObject.FindObjectOfType<ScenarioScene>();
                if (currentScene != null && localPlayerController == null)
                {
                    //Scene was already created and OcCreateSceneRPC was missed
                    CreatePlayer();
                    RestoreRemotePlayers();
                    yield break;
                }

            } while (localPlayerController == null);
        }

        void FixedUpdate()
        {
            if (currentScene != null && localPlayerController == null)
            {
                //Scene was already created and OcCreateSceneRPC was missed
                CreatePlayer();
            }
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

                currentScene = scene;
                //Now we can create players
                CreatePlayer();
			}
		}

		[PunRPC]
		void OnCreateSceneRPC()
		{
            if (currentScene == null)
            {
                currentScene = GameObject.FindObjectOfType<ScenarioScene>();
            }
			CreatePlayer();
		}

        void CreatePlayer()
        {
            int actorNumber = PhotonNetwork.player.ID;

            GameObject networkPlayerGO = PhotonNetwork.Instantiate("NetworkPlayer", 
                currentScene.playerPositions[actorNumber].position, currentScene.playerPositions[actorNumber].rotation, 0);
            localPlayerController = networkPlayerGO.GetComponent<NetworkPlayerController>();
            localPlayerController.Init();

            GameObject voiceRecorderGO = PhotonNetwork.Instantiate("VoiceRecorder", Vector3.zero, Quaternion.identity, 0);
            voiceRecorderGO.GetComponent<VoiceController>().Init(this);

            voiceRecorderGO.transform.parent = localPlayerController.transform;
            voiceRecorderGO.transform.localPosition = Vector3.zero;
        }

        public void RestoreRemotePlayers()
        {
            NetworkPlayerController[] allPlayerControllers = FindObjectsOfType<NetworkPlayerController>();
            foreach (NetworkPlayerController playerController in allPlayerControllers)
            {
                playerController.RestoreRemotePlayer();
            }    
        }


		#region IPunObservable implementation
		public void OnPhotonSerializeView (PhotonStream stream, PhotonMessageInfo info)
		{
		}
		#endregion
	}
}