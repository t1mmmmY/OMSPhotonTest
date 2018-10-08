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
		[SerializeField] OMSSceneManager sceneManager;
		[SerializeField] Color[] colorPresets;
		[SerializeField] GameObject soundOffImage;

		ScenarioScene currentScene;
		
		void Awake()
		{
			sceneManager.onCreateScene += OnCreateScene;
		}

		void OnDestroy()
		{
			sceneManager.onCreateScene -= OnCreateScene;
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
			int actorNumber = PhotonNetwork.player.ID;
			Debug.Log ("CreatePlayer " + actorNumber.ToString ());
			GameObject playerGO = PhotonNetwork.Instantiate("NetworkPlayer", currentScene.playerPositions[actorNumber].position,
				Quaternion.identity, 0);

//			RigidbodyFirstPersonController playerController = playerGO.GetComponent<RigidbodyFirstPersonController>();
//			playerController.Init (colorPresets[actorNumber], this);

		}

		#region IPunObservable implementation
		public void OnPhotonSerializeView (PhotonStream stream, PhotonMessageInfo info)
		{
		}
		#endregion
	}
}